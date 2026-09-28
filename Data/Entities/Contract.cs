namespace Pymex.Payroll.Data.Entities
{
    public class Contract : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        // Colecciones de navegación
        public ICollection<ContractConcept> ContractConcepts { get; set; } = new List<ContractConcept>();
        public ICollection<ContractVariable> ContractVariables { get; set; } = new List<ContractVariable>();
        public ICollection<RelatedContract> RelatedContracts { get; set; } = new List<RelatedContract>();
    }
}
