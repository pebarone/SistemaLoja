using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class DatabaseConnection
{
    // TODO: Complete a connection string com os dados corretos
    private static string connectionString = 
        "Server=__________,1433;" +
        "Database=__________;" +
        "User Id=__________;" +
        "Password=__________;" +
        "TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}