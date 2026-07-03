namespace Pymex.Payroll.Data.Entities
{
    public class ContractConcept : AuditableEntity
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; } = null!;
        public int PayrollConceptId { get; set; }
        public PayrollConcept PayrollConcept { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
