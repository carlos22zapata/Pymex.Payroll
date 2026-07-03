using Pymex.Payroll.Services;

namespace Pymex.Payroll.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantConnectionProvider tenantProvider)
        {
            var conex = context.Request.Headers["ConexName"].FirstOrDefault();
            if (!string.IsNullOrEmpty(conex))
            {
                tenantProvider.SetConnectionName(conex);
            }

            await _next(context);
        }
    }
}
