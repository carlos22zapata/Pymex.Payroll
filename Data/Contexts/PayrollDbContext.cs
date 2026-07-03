using Microsoft.EntityFrameworkCore;
using Pymex.Payroll.Data.Configurations;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Contexts
{
    public class PayrollDbContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public PayrollDbContext(DbContextOptions<PayrollDbContext> options,
            IHttpContextAccessor? httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractConcept> ContractConcepts { get; set; }
        public DbSet<PayrollConcept> PayrollConcepts { get; set; }
        public DbSet<PayrollVariable> PayrollVariables { get; set; }
        public DbSet<ContractVariable> ContractVariables { get; set; }
        public DbSet<PayrollNovelty> PayrollNovelties { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Coins> Coins { get; set; }
        public DbSet<CoinQuotations> CoinQuotations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));

#if DEBUG
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
#endif

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("enterprise");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DepartmentConfiguration).Assembly);
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditInfo()
        {
            string currentUser = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "System";
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUser;
                        entry.Entity.CreatedAt = now;
                        entry.Entity.UpdatedBy = currentUser;
                        entry.Entity.UpdatedAt = now;
                        break;

                    case EntityState.Modified:
                        entry.Property(x => x.CreatedBy).IsModified = false;
                        entry.Property(x => x.CreatedAt).IsModified = false;
                        entry.Entity.UpdatedBy = currentUser;
                        entry.Entity.UpdatedAt = now;
                        break;
                }
            }
        }
    }
}
