using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class PedidoRepository
{
    // EXERCÍCIO 7: Criar pedido com itens (transação)
    public void CriarPedido(Pedido pedido, List<PedidoItem> itens)
    {
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            SqlTransaction? transaction = null;
                
            try
            {
                transaction = conn.BeginTransaction();
                
                // 1. Inserir pedido e obter ID
                string sqlPedido = "INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) " +
                                   "OUTPUT INSERTED.Id " +
                                   "VALUES (@ClienteId, @DataPedido, @ValorTotal)";
                    
                int pedidoId = 0;
                using (SqlCommand cmd = new SqlCommand(sqlPedido, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);
                    cmd.Parameters.AddWithValue("@DataPedido", pedido.DataPedido);
                    cmd.Parameters.AddWithValue("@ValorTotal", pedido.ValorTotal);
                    
                    pedidoId = (int)cmd.ExecuteScalar();
                }
                    
                // 2. Inserir itens do pedido
                string sqlItem = "INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario) " +
                                 "VALUES (@PedidoId, @ProdutoId, @Quantidade, @PrecoUnitario)";
                
                foreach (var item in itens)
                {
                    using (SqlCommand cmd = new SqlCommand(sqlItem, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                        cmd.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                        cmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmd.Parameters.AddWithValue("@PrecoUnitario", item.PrecoUnitario);
                        
                        cmd.ExecuteNonQuery();
                    }
                    
                    // 3. Atualizar estoque
                    string sqlEstoque = "UPDATE Produtos SET Estoque = Estoque - @Quantidade " +
                                        "WHERE Id = @ProdutoId";
                    
                    using (SqlCommand cmd = new SqlCommand(sqlEstoque, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Quantidade", item.Quantidade);
                        cmd.Parameters.AddWithValue("@ProdutoId", item.ProdutoId);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                    
                transaction.Commit();
                Console.WriteLine($"\n✅ Pedido #{pedidoId} criado com sucesso!");
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                Console.WriteLine($"\n❌ Erro ao criar pedido: {ex.Message}");
                throw;
            }
        }
    }

    // EXERCÍCIO 8: Listar pedidos de um cliente
    public void ListarPedidosCliente(int clienteId)
    {
        Console.WriteLine("\n=== PEDIDOS DO CLIENTE ===\n");
            
        string sql = "SELECT * FROM Pedidos WHERE ClienteId = @ClienteId ORDER BY DataPedido DESC";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                    
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("{0,-10} {1,-20} {2,-15}", 
                        "ID Pedido", "Data", "Valor Total");
                    Console.WriteLine(new string('-', 50));
                    
                    bool encontrou = false;
                    while (reader.Read())
                    {
                        encontrou = true;
                        Console.WriteLine("{0,-10} {1,-20:dd/MM/yyyy HH:mm} {2,-15:C}", 
                            reader["Id"],
                            reader["DataPedido"],
                            reader["ValorTotal"]);
                    }
                    
                    if (!encontrou)
                    {
                        Console.WriteLine("Nenhum pedido encontrado para este cliente.");
                    }
                }
            }
        }
    }

    // EXERCÍCIO 9: Obter detalhes completos de um pedido
    public void ObterDetalhesPedido(int pedidoId)
    {
        Console.WriteLine("\n=== DETALHES DO PEDIDO ===\n");
            
        // Primeiro, buscar informações do pedido
        string sqlPedido = "SELECT p.*, c.Nome as NomeCliente " +
                           "FROM Pedidos p " +
                           "INNER JOIN Clientes c ON p.ClienteId = c.Id " +
                           "WHERE p.Id = @PedidoId";
        
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
            
            using (SqlCommand cmd = new SqlCommand(sqlPedido, conn))
            {
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"Pedido ID: {reader["Id"]}");
                        Console.WriteLine($"Cliente: {reader["NomeCliente"]}");
                        Console.WriteLine($"Data: {Convert.ToDateTime(reader["DataPedido"]):dd/MM/yyyy HH:mm}");
                        Console.WriteLine($"Valor Total: {Convert.ToDecimal(reader["ValorTotal"]):C}");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"Pedido #{pedidoId} não encontrado.");
                        return;
                    }
                }
            }
            
            // Agora buscar os itens do pedido
            string sql = @"SELECT 
                            pi.*, 
                            p.Nome as NomeProduto,
                            (pi.Quantidade * pi.PrecoUnitario) as Subtotal
                          FROM PedidoItens pi
                          INNER JOIN Produtos p ON pi.ProdutoId = p.Id
                          WHERE pi.PedidoId = @PedidoId";
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("ITENS DO PEDIDO:");
                    Console.WriteLine("{0,-30} {1,-10} {2,-15} {3,-15}", 
                        "Produto", "Qtd", "Preço Unit.", "Subtotal");
                    Console.WriteLine(new string('-', 75));
                    
                    decimal total = 0;
                    while (reader.Read())
                    {
                        decimal subtotal = Convert.ToDecimal(reader["Subtotal"]);
                        total += subtotal;
                        
                        Console.WriteLine("{0,-30} {1,-10} {2,-15:C} {3,-15:C}", 
                            reader["NomeProduto"],
                            reader["Quantidade"],
                            reader["PrecoUnitario"],
                            subtotal);
                    }
                    
                    Console.WriteLine(new string('-', 75));
                    Console.WriteLine($"{"TOTAL:",-55} {total:C}");
                }
            }
        }
    }

    // DESAFIO 3: Calcular total de vendas por período
    public void TotalVendasPorPeriodo(DateTime dataInicio, DateTime dataFim)
    {
        // TODO: Calcule o total de vendas em um período
        // Use ExecuteScalar para obter a soma
    }
}