namespace Pymex.Payroll.Services
{
    public interface ITenantConnectionProvider
    {
        void SetConnectionString(string connectionString);
        string GetConnectionString();
        string GetConnectionName();
        void SetConnectionName(string connectionName);
    }
}
