using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class ContractConceptRepository : IContractConceptRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<ContractConceptRepository> _logger;

        public ContractConceptRepository(PayrollDbContext context, ILogger<ContractConceptRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<ContractConcept>> GetById(int id)
        {
            try
            {
                var entity = await _context.ContractConcepts
                    .AsNoTracking()
                    .Include(cc => cc.Contract)
                    .Include(cc => cc.PayrollConcept)
                    .FirstOrDefaultAsync(cc => cc.Id == id);
                if (entity == null)
                    return Result<ContractConcept>.Failure("Concepto de contrato no encontrado.");
                return Result<ContractConcept>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener concepto de contrato ID: {Id}", id);
                return Result<ContractConcept>.Failure("Error al consultar el concepto de contrato.");
            }
        }

        public async Task<Result<ContractConcept>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<ContractConcept> query = _context.ContractConcepts
                    .AsNoTracking()
                    .Include(cc => cc.Contract)
                    .Include(cc => cc.PayrollConcept);
                ContractConcept? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<ContractConcept>.Failure("No hay registros disponibles.");
                return Result<ContractConcept>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<ContractConcept>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<ContractConcept>>> GetByContractId(int contractId)
        {
            try
            {
                var list = await _context.ContractConcepts
                    .AsNoTracking()
                    .Include(cc => cc.Contract)
                    .Include(cc => cc.PayrollConcept)
                    .Where(cc => cc.ContractId == contractId)
                    .OrderBy(cc => cc.PayrollConcept.Name)
                    .ToListAsync();
                return Result<List<ContractConcept>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener conceptos del contrato ID: {Id}", contractId);
                return Result<List<ContractConcept>>.Failure("Error al consultar conceptos del contrato.");
            }
        }

        public async Task<Result<bool>> Insert(ContractConcept contractConcept)
        {
            try
            {
                bool exists = await _context.ContractConcepts
                    .AnyAsync(cc => cc.ContractId == contractConcept.ContractId && cc.PayrollConceptId == contractConcept.PayrollConceptId);
                if (exists)
                    return Result<bool>.Failure("El concepto ya está asociado a este contrato.");
                await _context.ContractConcepts.AddAsync(contractConcept);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar concepto de contrato.");
                return Result<bool>.Failure($"Error al guardar el concepto de contrato: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(ContractConcept contractConcept)
        {
            try
            {
                _context.ContractConcepts.Update(contractConcept);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar concepto de contrato ID: {Id}", contractConcept.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.ContractConcepts.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("El concepto de contrato no existe.");
                _context.ContractConcepts.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar concepto de contrato ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el concepto de contrato.");
            }
        }
    }
}
