using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IPayrollNoveltyRepository
    {
        Task<Result<PayrollNovelty>> GetById(int id);
        Task<Result<PayrollNovelty>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PayrollNovelty>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(PayrollNovelty payrollNovelty);
        Task<Result<bool>> Update(PayrollNovelty payrollNovelty);
        Task<Result<bool>> Delete(int id);
    }
}