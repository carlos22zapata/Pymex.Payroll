using Pymex.Payroll.Data.Enums;

namespace Pymex.Payroll.Data.Entities
{
    public class PayrollConcept : AuditableEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public ConceptType ConceptType { get; set; }
        public bool IsTaxable { get; set; }
        public string Formula { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<ContractConcept> ContractConcepts { get; set; } = new List<ContractConcept>();
    }
}
