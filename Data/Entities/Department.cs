namespace Pymex.Payroll.Data.Entities
{
    public class Department : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentDepartmentId { get; set; }
        public Department? ParentDepartment { get; set; }
        public ICollection<Department>? ChildDepartments { get; set; }
    }
}
