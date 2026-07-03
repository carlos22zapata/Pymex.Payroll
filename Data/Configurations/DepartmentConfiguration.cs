using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_Departments");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);

            builder.HasIndex(e => e.Name)
                   .HasDatabaseName("UIDX_Departments_Name")
                   .IsUnique();

            builder.HasOne(e => e.ParentDepartment)
                   .WithMany(e => e.ChildDepartments)
                   .HasForeignKey(e => e.ParentDepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
