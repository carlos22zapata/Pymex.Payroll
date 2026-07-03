using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class CoinsConfiguration : IEntityTypeConfiguration<Coins>
    {
        public void Configure(EntityTypeBuilder<Coins> builder)
        {
            builder.ToTable("Coins", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_Coins");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(300);
            builder.Property(e => e.Symbol).IsRequired().HasMaxLength(10);
            builder.Property(e => e.IdWeb).HasMaxLength(100);

            builder.HasIndex(e => e.Name)
                   .HasDatabaseName("UIDX_Coins_Name")
                   .IsUnique();

            builder.HasIndex(e => e.Symbol)
                   .HasDatabaseName("UIDX_Coins_Symbol")
                   .IsUnique();
        }
    }
}
