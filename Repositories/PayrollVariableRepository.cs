using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class PayrollVariableRepository : IPayrollVariableRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<PayrollVariableRepository> _logger;

        public PayrollVariableRepository(PayrollDbContext context, ILogger<PayrollVariableRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PayrollVariable>> GetById(int id)
        {
            try
            {
                var entity = await _context.PayrollVariables.AsNoTracking().FirstOrDefaultAsync(pv => pv.Id == id);
                if (entity == null)
                    return Result<PayrollVariable>.Failure("Variable de nómina no encontrada.");
                return Result<PayrollVariable>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener variable de nómina ID: {Id}", id);
                return Result<PayrollVariable>.Failure("Error al consultar la variable de nómina.");
            }
        }

        public async Task<Result<PayrollVariable>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<PayrollVariable> query = _context.PayrollVariables.AsNoTracking();
                PayrollVariable? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<PayrollVariable>.Failure("No hay registros disponibles.");
                return Result<PayrollVariable>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<PayrollVariable>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<PayrollVariable>>> GetList(string? name)
        {
            try
            {
                var query = _context.PayrollVariables.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(pv => EF.Functions.ILike(pv.Name, $"%{name}%") || EF.Functions.ILike(pv.Code, $"%{name}%"));
                var list = await query.OrderBy(pv => pv.Name).ToListAsync();
                return Result<List<PayrollVariable>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar variables de nómina.");
                return Result<List<PayrollVariable>>.Failure("No se pudo obtener la lista de variables de nómina.");
            }
        }

        public async Task<Result<bool>> Insert(PayrollVariable payrollVariable)
        {
            try
            {
                bool exists = await _context.PayrollVariables.AnyAsync(pv => pv.Code == payrollVariable.Code);
                if (exists)
                    return Result<bool>.Failure($"Ya existe una variable con el código: {payrollVariable.Code}");
                await _context.PayrollVariables.AddAsync(payrollVariable);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar variable de nómina.");
                return Result<bool>.Failure($"Error al guardar la variable: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(PayrollVariable payrollVariable)
        {
            try
            {
                _context.PayrollVariables.Update(payrollVariable);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar variable de nómina ID: {Id}", payrollVariable.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.PayrollVariables.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("La variable de nómina no existe.");
                _context.PayrollVariables.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar variable de nómina ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar la variable de nómina.");
            }
        }
    }
}
