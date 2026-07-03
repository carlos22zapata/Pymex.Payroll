using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class CoinQuotationConfiguration : IEntityTypeConfiguration<CoinQuotations>
    {
        public void Configure(EntityTypeBuilder<CoinQuotations> builder)
        {
            builder.ToTable("CoinQuotations", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_CoinQuotations");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.Observation).HasMaxLength(500);
            builder.Property(e => e.Value).IsRequired().HasColumnType("decimal(18,6)");
            builder.Property(e => e.Origin).IsRequired();

            builder.HasOne(e => e.Coin)
                   .WithMany()
                   .HasForeignKey(e => e.CoinId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_CoinQuotations_Coins");

            builder.HasIndex(e => new { e.CoinId, e.Date })
                   .HasDatabaseName("IDX_CoinQuotations_CoinId_Date");
        }
    }
}
