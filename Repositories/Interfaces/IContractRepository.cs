using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IContractRepository
    {
        Task<Result<Contract>> GetById(int id);
        Task<Result<Contract>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<Contract>>> GetList(string? name);
        Task<Result<bool>> Insert(Contract contract);
        Task<Result<bool>> Update(Contract contract);
        Task<Result<bool>> Delete(int id);
    }
}
