using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<ContractRepository> _logger;

        public ContractRepository(PayrollDbContext context, ILogger<ContractRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Contract>> GetById(int id)
        {
            try
            {
                var contract = await _context.Contracts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (contract == null)
                    return Result<Contract>.Failure("Contrato no encontrado.");
                return Result<Contract>.Success(contract);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener contrato ID: {Id}", id);
                return Result<Contract>.Failure("Error al consultar el contrato.");
            }
        }

        public async Task<Result<Contract>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<Contract> query = _context.Contracts.AsNoTracking();
                Contract? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<Contract>.Failure("No hay registros disponibles.");
                return Result<Contract>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<Contract>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<Contract>>> GetList(string? name)
        {
            try
            {
                var query = _context.Contracts.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(c => EF.Functions.ILike(c.Name, $"%{name}%"));
                var list = await query.OrderBy(c => c.Name).ToListAsync();
                return Result<List<Contract>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar contratos.");
                return Result<List<Contract>>.Failure("No se pudo obtener la lista de contratos.");
            }
        }

        public async Task<Result<bool>> Insert(Contract contract)
        {
            try
            {
                bool exists = await _context.Contracts.AnyAsync(c => c.Name == contract.Name);
                if (exists)
                    return Result<bool>.Failure($"Ya existe un contrato con el nombre: {contract.Name}");
                await _context.Contracts.AddAsync(contract);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar contrato.");
                return Result<bool>.Failure($"Error al guardar el contrato: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(Contract contract)
        {
            try
            {
                _context.Contracts.Update(contract);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar contrato ID: {Id}", contract.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var contract = await _context.Contracts.FindAsync(id);
                if (contract == null)
                    return Result<bool>.Failure("El contrato no existe.");
                _context.Contracts.Remove(contract);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar contrato ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el contrato. Verifique si tiene empleados asociados.");
            }
        }
    }
}
