using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IContractService
    {
        Task<Result<ContractDto>> GetById(int id);
        Task<Result<ContractDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<ContractDto>>> GetList(string? name);
        Task<Result<bool>> Insert(ContractDto contractDto);
        Task<Result<bool>> Update(ContractDto contractDto);
        Task<Result<bool>> Delete(int id);
    }
}
