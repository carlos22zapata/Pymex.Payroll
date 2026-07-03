namespace Pymex.Payroll.Services
{
    public class TenantConnectionProvider : ITenantConnectionProvider
    {
        private string _connectionString;
        private string _connectionName;

        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                throw new InvalidOperationException("Connection string has not been set for the current tenant.");
            return _connectionString;
        }

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionName()
        {
            if (string.IsNullOrEmpty(_connectionName))
                throw new InvalidOperationException("Connection name has not been set for the current tenant.");
            return _connectionName;
        }

        public void SetConnectionName(string connectionName)
        {
            _connectionName = connectionName;
        }
    }
}
