using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface ICoinsService
    {
        Task<Result<CoinsDto>> GetById(int id);
        Task<Result<CoinsDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<CoinsDto>>> GetList(int page, int pageSize, string? name);
        Task<Result<bool>> Insert(CoinsDto coinDto);
        Task<Result<bool>> Update(CoinsDto coinDto);
        Task<Result<bool>> Delete(int id);
    }
}
