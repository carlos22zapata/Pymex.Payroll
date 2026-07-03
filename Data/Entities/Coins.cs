using System.ComponentModel.DataAnnotations;

namespace Pymex.Payroll.Data.Entities
{
    public class Coins
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = string.Empty;
        [Required]
        public bool Enabled { get; set; } = true;
        public string IdWeb { get; set; } = string.Empty;
    }
}
