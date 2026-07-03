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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeesController(IEmployeeService service)
        {
            _service = service;
        }

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
        public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? name = null)
        {
            var result = await _service.GetList(page, pageSize, name);
            return Ok(result);
        }

        [HttpGet("GetByDepartmentId")]
        public async Task<IActionResult> GetByDepartmentId([FromQuery] int departmentId)
        {
            var result = await _service.GetByDepartmentId(departmentId);
            return Ok(result);
        }

        [HttpGet("GetByCode")]
        public async Task<IActionResult> GetByCode([FromQuery] string code)
        {
            var result = await _service.GetByCode(code);
            return Ok(result);
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> Insert([FromBody] EmployeeDto employee)
        {
            var result = await _service.Insert(employee);
            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] EmployeeDto employee)
        {
            var result = await _service.Update(employee);
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
