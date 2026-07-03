namespace Pymex.Payroll.Data.Dtos
{
    public class PayrollNoveltyDto
    {
        public int Id { get; set; }
        public string PeriodCode { get; set; } = string.Empty;
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public int PayrollVariableId { get; set; }
        public string? VariableCode { get; set; }
        public string? VariableName { get; set; }
        public decimal Value { get; set; }
    }
}