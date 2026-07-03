using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Services.Interfaces
{
    public interface ICoinQuotationService
    {
        Task<Result<CoinQuotationDto>> GetById(int id);
        Task<Result<List<CoinQuotationDto>>> GetList(int page, int pageSize, int coinId);
        Task<Result<bool>> Insert(CoinQuotationDto quotationDto);
        Task<Result<bool>> Update(CoinQuotationDto quotationDto);
        Task<Result<bool>> Delete(int id);
        Task<Result<bool>> InsertList(List<CoinQuotationDto> quotationsDto);
        Task<Result<bool>> UpdateList(List<CoinQuotationDto> quotationsDto);
        Task<Result<bool>> UpdateBCVCoinQuotationListScraper(string webSite);
        Task<Result<bool>> ImportBCVExcelHistoricalRates();
    }
}
