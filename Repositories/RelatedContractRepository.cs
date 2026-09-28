using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class RelatedContractRepository : IRelatedContractRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<RelatedContractRepository> _logger;

        public RelatedContractRepository(PayrollDbContext context, ILogger<RelatedContractRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<RelatedContract>> GetById(int id)
        {
            try
            {
                var entity = await _context.RelatedContracts
                    .AsNoTracking()
                    .Include(rc => rc.Contract)
                    .Include(rc => rc.RelatedContractRef)
                    .FirstOrDefaultAsync(rc => rc.Id == id);
                if (entity == null)
                    return Result<RelatedContract>.Failure("Contrato relacionado no encontrado.");
                return Result<RelatedContract>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contrato relacionado ID: {Id}", id);
                return Result<RelatedContract>.Failure("Error al consultar el contrato relacionado.");
            }
        }

        public async Task<Result<RelatedContract>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<RelatedContract> query = _context.RelatedContracts
                    .AsNoTracking()
                    .Include(rc => rc.Contract)
                    .Include(rc => rc.RelatedContractRef);

                RelatedContract? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };

                if (target == null)
                    return Result<RelatedContract>.Failure("No hay registros disponibles.");

                return Result<RelatedContract>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar de contratos relacionados.");
                return Result<RelatedContract>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<RelatedContract>>> GetByContractId(int contractId)
        {
            try
            {
                var list = await _context.RelatedContracts
                    .AsNoTracking()
                    .Include(rc => rc.Contract)
                    .Include(rc => rc.RelatedContractRef)
                    .Where(rc => rc.ContractId == contractId || rc.RelatedContractId == contractId)
                    .OrderBy(rc => rc.Contract.Name)
                    .ToListAsync();
                return Result<List<RelatedContract>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contratos relacionados del contrato ID: {Id}", contractId);
                return Result<List<RelatedContract>>.Failure("Error al consultar contratos relacionados.");
            }
        }

        public async Task<Result<bool>> Insert(RelatedContract relatedContract)
        {
            try
            {
                bool exists = await _context.RelatedContracts
                    .AnyAsync(rc => rc.ContractId == relatedContract.ContractId && rc.RelatedContractId == relatedContract.RelatedContractId);

                if (exists)
                    return Result<bool>.Failure("Esta relación entre contratos ya existe.");

                if (relatedContract.ContractId == relatedContract.RelatedContractId)
                    return Result<bool>.Failure("Un contrato no puede estar relacionado consigo mismo.");

                await _context.RelatedContracts.AddAsync(relatedContract);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar contrato relacionado.");
                return Result<bool>.Failure($"Error al guardar el contrato relacionado: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(RelatedContract relatedContract)
        {
            try
            {
                _context.RelatedContracts.Update(relatedContract);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar contrato relacionado ID: {Id}", relatedContract.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.RelatedContracts.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("El contrato relacionado no existe.");

                _context.RelatedContracts.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar contrato relacionado ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el contrato relacionado.");
            }
        }
    }
}
