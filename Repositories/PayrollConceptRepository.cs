using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class PayrollConceptRepository : IPayrollConceptRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<PayrollConceptRepository> _logger;

        public PayrollConceptRepository(PayrollDbContext context, ILogger<PayrollConceptRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<PayrollConcept>> GetById(int id)
        {
            try
            {
                var entity = await _context.PayrollConcepts.AsNoTracking().FirstOrDefaultAsync(pc => pc.Id == id);
                if (entity == null)
                    return Result<PayrollConcept>.Failure("Concepto de nómina no encontrado.");
                return Result<PayrollConcept>.Success(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener concepto de nómina ID: {Id}", id);
                return Result<PayrollConcept>.Failure("Error al consultar el concepto de nómina.");
            }
        }

        public async Task<Result<PayrollConcept>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<PayrollConcept> query = _context.PayrollConcepts.AsNoTracking();
                PayrollConcept? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<PayrollConcept>.Failure("No hay registros disponibles.");
                return Result<PayrollConcept>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<PayrollConcept>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<PayrollConcept>>> GetList(string? name)
        {
            try
            {
                var query = _context.PayrollConcepts.AsNoTracking();
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(pc => EF.Functions.ILike(pc.Name, $"%{name}%") || EF.Functions.ILike(pc.Code, $"%{name}%"));
                var list = await query.OrderBy(pc => pc.Name).ToListAsync();
                return Result<List<PayrollConcept>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar conceptos de nómina.");
                return Result<List<PayrollConcept>>.Failure("No se pudo obtener la lista de conceptos de nómina.");
            }
        }

        public async Task<Result<bool>> Insert(PayrollConcept payrollConcept)
        {
            try
            {
                bool exists = await _context.PayrollConcepts.AnyAsync(pc => pc.Code == payrollConcept.Code);
                if (exists)
                    return Result<bool>.Failure($"Ya existe un concepto con el código: {payrollConcept.Code}");
                await _context.PayrollConcepts.AddAsync(payrollConcept);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar concepto de nómina.");
                return Result<bool>.Failure($"Error al guardar el concepto: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(PayrollConcept payrollConcept)
        {
            try
            {
                _context.PayrollConcepts.Update(payrollConcept);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar concepto de nómina ID: {Id}", payrollConcept.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var entity = await _context.PayrollConcepts.FindAsync(id);
                if (entity == null)
                    return Result<bool>.Failure("El concepto de nómina no existe.");
                _context.PayrollConcepts.Remove(entity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar concepto de nómina ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el concepto de nómina.");
            }
        }
    }
}
