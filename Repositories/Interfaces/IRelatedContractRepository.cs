using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IRelatedContractRepository
    {
        Task<Result<RelatedContract>> GetById(int id);
        Task<Result<RelatedContract>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<RelatedContract>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(RelatedContract relatedContract);
        Task<Result<bool>> Update(RelatedContract relatedContract);
        Task<Result<bool>> Delete(int id);
    }
}
