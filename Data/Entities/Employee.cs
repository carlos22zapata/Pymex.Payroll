namespace Pymex.Payroll.Data.Entities
{
    public class Employee : AuditableEntity
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; } = null!;
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public int PositionId { get; set; }
        public Position Position { get; set; } = null!;
        public bool IsActive => !TerminationDate.HasValue;
    }
}
