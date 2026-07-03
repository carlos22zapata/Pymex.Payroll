using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface ICoinsRepository
    {
        Task<Result<Coins>> GetById(int id);
        Task<Result<Coins>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<Coins>>> GetList(int page, int pageSize, string? name);
        Task<Result<bool>> Insert(Coins coin);
        Task<Result<bool>> Update(Coins coin);
        Task<Result<bool>> Delete(int id);
    }
}
