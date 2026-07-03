using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CoinQuotationsController : ControllerBase
    {
        private readonly ICoinQuotationService _service;

        public CoinQuotationsController(ICoinQuotationService service) => _service = service;

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] int coinId)
        {
            var result = await _service.GetList(page, pageSize, coinId);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] CoinQuotationDto quotation)
        {
            var result = await _service.Insert(quotation);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CoinQuotationDto quotation)
        {
            var result = await _service.Update(quotation);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }

        [HttpPost("InsertList")]
        public async Task<IActionResult> InsertList([FromBody] List<CoinQuotationDto> quotations)
        {
            var result = await _service.InsertList(quotations);
            return Ok(result);
        }

        [HttpPost("UpdateList")]
        public async Task<IActionResult> UpdateList([FromBody] List<CoinQuotationDto> quotations)
        {
            var result = await _service.UpdateList(quotations);
            return Ok(result);
        }

        [HttpPost("UpdateBCVCoinQuotationListScraper")]
        public async Task<IActionResult> UpdateBCVCoinQuotationListScraper()
        {
            var result = await _service.UpdateBCVCoinQuotationListScraper("https://www.bcv.org.ve");
            return Ok(result);
        }

        [HttpPost("ImportBCVExcelHistoricalRates")]
        public async Task<IActionResult> ImportBCVExcelHistoricalRates()
        {
            var result = await _service.ImportBCVExcelHistoricalRates();
            return Ok(result);
        }
    }
}
