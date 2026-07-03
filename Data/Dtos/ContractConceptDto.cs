namespace Pymex.Payroll.Data.Dtos
{
    public class ContractConceptDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public int PayrollConceptId { get; set; }
        public string? PayrollConceptName { get; set; }
        public bool IsActive { get; set; }
    }
}
