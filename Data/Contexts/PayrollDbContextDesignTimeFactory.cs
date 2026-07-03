using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pymex.Payroll.Data.Contexts;

public class PayrollDbContextDesignTimeFactory : IDesignTimeDbContextFactory<PayrollDbContext>
{
    public PayrollDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PayrollDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=PymexPayroll;Username=postgres;Password=*A123456", opts => opts.MigrationsHistoryTable("__EFMigrationsHistory", "enterprise"));

        return new PayrollDbContext(optionsBuilder.Options, null);
    }
}
