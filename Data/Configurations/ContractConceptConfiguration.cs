using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class ContractConceptConfiguration : IEntityTypeConfiguration<ContractConcept>
    {
        public void Configure(EntityTypeBuilder<ContractConcept> builder)
        {
            builder.ToTable("ContractConcepts", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_ContractConcepts");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();

            builder.HasOne(e => e.Contract)
                   .WithMany(e => e.ContractConcepts)
                   .HasForeignKey(e => e.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.PayrollConcept)
                   .WithMany(e => e.ContractConcepts)
                   .HasForeignKey(e => e.PayrollConceptId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ContractId, e.PayrollConceptId })
                   .HasDatabaseName("UIDX_ContractConcepts")
                   .IsUnique();
        }
    }
}
