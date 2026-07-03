using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<Result<Employee>> GetById(int id);
        Task<Result<Employee>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<Employee>>> GetList(int page, int pageSize, string? name);
        Task<Result<List<Employee>>> GetByDepartmentId(int departmentId);
        Task<Result<Employee>> GetByCode(string code);
        Task<Result<bool>> Insert(Employee employee);
        Task<Result<bool>> Update(Employee employee);
        Task<Result<bool>> Delete(int id);
    }
}
