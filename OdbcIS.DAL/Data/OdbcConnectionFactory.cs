using System.Data.Odbc;

namespace OdbcIS.DAL.Data;

public class OdbcConnectionFactory
{
    private readonly string _connectionString;

    public OdbcConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("ODBC connection string is not configured.", nameof(connectionString));

        _connectionString = connectionString;
    }

    public OdbcConnection CreateConnection() => new(_connectionString);
}
