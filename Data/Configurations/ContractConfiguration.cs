using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class ContractConfiguration : IEntityTypeConfiguration<Contract>
    {
        public void Configure(EntityTypeBuilder<Contract> builder)
        {
            builder.ToTable("Contracts", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_Contracts");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Description).HasMaxLength(500);

            builder.HasIndex(e => e.Name)
                   .HasDatabaseName("UIDX_Contracts_Name")
                   .IsUnique();
        }
    }
}
