namespace Pymex.Payroll.Data.Dtos
{
    public class EmployeeDto
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
        public string? ContractName { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
        public bool IsActive { get; set; }
    }
}
