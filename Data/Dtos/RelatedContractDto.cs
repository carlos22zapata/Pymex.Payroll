namespace Pymex.Payroll.Data.Dtos
{
    public class RelatedContractDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public int RelatedContractId { get; set; }
        public string? RelatedContractName { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
