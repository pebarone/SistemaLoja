﻿using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class DatabaseConnection
{
    // EXERCÍCIO 1: Connection String configurada
    private static string connectionString = 
        "Server=localhost,1433;" +
        "Database=LojaDB;" +
        "User Id=sa;" +
        "Password=SqlServer2024!;" +
        "TrustServerCertificate=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}