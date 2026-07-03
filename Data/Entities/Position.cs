namespace Pymex.Payroll.Data.Entities
{
    public class Position : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
