using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IPayrollVariableRepository
    {
        Task<Result<PayrollVariable>> GetById(int id);
        Task<Result<PayrollVariable>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PayrollVariable>>> GetList(string? name);
        Task<Result<bool>> Insert(PayrollVariable payrollVariable);
        Task<Result<bool>> Update(PayrollVariable payrollVariable);
        Task<Result<bool>> Delete(int id);
    }
}
