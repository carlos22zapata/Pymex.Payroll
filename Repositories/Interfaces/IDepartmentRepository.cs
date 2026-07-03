using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<Result<Department>> GetById(int id);
        Task<Result<Department>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<Department>>> GetList(string? name);
        Task<Result<bool>> Insert(Department department);
        Task<Result<bool>> Update(Department department);
        Task<Result<bool>> Delete(int id);
    }
}
