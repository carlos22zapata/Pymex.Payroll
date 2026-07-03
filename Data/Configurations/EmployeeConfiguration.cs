using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pymex.Payroll.Data.Entities;

namespace Pymex.Payroll.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", schema: "enterprise");

            builder.HasKey(e => e.Id).HasName("PK_Employees");

            builder.Property(e => e.Id).UseIdentityByDefaultColumn();
            builder.Property(e => e.EmployeeCode).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.LastName).IsRequired().HasMaxLength(200);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
            builder.Property(e => e.Phone).HasMaxLength(100);
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.City).HasMaxLength(100);
            builder.Property(e => e.State).HasMaxLength(100);
            builder.Property(e => e.ZipCode).HasMaxLength(20);
            builder.Property(e => e.HireDate).IsRequired();

            builder.HasIndex(e => e.EmployeeCode)
                   .HasDatabaseName("UIDX_Employees_Code")
                   .IsUnique();

            builder.HasIndex(e => e.Email)
                   .HasDatabaseName("IDX_Employees_Email");

            builder.HasIndex(e => new { e.Name, e.LastName })
                   .HasDatabaseName("IDX_Employees_FullName");

            builder.HasIndex(e => e.ContractId)
                   .HasDatabaseName("IDX_Employees_ContractId");

            builder.HasOne(e => e.Contract)
                   .WithMany()
                   .HasForeignKey(e => e.ContractId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Department)
                   .WithMany()
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Position)
                   .WithMany()
                   .HasForeignKey(e => e.PositionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
