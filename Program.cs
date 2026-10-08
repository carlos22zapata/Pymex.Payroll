using Microsoft.EntityFrameworkCore;
using Pymex.Auth.Data;
using Pymex.Auth.Extensions;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Infrastructure;
using Pymex.Payroll.Mappers;
using Pymex.Payroll.Middleware;
using Pymex.Payroll.Repositories;
using Pymex.Payroll.Repositories.Interfaces;
using Pymex.Payroll.Services;
using Pymex.Payroll.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Kestrel ports
var useKestrel = builder.Configuration.GetValue<bool>("ServerOptions:UseKestrel");
if (useKestrel)
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        var httpPort = builder.Configuration.GetValue<int>("ServerOptions:KestrelSettings:HttpPort");
        var httpsPort = builder.Configuration.GetValue<int>("ServerOptions:KestrelSettings:HttpsPort");
        var sslPath = builder.Configuration.GetValue<string>("ServerOptions:KestrelSettings:SslPath");
        var sslPassword = builder.Configuration.GetValue<string>("ServerOptions:KestrelSettings:SslPassword");

        options.ListenAnyIP(httpPort > 0 ? httpPort : 7600);
        if (httpsPort > 0)
        {
            options.ListenAnyIP(httpsPort, listenOptions =>
            {
                listenOptions.UseHttps(sslPath, sslPassword);
            });
        }
    });
}
else
{
    builder.WebHost.UseIIS();
}

// CORS
var _PymexCore = "PymexCore";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: _PymexCore, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers(o =>
    {
        o.ModelBinderProviders.Insert(0, new Pymex.Payroll.Infrastructure.AppClockModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new Pymex.Shared.Time.JsonDateTimeConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityDefinition("ConexName", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "ConexName",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nombre de la conexi�n/tenant. Ej: ConexSQLEPayroll"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ConexName"
                }
            },
            new string[] {}
        }
    });
});

// JWT validation (same key as Pymex.Auth)
builder.Services.AddJWTTokenServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization();

// MasterDbContext (from Pymex.Auth)
builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ConexSQLMaster")));

// Tenant connection provider
builder.Services.AddScoped<ITenantConnectionProvider, TenantConnectionProvider>();

// DbContext factory for migrations
builder.Services.AddTransient<IDbContextFactory<PayrollDbContext>, PayrollDbContextFactory>();
builder.Services.AddTransient<PayrollDbContextFactory>();

// PayrollDbContext with dynamic tenant resolution
builder.Services.AddScoped<PayrollDbContext>(provider =>
{
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    var configuration = provider.GetRequiredService<IConfiguration>();
    var tenantProvider = provider.GetService<ITenantConnectionProvider>();
    var masterContext = provider.GetRequiredService<MasterDbContext>();

    string connectionString = null;
    string conexName = null;

    if (tenantProvider != null)
    {
        try { conexName = tenantProvider.GetConnectionName(); }
        catch { conexName = null; }
    }

    if (string.IsNullOrEmpty(conexName))
    {
        conexName = httpContextAccessor.HttpContext?.Request?.Headers["ConexName"].FirstOrDefault();
    }

    if (!string.IsNullOrEmpty(conexName))
    {
        var ent = masterContext.Enterprises.AsNoTracking().FirstOrDefault(e => e.Database == conexName);
        if (ent != null)
        {
            if (!string.IsNullOrEmpty(ent.CustomConnectionString))
            {
                connectionString = ent.CustomConnectionString;
            }
            else if (!string.IsNullOrEmpty(ent.Database))
            {
                var masterConn = configuration.GetConnectionString("ConexSQLMaster") ?? string.Empty;
                string host = "localhost";
                string user = "postgres";
                string pwd = "";

                var parts = masterConn.Split(';', StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var kv = p.Split('=', 2);
                    if (kv.Length != 2) continue;
                    var key = kv[0].Trim().ToLowerInvariant();
                    var val = kv[1].Trim();
                    if (key == "host") host = val;
                    if (key == "username" || key == "user id" || key == "user") user = val;
                    if (key == "password" || key == "pwd") pwd = val;
                }

                connectionString = $"Host={host};Database={ent.Database};Username={user};Password={pwd}";
            }
        }
    }

    if (string.IsNullOrEmpty(connectionString) && !string.IsNullOrEmpty(conexName))
    {
        connectionString = configuration.GetConnectionString(conexName);
    }

    if (string.IsNullOrEmpty(connectionString))
    {
        var first = configuration.GetSection("ConnectionStrings").GetChildren()
                        .FirstOrDefault(c => c.Key.StartsWith("ConexSQLE", StringComparison.OrdinalIgnoreCase));
        if (first != null)
        {
            connectionString = first.Value;
        }
    }

    if (string.IsNullOrEmpty(connectionString))
        throw new InvalidOperationException("Tenant connection string could not be resolved. Ensure Master.Enterprises contains the tenant or a connection string exists in configuration.");

    var optionsBuilder = new DbContextOptionsBuilder<PayrollDbContext>();
    optionsBuilder.UseNpgsql(connectionString, opts => opts.MigrationsHistoryTable("__EFMigrationsHistory", "enterprise"));

    return new PayrollDbContext(optionsBuilder.Options, httpContextAccessor);
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappersProfile).Assembly);

builder.Services.AddHttpClient();
builder.Services.AddLogging();

// Repository registrations
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IContractConceptRepository, ContractConceptRepository>();
builder.Services.AddScoped<IPayrollConceptRepository, PayrollConceptRepository>();
builder.Services.AddScoped<IPayrollVariableRepository, PayrollVariableRepository>();
builder.Services.AddScoped<IContractVariableRepository, ContractVariableRepository>();
builder.Services.AddScoped<IPayrollNoveltyRepository, PayrollNoveltyRepository>();
builder.Services.AddScoped<IPayrollCalculationRepository, PayrollCalculationRepository>();
builder.Services.AddScoped<ICoinsRepository, CoinsRepository>();
builder.Services.AddScoped<ICoinQuotationRepository, CoinQuotationRepository>();
builder.Services.AddScoped<IRelatedContractRepository, RelatedContractRepository>();

// Service registrations
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IContractConceptService, ContractConceptService>();
builder.Services.AddScoped<IPayrollConceptService, PayrollConceptService>();
builder.Services.AddScoped<IPayrollVariableService, PayrollVariableService>();
builder.Services.AddScoped<IContractVariableService, ContractVariableService>();
builder.Services.AddScoped<IPayrollNoveltyService, PayrollNoveltyService>();
builder.Services.AddScoped<IPayrollCalculationService, PayrollCalculationService>();
builder.Services.AddScoped<ICoinsService, CoinsService>();
builder.Services.AddScoped<ICoinQuotationService, CoinQuotationService>();
builder.Services.AddScoped<IRelatedContractService, RelatedContractService>();

var app = builder.Build();

// Database initialization
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        await DatabaseInitializer.InitializeAsync(app.Services, app.Configuration, logger);
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error during database initialization");
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pymex Payroll API V1");
    c.RoutePrefix = "swagger";
});

app.UseCors(_PymexCore);

app.UseMiddleware<TenantMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
