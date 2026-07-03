using System.ComponentModel.DataAnnotations;

namespace Pymex.Payroll.Data.Entities
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ThousandsSeparator { get; set; }
        [Required]
        public string DecimalSeparator { get; set; }
        [Required]
        public int Decimals { get; set; }
    }
}
