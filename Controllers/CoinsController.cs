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
    public class CoinsController : ControllerBase
    {
        private readonly ICoinsService _service;

        public CoinsController(ICoinsService service) => _service = service;

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _service.GetById(id);
            return Ok(result);
        }

        [HttpGet("GetNavBarById")]
        public async Task<IActionResult> GetNavBarById([FromQuery] int navPositionId, [FromQuery] int idReference)
        {
            var result = await _service.GetNavBarById(navPositionId, idReference);
            return Ok(result);
        }

        [HttpGet("GetList")]
        public async Task<IActionResult> GetList([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string? name)
        {
            var result = await _service.GetList(page, pageSize, name);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] CoinsDto coin)
        {
            var result = await _service.Insert(coin);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CoinsDto coin)
        {
            var result = await _service.Update(coin);
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var result = await _service.Delete(id);
            return Ok(result);
        }
    }
}
