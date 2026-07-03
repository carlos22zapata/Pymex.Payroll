using Pymex.Payroll.Data.Enums;

namespace Pymex.Payroll.Data.Entities
{
    public class PayrollVariable : AuditableEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CoinId { get; set; }
        public VariableDataType DataType { get; set; }
        public PersistenceBehavior Behavior { get; set; }
        public bool IsActive { get; set; } = true;
        public Coins? Coin { get; set; }
    }
}
