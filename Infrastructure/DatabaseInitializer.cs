using Microsoft.EntityFrameworkCore;
using Pymex.Auth.Data;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Enums;

namespace Pymex.Payroll.Infrastructure
{
    public static class DatabaseInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
        {
            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var masterContext = serviceProvider.GetService<MasterDbContext>();
            if (masterContext != null)
            {
                try
                {
                    await masterContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error migrating MasterDbContext");
                }

                try
                {
                    await SeedMasterAsync(masterContext);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error seeding MasterDbContext");
                }
            }

            var connectionStrings = configuration.GetSection("ConnectionStrings").GetChildren();
            var dbContextFactory = serviceProvider.GetService<PayrollDbContextFactory>();

            foreach (var cs in connectionStrings.Where(c => c.Key.StartsWith("ConexSQLE", StringComparison.OrdinalIgnoreCase)))
            {
                try
                {
                    var conn = cs.Value;
                    if (string.IsNullOrWhiteSpace(conn))
                    {
                        logger.LogWarning("Connection string {Key} is empty, skipping.", cs.Key);
                        continue;
                    }

                    try
                    {
                        var npg = new Npgsql.NpgsqlConnectionStringBuilder(conn);
                        if (npg.Database.Equals("PymexMaster", StringComparison.OrdinalIgnoreCase))
                        {
                            logger.LogInformation("Skipping migration for master database found in connection string {Key}", cs.Key);
                            continue;
                        }
                    }
                    catch { }

                    if (dbContextFactory != null)
                    {
                        var tenantContext = dbContextFactory.CreateDbContext(conn);
                        if (tenantContext != null)
                        {
                            try
                            {
                                await tenantContext.Database.MigrateAsync();
                                logger.LogInformation("Migrated tenant DB from config key {Key}", cs.Key);

                                await SeedTenantAsync(tenantContext);
                            }
                            catch (Exception ex)
                            {
                                logger.LogError(ex, "Error migrating tenant DB for {Key}", cs.Key);
                            }
                            finally
                            {
                                await tenantContext.DisposeAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Unhandled error processing connection string {Key}", cs.Key);
                }
            }
        }

        private static async Task SeedMasterAsync(MasterDbContext context)
        {
            // Master data is seeded by Pymex.Auth, nothing extra needed here
            await Task.CompletedTask;
        }

        private static async Task SeedTenantAsync(PayrollDbContext context)
        {
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(new List<Department>
                {
                    new Department { Name = "Recursos Humanos" },
                    new Department { Name = "Tecnología" },
                    new Department { Name = "Contabilidad" }
                });
                await context.SaveChangesAsync();
            }

            if (!context.Positions.Any())
            {
                context.Positions.AddRange(new List<Position>
                {
                    new Position { Name = "Gerente" },
                    new Position { Name = "Supervisor" },
                    new Position { Name = "Analista" },
                    new Position { Name = "Asistente" }
                });
                await context.SaveChangesAsync();
            }

            if (!context.Coins.Any())
            {
                context.Coins.Add(new Coins { Enabled = true, Name = "Bolivares", Symbol = "VES" });
                context.Coins.Add(new Coins { Enabled = true, Name = "Dólares", Symbol = "USD" });
                await context.SaveChangesAsync();
            }

            if (!context.CoinQuotations.Any())
            {
                var coinId = context.Coins.Select(s => s.Id).FirstOrDefault();
                context.CoinQuotations.Add(new CoinQuotations { Date = DateTime.Now, Observation = "", Value = 1, CoinId = coinId, Origin = 0 });
                await context.SaveChangesAsync();
            }

            if (!context.PayrollVariables.Any())
            {
                var payrollVariables = new List<PayrollVariable>
                {
                    // --- Variables Base y de Clasificación ---
                    new PayrollVariable { Code = "V_SUELDO_BASE", Name = "Sueldo Mensual Base", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_TIPO_SUELDO", Name = "Tipo de Sueldo (1=Mensual, 2=Diario)", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_CARGA_FAM", Name = "Cantidad de Cargas Familiares", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_PORC_ISLR", Name = "Porcentaje Retención ISLR", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },

                    // --- Variables de Tiempo y Asistencia ---
                    new PayrollVariable { Code = "V_DIAS_TRAB", Name = "Días Trabajados en el Periodo", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },
                    new PayrollVariable { Code = "V_FALTAS_INJ", Name = "Días de Faltas Injustificadas", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },
                    new PayrollVariable { Code = "V_DIAS_REPOSO", Name = "Días de Reposo (IVSS)", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },
                    new PayrollVariable { Code = "V_LUNES_MES", Name = "Lunes del Mes (4 o 5)", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },

                    // --- Variables de Tiempo Extraordinario ---
                    new PayrollVariable { Code = "V_HE_DIURNAS", Name = "Cantidad Horas Extras Diurnas", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },
                    new PayrollVariable { Code = "V_HE_NOCT", Name = "Cantidad Horas Extras Nocturnas", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },
                    new PayrollVariable { Code = "V_HORAS_NOCT", Name = "Horas Ordinarias Nocturnas", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },
                    new PayrollVariable { Code = "V_FERIADOS_TR", Name = "Días Feriados Trabajados", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },

                    // --- Beneficios y Leyes Conexas ---
                    new PayrollVariable { Code = "V_VALOR_CESTA", Name = "Valor Cestaticket Actual", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_DESC_CESTA", Name = "Días a Descontar Cestaticket", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Volatile },

                    // --- Retenciones Estándar de Ley ---
                    new PayrollVariable { Code = "V_PORC_IVSS", Name = "Porcentaje Retención IVSS", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_PORC_RPE", Name = "Porcentaje Retención Paro Forzoso", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_PORC_FAOV", Name = "Porcentaje Retención FAOV", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },
                    new PayrollVariable { Code = "V_TOPE_IVSS", Name = "Tope Salarios Mínimos IVSS", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Fixed },

                    // --- Acumulados y Prestaciones Sociales ---
                    new PayrollVariable { Code = "V_ANTIGUEDAD", Name = "Días de Antigüedad Acumulados", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Calculated },
                    new PayrollVariable { Code = "V_SAL_NORMAL", Name = "Salario Normal Actual", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Calculated },
                    new PayrollVariable { Code = "V_SAL_INT", Name = "Salario Integral Actual", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Calculated },
                    new PayrollVariable { Code = "V_ALIC_UTIL", Name = "Alícuota de Utilidades", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Calculated },
                    new PayrollVariable { Code = "V_ALIC_BV", Name = "Alícuota de Bono Vacacional", DataType = VariableDataType.Numeric, Behavior = PersistenceBehavior.Calculated }
                };
                var defaultCoinId = context.Coins.Select(c => c.Id).First();
                foreach (var variable in payrollVariables)
                {
                    variable.CoinId = defaultCoinId;
                }
                context.PayrollVariables.AddRange(payrollVariables);
                await context.SaveChangesAsync();
            }

            if(!context.Contracts.Any())
            {
                context.Contracts.AddRange(new List<Contract>
                {
                    new Contract { Name = "Contrato Quincenal", Description = "Contrato quincenal LOTTT", IsActive = true },
                    new Contract { Name = "Contrato Mensual", Description = "Contrato mensual LOTTT", IsActive = true }
                });
                await context.SaveChangesAsync();
            }

            if (!context.Settings.Any())
            {
                context.Settings.Add(new Setting { Decimals = 2, ThousandsSeparator = ",", DecimalSeparator = "." });
                await context.SaveChangesAsync();
            }
        }
    }
}
