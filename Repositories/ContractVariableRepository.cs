using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class ContractVariableRepository : IContractVariableRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<ContractVariableRepository> _logger;

        public ContractVariableRepository(PayrollDbContext context, ILogger<ContractVariableRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<ContractVariable>> GetById(int id)
        {
            try
            {
                var entity = await _context.ContractVariables
                    .AsNoTracking()
                    .Include(cv => cv.Contract)
                    .Include(cv => cv.PayrollVariable)
                        .ThenInclude(pv => pv.Coin)
                    .FirstOrDefaultAsync(cv => cv.Id == id);
                if (entity == null)
                    return Result<ContractVariable>.Failure("Variable de contrato no encontrada.");
                return Result<ContractVariable>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener variable de contrato ID: {Id}", id);
                return Result<ContractVariable>.Failure("Error al consultar la variable de contrato.");
            }
        }

        public async Task<Result<ContractVariable>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<ContractVariable> query = _context.ContractVariables
                    .AsNoTracking()
                    .Include(cv => cv.Contract)
                    .Include(cv => cv.PayrollVariable)
                        .ThenInclude(pv => pv.Coin);
                ContractVariable? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<ContractVariable>.Failure("No hay registros disponibles.");
                return Result<ContractVariable>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<ContractVariable>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<ContractVariable>>> GetByContractId(int contractId)
        {
            try
            {
                var list = await _context.ContractVariables
                    .AsNoTracking()
                    .Include(cv => cv.Contract)
                    .Include(cv => cv.PayrollVariable)
                        .ThenInclude(pv => pv.Coin)
                    .Where(cv => cv.ContractId == contractId)
                    .OrderBy(cv => cv.PayrollVariable.Name)
                    .ToListAsync();
                return Result<List<ContractVariable>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener variables del contrato ID: {Id}", contractId);
                return Result<List<ContractVariable>>.Failure("Error al consultar variables del contrato.");
            }
        }

        public async Task<Result<bool>> Insert(ContractVariable contractVariable)
        {
            try
            {
                bool exists = await _context.ContractVariables
                    .AnyAsync(cv => cv.ContractId == contractVariable.ContractId && cv.PayrollVariableId == contractVariable.PayrollVariableId);
                if (exists)
                    return Result<bool>.Failure("La variable ya está asociada a este contrato.");
                await _context.ContractVariables.AddAsync(contractVariable);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar variable de contrato.");
                return Result<bool>.Failure($"Error al guardar la variable de contrato: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(ContractVariable contractVariable)
        {
            try
            {
                _context.ContractVariables.Update(contractVariable);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar variable de contrato ID: {Id}", contractVariable.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.ContractVariables.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("La variable de contrato no existe.");
                _context.ContractVariables.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar variable de contrato ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar la variable de contrato.");
            }
        }
    }
}