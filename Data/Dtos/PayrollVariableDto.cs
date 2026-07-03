namespace Pymex.Payroll.Data.Dtos
{
    public class PayrollVariableDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int DataType { get; set; }
        public string DataTypeName { get; set; } = string.Empty;
        public int Behavior { get; set; }
        public string BehaviorName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CoinId { get; set; }
        public string? CoinName { get; set; }
    }
}
