namespace Pymex.Payroll.Data.Dtos
{
    public class PayrollResultDto
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public List<ConceptResultDto> Concepts { get; set; } = new();
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetPay { get; set; }
    }

    public class ConceptResultDto
    {
        public int PayrollConceptId { get; set; }
        public string? ConceptCode { get; set; }
        public string? ConceptName { get; set; }
        public int ConceptType { get; set; }
        public decimal Amount { get; set; }
        public string? Formula { get; set; }
    }
}
