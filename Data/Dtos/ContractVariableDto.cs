namespace Pymex.Payroll.Data.Dtos
{
    public class ContractVariableDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public string? ContractName { get; set; }
        public int PayrollVariableId { get; set; }
        public string? VariableCode { get; set; }
        public string? VariableName { get; set; }
        public int DataType { get; set; }
        public int Behavior { get; set; }
        public decimal Value { get; set; }
        public string? StringValue { get; set; }
        public int CoinId { get; set; }
        public string? CoinName { get; set; }
    }
}