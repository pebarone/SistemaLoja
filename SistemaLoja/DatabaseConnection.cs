using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class DatabaseConnection
{
    // Connection String - suporta ambiente local e Docker
    private static string connectionString = 
        "Server=" + (Environment.GetEnvironmentVariable("SQL_SERVER") ?? "localhost") + ",1433;" +
        "Database=LojaDB;" +
        "User Id=sa;" +
        "Password=SqlServer2024!;" +
        "TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}