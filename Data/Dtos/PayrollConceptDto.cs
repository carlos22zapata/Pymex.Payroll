namespace Pymex.Payroll.Data.Dtos
{
    public class PayrollConceptDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int ConceptType { get; set; }
        public string ConceptTypeName { get; set; } = string.Empty;
        public bool IsTaxable { get; set; }
        public string Formula { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
