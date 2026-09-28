using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IRelatedContractService
    {
        Task<Result<RelatedContractDto>> GetById(int id);
        Task<Result<RelatedContractDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<RelatedContractDto>>> GetByContractId(int contractId);
        Task<Result<bool>> Insert(RelatedContractDto relatedContractDto);
        Task<Result<bool>> Update(RelatedContractDto relatedContractDto);
        Task<Result<bool>> Delete(int id);
    }
}
