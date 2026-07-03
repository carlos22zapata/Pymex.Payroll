using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<Result<DepartmentDto>> GetById(int id);
        Task<Result<DepartmentDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<DepartmentDto>>> GetList(string? name);
        Task<Result<bool>> Insert(DepartmentDto departmentDto);
        Task<Result<bool>> Update(DepartmentDto departmentDto);
        Task<Result<bool>> Delete(int id);
    }
}
