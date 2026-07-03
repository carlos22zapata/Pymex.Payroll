namespace Pymex.Payroll.Data.Enums
{
    public enum CalculationMethod
    {
        FixedAmount = 1,   // Monto fijo ($50)
        Percentage = 2,    // Porcentaje (4%)
        Formula = 3        // Ecuación compleja ("BaseSalary / 30 * Days")
    }
}
