using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<Result<EmployeeDto>> GetById(int id);
        Task<Result<EmployeeDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<EmployeeDto>>> GetList(int page, int pageSize, string? name);
        Task<Result<List<EmployeeDto>>> GetByDepartmentId(int departmentId);
        Task<Result<EmployeeDto>> GetByCode(string code);
        Task<Result<bool>> Insert(EmployeeDto employeeDto);
        Task<Result<bool>> Update(EmployeeDto employeeDto);
        Task<Result<bool>> Delete(int id);
    }
}
