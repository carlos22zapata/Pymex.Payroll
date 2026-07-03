using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Pymex.Payroll.Services.Interfaces;

namespace Pymex.Payroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PayrollCalculationController : ControllerBase
    {
        private readonly IPayrollCalculationService _service;

        public PayrollCalculationController(IPayrollCalculationService service) => _service = service;

        [HttpGet("CalculateEmployeePayroll")]
        public async Task<IActionResult> CalculateEmployeePayroll([FromQuery] int employeeId)
        {
            var result = await _service.CalculateEmployeePayroll(employeeId);
            return Ok(result);
        }

        [HttpGet("CalculateAllPayroll")]
        public async Task<IActionResult> CalculateAllPayroll()
        {
            var result = await _service.CalculateAllPayroll();
            return Ok(result);
        }

        [HttpPost("ClosePayroll")]
        public async Task<IActionResult> ClosePayroll()
        {
            var result = await _service.ClosePayroll();
            return Ok(result);
        }
    }
}
