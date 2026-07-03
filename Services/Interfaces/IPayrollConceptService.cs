using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IPayrollConceptService
    {
        Task<Result<PayrollConceptDto>> GetById(int id);
        Task<Result<PayrollConceptDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PayrollConceptDto>>> GetList(string? name);
        Task<Result<bool>> Insert(PayrollConceptDto payrollConceptDto);
        Task<Result<bool>> Update(PayrollConceptDto payrollConceptDto);
        Task<Result<bool>> Delete(int id);
    }
}
