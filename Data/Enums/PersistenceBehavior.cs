namespace Pymex.Payroll.Data.Enums
{
    public enum PersistenceBehavior
    {
        Fixed = 1,        // Fijo: Mantiene su valor nómina tras nómina (Ej: % ISLR)
        Volatile = 2,     // No Fijo: Se resetea a 0 o vacío al hacer el "Cierre de Nómina" (Ej: Horas Extras)
        Calculated = 3    // No Editable: El usuario no lo toca, lo calcula el sistema (Ej: Antigüedad en días)
    }
}
