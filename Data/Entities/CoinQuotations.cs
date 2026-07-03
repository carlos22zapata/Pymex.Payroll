using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Pymex.Payroll.Data.Entities
{
    public class CoinQuotations
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Observation { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "decimal(18, 6)")]
        public decimal Value { get; set; }
        public int CoinId { get; set; }
        public int Origin { get; set; }

        [JsonIgnore]
        public Coins? Coin { get; set; }
    }
}
