using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Pymex.Payroll.Data.Dtos;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContractConceptsController : ControllerBase
    {
        private readonly IContractConceptService _service;

        public ContractConceptsController(IContractConceptService service) => _service = service;

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

        [HttpGet("GetByContractId")]
        public async Task<IActionResult> GetByContractId([FromQuery] int contractId)
        {
            var result = await _service.GetByContractId(contractId);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] ContractConceptDto contractConcept)
        {
            var result = await _service.Insert(contractConcept);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ContractConceptDto contractConcept)
        {
            var result = await _service.Update(contractConcept);
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
