using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IContractVariableRepository
    {
        Task<Result<ContractVariable>> GetById(int id);
        Task<Result<ContractVariable>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<ContractVariable>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(ContractVariable contractVariable);
        Task<Result<bool>> Update(ContractVariable contractVariable);
        Task<Result<bool>> Delete(int id);
    }
}