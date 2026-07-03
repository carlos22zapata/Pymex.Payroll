using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IPayrollCalculationRepository
    {
        Task<Result<List<PayrollResultDto>>> CalculateEmployeePayroll(int employeeId);
        Task<Result<List<PayrollResultDto>>> CalculateAllPayroll();
        Task<Result<bool>> ClosePayroll();
    }
}
