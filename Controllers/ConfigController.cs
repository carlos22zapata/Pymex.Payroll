using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pymex.Shared.Time;

namespace Pymex.Payroll.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ConfigController : ControllerBase
    {
        /// <summary>Configuracion del sistema: zona horaria activa (TZ) y hora del servidor.</summary>
        [HttpGet]
        public IActionResult Get() => Ok(new
        {
            timeZone = AppClock.TimeZoneId,
            serverNow = AppClock.UtcNow,
            utcOffsetMinutes = AppClock.UtcOffsetMinutes
        });
    }
}
