using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IPayrollVariableService
    {
        Task<Result<PayrollVariableDto>> GetById(int id);
        Task<Result<PayrollVariableDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PayrollVariableDto>>> GetList(string? name);
        Task<Result<bool>> Insert(PayrollVariableDto payrollVariableDto);
        Task<Result<bool>> Update(PayrollVariableDto payrollVariableDto);
        Task<Result<bool>> Delete(int id);
    }
}
