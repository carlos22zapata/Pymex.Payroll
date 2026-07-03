using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class PayrollNoveltyConfiguration : IEntityTypeConfiguration<PayrollNovelty>
    {
        public void Configure(EntityTypeBuilder<PayrollNovelty> builder)
        {
            builder.ToTable("PayrollNovelties", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_PayrollNovelties");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.PeriodCode).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Value).HasPrecision(18, 2);

            builder.HasOne(e => e.Contract)
                   .WithMany()
                   .HasForeignKey(e => e.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PayrollVariable)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollVariableId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ContractId, e.PayrollVariableId, e.PeriodCode })
                   .HasDatabaseName("UIDX_PayrollNovelties")
                   .IsUnique();
        }
    }
}