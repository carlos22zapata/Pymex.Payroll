using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class PayrollConceptConfiguration : IEntityTypeConfiguration<PayrollConcept>
    {
        public void Configure(EntityTypeBuilder<PayrollConcept> builder)
        {
            builder.ToTable("PayrollConcepts", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_PayrollConcepts");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Code).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.Property(e => e.ConceptType)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(e => e.Formula).IsRequired().HasMaxLength(1000);

            builder.HasIndex(e => e.Code)
                   .HasDatabaseName("UIDX_PayrollConcepts_Code")
                   .IsUnique();
        }
    }
}
