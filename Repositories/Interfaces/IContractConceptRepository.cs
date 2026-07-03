using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IContractConceptRepository
    {
        Task<Result<ContractConcept>> GetById(int id);
        Task<Result<ContractConcept>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<ContractConcept>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(ContractConcept contractConcept);
        Task<Result<bool>> Update(ContractConcept contractConcept);
        Task<Result<bool>> Delete(int id);
    }
}
