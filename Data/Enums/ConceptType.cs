namespace Pymex.Payroll.Data.Enums
{
    public enum ConceptType
    {
        Earning = 1,       // Asignaciones (Ingresos que suman al neto del trabajador)
        Deduction = 2,     // Deducciones (Descuentos internos, como préstamos o adelantos)
        Withholding = 3,   // Retenciones (Descuentos de ley, como impuestos o seguro social)
        Other = 4          // Otros (Provisiones, aportes patronales que no afectan el pago neto)
    }
}
