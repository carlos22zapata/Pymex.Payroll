using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Contexts;
using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;
using Pymex.Payroll.Repositories.Interfaces;

namespace Pymex.Payroll.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PayrollDbContext _context;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(PayrollDbContext context, ILogger<EmployeeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Employee>> GetById(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Include(e => e.Position)
                    .Include(e => e.Contract)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                    return Result<Employee>.Failure("Empleado no encontrado.");

                return Result<Employee>.Success(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleado ID: {Id}", id);
                return Result<Employee>.Failure("Error al consultar el empleado.");
            }
        }

        public async Task<Result<Employee>> GetNavBarById(int navPositionId, int idReference)
        {
            try
            {
                IQueryable<Employee> query = _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Include(e => e.Position)
                    .Include(e => e.Contract);
                Employee? target = navPositionId switch
                {
                    0 => await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    1 => await query.Where(e => e.Id < idReference).OrderByDescending(e => e.Id).FirstOrDefaultAsync() ?? await query.OrderBy(e => e.Id).FirstOrDefaultAsync(),
                    2 => await query.Where(e => e.Id > idReference).OrderBy(e => e.Id).FirstOrDefaultAsync() ?? await query.FirstOrDefaultAsync(e => e.Id == idReference),
                    3 => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync(),
                    _ => await query.OrderByDescending(e => e.Id).FirstOrDefaultAsync()
                };
                if (target == null) return Result<Employee>.Failure("No hay registros disponibles.");
                return Result<Employee>.Success(target);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en NavBar");
                return Result<Employee>.Failure("Error en navegación.");
            }
        }

        public async Task<Result<List<Employee>>> GetList(int page, int pageSize, string? name)
        {
            try
            {
                IQueryable<Employee> query = _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Include(e => e.Position)
                    .Include(e => e.Contract);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(e =>
                        EF.Functions.ILike(e.Name, $"%{name}%") ||
                        EF.Functions.ILike(e.LastName, $"%{name}%") ||
                        EF.Functions.ILike(e.EmployeeCode, $"%{name}%"));
                }

                int skip = (page - 1) * pageSize;
                var list = await query.OrderBy(e => e.LastName).ThenBy(e => e.Name)
                                      .Skip(skip)
                                      .Take(pageSize)
                                      .ToListAsync();

                return Result<List<Employee>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar empleados.");
                return Result<List<Employee>>.Failure("No se pudo obtener la lista de empleados.");
            }
        }

        public async Task<Result<List<Employee>>> GetByDepartmentId(int departmentId)
        {
            try
            {
                var list = await _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Include(e => e.Position)
                    .Include(e => e.Contract)
                    .Where(e => e.DepartmentId == departmentId)
                    .OrderBy(e => e.LastName).ThenBy(e => e.Name)
                    .ToListAsync();

                return Result<List<Employee>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados por departamento ID: {Id}", departmentId);
                return Result<List<Employee>>.Failure("Error al consultar empleados del departamento.");
            }
        }

        public async Task<Result<Employee>> GetByCode(string code)
        {
            try
            {
                var employee = await _context.Employees
                    .AsNoTracking()
                    .Include(e => e.Department)
                    .Include(e => e.Position)
                    .Include(e => e.Contract)
                    .FirstOrDefaultAsync(e => e.EmployeeCode == code);

                if (employee == null)
                    return Result<Employee>.Failure("Empleado no encontrado.");

                return Result<Employee>.Success(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar empleado por código: {Code}", code);
                return Result<Employee>.Failure("Error al buscar el empleado.");
            }
        }

        public async Task<Result<bool>> Insert(Employee employee)
        {
            try
            {
                bool exists = await _context.Employees.AnyAsync(e => e.EmployeeCode == employee.EmployeeCode);
                if (exists)
                    return Result<bool>.Failure($"Ya existe un empleado con el código: {employee.EmployeeCode}");

                await _context.Employees.AddAsync(employee);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar empleado.");
                return Result<bool>.Failure($"Error al guardar el empleado: {ex.Message}");
            }
        }

        public async Task<Result<bool>> Update(Employee employee)
        {
            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar empleado ID: {Id}", employee.Id);
                return Result<bool>.Failure("Error al actualizar el registro.");
            }
        }

        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee == null)
                    return Result<bool>.Failure("El empleado no existe.");

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al borrar empleado ID: {Id}", id);
                return Result<bool>.Failure("No se pudo borrar el empleado.");
            }
        }
    }
}
