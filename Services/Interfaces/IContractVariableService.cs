using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IContractVariableService
    {
        Task<Result<ContractVariableDto>> GetById(int id);
        Task<Result<ContractVariableDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<ContractVariableDto>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(ContractVariableDto contractVariableDto);
        Task<Result<bool>> Update(ContractVariableDto contractVariableDto);
        Task<Result<bool>> Delete(int id);
    }
}