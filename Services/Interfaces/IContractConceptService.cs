using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IContractConceptService
    {
        Task<Result<ContractConceptDto>> GetById(int id);
        Task<Result<ContractConceptDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<ContractConceptDto>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(ContractConceptDto contractConceptDto);
        Task<Result<bool>> Update(ContractConceptDto contractConceptDto);
        Task<Result<bool>> Delete(int id);
    }
}
