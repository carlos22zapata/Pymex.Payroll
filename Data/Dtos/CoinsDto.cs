namespace Pymex.Payroll.Data.Dtos
{
    public class CoinsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public string IdWeb { get; set; } = string.Empty;
    }
}
