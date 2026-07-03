namespace Pymex.Payroll.Data.Entities
{
    public class ContractVariable : AuditableEntity
    {
        public int Id { get; set; }

        // Llave foránea hacia el Contrato
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        // Llave foránea hacia la Variable maestra
        public int PayrollVariableId { get; set; }
        public PayrollVariable? PayrollVariable { get; set; }

        // El valor acordado para este contrato (ej: 150.00 para un bono)
        public decimal Value { get; set; }

        // Opcional: Si el valor de la variable es un string o fórmula
        public string? StringValue { get; set; }
    }
}
