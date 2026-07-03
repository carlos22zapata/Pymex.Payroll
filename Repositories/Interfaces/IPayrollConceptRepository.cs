using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IPayrollConceptRepository
    {
        Task<Result<PayrollConcept>> GetById(int id);
        Task<Result<PayrollConcept>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PayrollConcept>>> GetList(string? name);
        Task<Result<bool>> Insert(PayrollConcept payrollConcept);
        Task<Result<bool>> Update(PayrollConcept payrollConcept);
        Task<Result<bool>> Delete(int id);
    }
}
