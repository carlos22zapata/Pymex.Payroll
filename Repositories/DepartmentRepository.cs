using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(PayrollDbContext context, ILogger<DepartmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Department>> GetById(int id)
        {
            try
            {
                var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                if (department == null)
                    return Result<Department>.Failure("Departamento no encontrado.");

                return Result<Department>.Success(department);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener departamento ID: {Id}", id);
                return Result<Department>.Failure("Error al consultar el departamento.");
            }
        }

        public async Task<Result<Department>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<Department> query = _context.Departments.AsNoTracking();
                Department? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<Department>.Failure("No hay registros disponibles.");
                return Result<Department>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<Department>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<Department>>> GetList(string? name)
        {
            try
            {
                var query = _context.Departments.AsNoTracking();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(d => EF.Functions.ILike(d.Name, $"%{name}%"));
                }

                var list = await query.OrderBy(d => d.Name).ToListAsync();
                return Result<List<Department>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar departamentos.");
                return Result<List<Department>>.Failure("No se pudo obtener la lista de departamentos.");
            }
        }

        public async Task<Result<bool>> Insert(Department department)
        {
            try
            {
                bool exists = await _context.Departments.AnyAsync(d => d.Name == department.Name);
                if (exists)
                    return Result<bool>.Failure($"Ya existe un departamento con el nombre: {department.Name}");

                await _context.Departments.AddAsync(department);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar departamento.");
                return Result<bool>.Failure($"Error al guardar el departamento: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(Department department)
        {
            try
            {
                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar departamento ID: {Id}", department.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var department = await _context.Departments.FindAsync(id);
                if (department == null)
                    return Result<bool>.Failure("El departamento no existe.");

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar departamento ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el departamento. Verifique si tiene empleados asociados.");
            }
        }
    }
}
