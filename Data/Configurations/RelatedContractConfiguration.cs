using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class RelatedContractConfiguration : IEntityTypeConfiguration<RelatedContract>
    {
        public void Configure(EntityTypeBuilder<RelatedContract> builder)
        {
            builder.ToTable("RelatedContracts", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_RelatedContracts");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();

            builder.Property(e => e.ContractId).HasColumnName("ContractId");

            builder.Property(e => e.RelatedContractId).HasColumnName("RelatedContractId");

            builder.HasOne(e => e.Contract)
                   .WithMany(c => c.RelatedContracts)
                   .HasForeignKey(e => e.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.RelatedContractRef)
                   .WithMany()
                   .HasForeignKey(e => e.RelatedContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(e => new { e.ContractId, e.RelatedContractId })
                   .HasDatabaseName("UIDX_RelatedContracts")
                   .IsUnique();
        }
    }
}
