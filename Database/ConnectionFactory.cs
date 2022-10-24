using Microsoft.Data.SqlClient;
namespace Database;
public static class ConnectionFactory
{
    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString.Get());
    }
}
