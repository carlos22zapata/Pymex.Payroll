using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class CoinsRepository : ICoinsRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<CoinsRepository> _logger;

        public CoinsRepository(PayrollDbContext context, ILogger<CoinsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Coins>> GetById(int id)
        {
            try
            {
                var coin = await _context.Coins.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                if (coin == null)
                    return Result<Coins>.Failure("Moneda no encontrada.");
                return Result<Coins>.Success(coin);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener moneda ID: {Id}", id);
                return Result<Coins>.Failure("Error al consultar la moneda.");
            }
        }

        public async Task<Result<Coins>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<Coins> query = _context.Coins.AsNoTracking();
                Coins? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<Coins>.Failure("No hay registros disponibles.");
                return Result<Coins>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<Coins>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<Coins>>> GetList(int page, int pageSize, string? name)
        {
            try
            {
                var query = _context.Coins.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    var pattern = $"%{name}%";
                    query = query.Where(c => EF.Functions.ILike(c.Name, pattern) ||
                                             EF.Functions.ILike(c.Symbol, pattern));
                }

                var skip = (page - 1) * pageSize;
                var list = await query
                    .OrderBy(c => c.Name)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToListAsync();

                return Result<List<Coins>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar monedas.");
                return Result<List<Coins>>.Failure("No se pudo obtener la lista de monedas.");
            }
        }

        public async Task<Result<bool>> Insert(Coins coin)
        {
            try
            {
                bool exists = await _context.Coins.AnyAsync(c => c.Name == coin.Name || c.Symbol == coin.Symbol);
                if (exists)
                    return Result<bool>.Failure("Ya existe una moneda con ese nombre o símbolo.");

                await _context.Coins.AddAsync(coin);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar moneda.");
                return Result<bool>.Failure("Error al guardar la moneda.");
            }
        }

        public async Task<Result<bool>> Update(Coins coin)
        {
            try
            {
                _context.Coins.Update(coin);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar moneda ID: {Id}", coin.Id);
                return Result<bool>.Failure("Error al actualizar la moneda.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var coin = await _context.Coins.FindAsync(id);
                if (coin == null)
                    return Result<bool>.Failure("La moneda no existe.");

                _context.Coins.Remove(coin);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (DbUpdateException)
            {
                return Result<bool>.Failure("No se puede eliminar la moneda porque tiene cotizaciones vinculadas.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar moneda ID: {Id}", id);
                return Result<bool>.Failure("Error al eliminar la moneda.");
            }
        }
    }
}
