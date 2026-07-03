using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface IPositionRepository
    {
        Task<Result<Position>> GetById(int id);
        Task<Result<Position>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<Position>>> GetList(string? name);
        Task<Result<bool>> Insert(Position position);
        Task<Result<bool>> Update(Position position);
        Task<Result<bool>> Delete(int id);
    }
}
