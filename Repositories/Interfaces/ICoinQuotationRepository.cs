using Pymex.Payroll.Data.Entities;
using Pymex.Payroll.Data.Results;

namespace Pymex.Payroll.Repositories.Interfaces
{
    public interface ICoinQuotationRepository
    {
        Task<Result<CoinQuotations>> GetById(int id);
        Task<Result<List<CoinQuotations>>> GetList(int page, int pageSize, int coinId);
        Task<Result<bool>> Insert(CoinQuotations quotation);
        Task<Result<bool>> Update(CoinQuotations quotation);
        Task<Result<bool>> Delete(int id);
        Task<Result<bool>> InsertList(List<CoinQuotations> quotations);
        Task<Result<bool>> UpdateList(List<CoinQuotations> quotations);
        Task<Result<bool>> UpdateBCVCoinQuotationListScraper(string webSite);
        Task<Result<bool>> ImportBCVExcelHistoricalRates();
    }
}
