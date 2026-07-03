namespace Pymex.Payroll.Data.Entities
{
    public class PayrollNovelty : AuditableEntity
    {
        public int Id { get; set; }

        // Identificador de la quincena o semana (ej: "2026-06-Q1")
        public string PeriodCode { get; set; } = string.Empty;

        // Se ata al contrato, no al empleado
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        public int PayrollVariableId { get; set; }
        public PayrollVariable? PayrollVariable { get; set; }

        // Cantidad de la novedad (Ej: 3 para "Días de inasistencia")
        public decimal Value { get; set; }
    }
}
