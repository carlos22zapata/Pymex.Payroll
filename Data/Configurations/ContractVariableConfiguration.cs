using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class ContractVariableConfiguration : IEntityTypeConfiguration<ContractVariable>
    {
        public void Configure(EntityTypeBuilder<ContractVariable> builder)
        {
            builder.ToTable("ContractVariables", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_ContractVariables");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Value).HasPrecision(18, 2);
            builder.Property(e => e.StringValue).HasMaxLength(500);

            builder.HasOne(e => e.Contract)
                   .WithMany(e => e.ContractVariables)
                   .HasForeignKey(e => e.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PayrollVariable)
                   .WithMany()
                   .HasForeignKey(e => e.PayrollVariableId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ContractId, e.PayrollVariableId })
                   .HasDatabaseName("UIDX_ContractVariables")
                   .IsUnique();
        }
    }
}