using Microsoft.EntityFrameworkCore;

namespace Pymex.Payroll.Data.Contexts
{
    public class PayrollDbContextFactory : IDbContextFactory<PayrollDbContext>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayrollDbContextFactory(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<PayrollDbContext> CreateDbContextWithNameAsync(string connectionStringName, CancellationToken cancellationToken = default)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PayrollDbContext>();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(connectionStringName), opts => opts.MigrationsHistoryTable("__EFMigrationsHistory", "enterprise"));

            return Task.FromResult(new PayrollDbContext(optionsBuilder.Options, _httpContextAccessor));
        }

        public PayrollDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PayrollDbContext>();
            optionsBuilder.UseNpgsql(connectionString, opts => opts.MigrationsHistoryTable("__EFMigrationsHistory", "enterprise"));

            return new PayrollDbContext(optionsBuilder.Options, _httpContextAccessor);
        }

        public PayrollDbContext CreateDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
