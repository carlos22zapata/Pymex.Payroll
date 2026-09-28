namespace Pymex.Payroll.Data.Entities
{
    public class RelatedContract : AuditableEntity
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public Contract Contract { get; set; } = null!;
        public int RelatedContractId { get; set; }
        public Contract RelatedContractRef { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
