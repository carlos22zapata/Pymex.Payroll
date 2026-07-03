using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface IPositionService
    {
        Task<Result<PositionDto>> GetById(int id);
        Task<Result<PositionDto>> GetNavBarById(int navPositionId, int idReference);
        Task<Result<List<PositionDto>>> GetList(string? name);
        Task<Result<bool>> Insert(PositionDto positionDto);
        Task<Result<bool>> Update(PositionDto positionDto);
        Task<Result<bool>> Delete(int id);
    }
}
