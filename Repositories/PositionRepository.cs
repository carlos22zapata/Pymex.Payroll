using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<PositionRepository> _logger;

        public PositionRepository(PayrollDbContext context, ILogger<PositionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Position>> GetById(int id)
        {
            try
            {
                var position = await _context.Positions.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (position == null)
                    return Result<Position>.Failure("Cargo no encontrado.");

                return Result<Position>.Success(position);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cargo ID: {Id}", id);
                return Result<Position>.Failure("Error al consultar el cargo.");
            }
        }

        public async Task<Result<Position>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<Position> query = _context.Positions.AsNoTracking();
                Position? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<Position>.Failure("No hay registros disponibles.");
                return Result<Position>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<Position>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<Position>>> GetList(string? name)
        {
            try
            {
                var query = _context.Positions.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(p => EF.Functions.ILike(p.Name, $"%{name}%"));
                }

                var list = await query.OrderBy(p => p.Name).ToListAsync();
                return Result<List<Position>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar cargos.");
                return Result<List<Position>>.Failure("No se pudo obtener la lista de cargos.");
            }
        }

        public async Task<Result<bool>> Insert(Position position)
        {
            try
            {
                bool exists = await _context.Positions.AnyAsync(p => p.Name == position.Name);
                if (exists)
                    return Result<bool>.Failure($"Ya existe un cargo con el nombre: {position.Name}");

                await _context.Positions.AddAsync(position);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar cargo.");
                return Result<bool>.Failure($"Error al guardar el cargo: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(Position position)
        {
            try
            {
                _context.Positions.Update(position);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cargo ID: {Id}", position.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var position = await _context.Positions.FindAsync(id);
                if (position == null)
                    return Result<bool>.Failure("El cargo no existe.");

                _context.Positions.Remove(position);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar cargo ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el cargo. Verifique si tiene empleados asociados.");
            }
        }
    }
}
