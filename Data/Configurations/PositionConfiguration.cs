using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_Positions");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.HasIndex(e => e.Name)
                   .HasDatabaseName("UIDX_Positions_Name")
                   .IsUnique();
        }
    }
}
