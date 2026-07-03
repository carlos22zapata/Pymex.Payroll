using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class PayrollNoveltyRepository : IPayrollNoveltyRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<PayrollNoveltyRepository> _logger;

        public PayrollNoveltyRepository(PayrollDbContext context, ILogger<PayrollNoveltyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PayrollNovelty>> GetById(int id)
        {
            try
            {
                var entity = await _context.PayrollNovelties
                    .AsNoTracking()
                    .Include(pn => pn.Contract)
                    .Include(pn => pn.PayrollVariable)
                    .FirstOrDefaultAsync(pn => pn.Id == id);
                if (entity == null)
                    return Result<PayrollNovelty>.Failure("Novedad de nómina no encontrada.");
                return Result<PayrollNovelty>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener novedad ID: {Id}", id);
                return Result<PayrollNovelty>.Failure("Error al consultar la novedad.");
            }
        }

        public async Task<Result<PayrollNovelty>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<PayrollNovelty> query = _context.PayrollNovelties
                    .AsNoTracking()
                    .Include(pn => pn.Contract)
                    .Include(pn => pn.PayrollVariable);
                PayrollNovelty? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<PayrollNovelty>.Failure("No hay registros disponibles.");
                return Result<PayrollNovelty>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<PayrollNovelty>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<PayrollNovelty>>> GetByContractId(int contractId)
        {
            try
            {
                var list = await _context.PayrollNovelties
                    .AsNoTracking()
                    .Include(pn => pn.Contract)
                    .Include(pn => pn.PayrollVariable)
                    .Where(pn => pn.ContractId == contractId)
                    .OrderBy(pn => pn.PeriodCode)
                    .ToListAsync();
                return Result<List<PayrollNovelty>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener novedades del contrato ID: {Id}", contractId);
                return Result<List<PayrollNovelty>>.Failure("Error al consultar novedades del contrato.");
            }
        }

        public async Task<Result<bool>> Insert(PayrollNovelty payrollNovelty)
        {
            try
            {
                bool exists = await _context.PayrollNovelties
                    .AnyAsync(pn => pn.ContractId == payrollNovelty.ContractId && pn.PayrollVariableId == payrollNovelty.PayrollVariableId && pn.PeriodCode == payrollNovelty.PeriodCode);
                if (exists)
                    return Result<bool>.Failure("Ya existe una novedad con la misma variable y período para este contrato.");
                await _context.PayrollNovelties.AddAsync(payrollNovelty);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar novedad.");
                return Result<bool>.Failure($"Error al guardar la novedad: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(PayrollNovelty payrollNovelty)
        {
            try
            {
                _context.PayrollNovelties.Update(payrollNovelty);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar novedad ID: {Id}", payrollNovelty.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.PayrollNovelties.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("La novedad no existe.");
                _context.PayrollNovelties.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar novedad ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar la novedad.");
            }
        }
    }
}