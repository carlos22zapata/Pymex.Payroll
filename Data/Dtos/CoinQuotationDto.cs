namespace Pymex.Payroll.Data.Dtos
{
    public class CoinQuotationDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observation { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public int CoinId { get; set; }
        public int Origin { get; set; }
        public string? CoinName { get; set; }
        public string? CoinSymbol { get; set; }
    }
}
