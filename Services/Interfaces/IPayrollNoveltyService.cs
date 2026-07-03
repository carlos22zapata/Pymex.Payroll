using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IPayrollNoveltyService
    {
        Task<Result<PayrollNoveltyDto>> GetById(int id);
        Task<Result<PayrollNoveltyDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PayrollNoveltyDto>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(PayrollNoveltyDto payrollNoveltyDto);
        Task<Result<bool>> Update(PayrollNoveltyDto payrollNoveltyDto);
        Task<Result<bool>> Delete(int id);
    }
}