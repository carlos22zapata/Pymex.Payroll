using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class PayrollVariableConfiguration : IEntityTypeConfiguration<PayrollVariable>
    {
        public void Configure(EntityTypeBuilder<PayrollVariable> builder)
        {
            builder.ToTable("PayrollVariables", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_PayrollVariables");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Code).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.Property(e => e.DataType)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(e => e.Behavior)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(e => e.CoinId).IsRequired();

            builder.HasOne(e => e.Coin)
                   .WithMany()
                   .HasForeignKey(e => e.CoinId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => e.Code)
                   .HasDatabaseName("UIDX_PayrollVariables_Code")
                   .IsUnique();
        }
    }
}
