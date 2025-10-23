using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class PedidoRepository
{
    // EXERCÍCIO 7: Criar pedido com itens (transação)
    public void CriarPedido(Pedido pedido, List<PedidoItem> itens)
    {
        // TODO: Implemente criação de pedido com transação
        // 1. Inserir Pedido
        // 2. Inserir cada PedidoItem
        // 3. Atualizar estoque dos produtos
        // IMPORTANTE: Use SqlTransaction!
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            // TODO: Inicie a transação
            SqlTransaction transaction = conn.BeginTransaction();
                
            try
            {
                // TODO: 1. Inserir pedido e obter ID
                string sqlPedido = "INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) " +
                                   "OUTPUT INSERTED.Id " +
                                   "VALUES (@ClienteId, @DataPedido, @ValorTotal)";
                    
                int pedidoId = 0;
                using (SqlCommand cmd = new SqlCommand(sqlPedido, conn, transaction))
                {
                    // TODO: Complete os parâmetros e execute
                }
                    
                // TODO: 2. Inserir itens do pedido
                    
                // TODO: 3. Atualizar estoque
                    
                // TODO: Commit da transação
                transaction.Commit();
                Console.WriteLine("Pedido criado com sucesso!");
            }
            catch (Exception ex)
            {
                // TODO: Rollback em caso de erro
                transaction.Rollback();
                Console.WriteLine($"Erro ao criar pedido: {ex.Message}");
                throw;
            }
        }
    }

    // EXERCÍCIO 8: Listar pedidos de um cliente
    public void ListarPedidosCliente(int clienteId)
    {
        // TODO: Liste todos os pedidos de um cliente
        // Mostre: Id, Data, ValorTotal
            
        string sql = "SELECT * FROM Pedidos WHERE ClienteId = @ClienteId ORDER BY DataPedido DESC";
            
        // TODO: Complete a implementação
    }

    // EXERCÍCIO 9: Obter detalhes completos de um pedido
    public void ObterDetalhesPedido(int pedidoId)
    {
        // TODO: Mostre o pedido com todos os itens
        // Faça JOIN com Produtos para mostrar nomes
            
        string sql = @"SELECT 
                            pi.*, 
                            p.Nome as NomeProduto,
                            (pi.Quantidade * pi.PrecoUnitario) as Subtotal
                          FROM PedidoItens pi
                          INNER JOIN Produtos p ON pi.ProdutoId = p.Id
                          WHERE pi.PedidoId = @PedidoId";
            
        // TODO: Complete a implementação
    }

    // DESAFIO 3: Calcular total de vendas por período
    public void TotalVendasPorPeriodo(DateTime dataInicio, DateTime dataFim)
    {
        // TODO: Calcule o total de vendas em um período
        // Use ExecuteScalar para obter a soma
    }
}