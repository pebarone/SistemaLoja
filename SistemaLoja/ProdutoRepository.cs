using Microsoft.Data.SqlClient;

namespace SistemaLoja.Lab12_ConexaoSQLServer;

public class ProdutoRepository
{
    // EXERCÍCIO 1: Listar todos os produtos
    public void ListarTodosProdutos()
    {
        Console.WriteLine("\n=== LISTA DE PRODUTOS ===\n");
            
        string sql = "SELECT * FROM Produtos";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("{0,-5} {1,-30} {2,-15} {3,-10} {4,-12}", 
                        "ID", "Nome", "Preço", "Estoque", "CategoriaID");
                    Console.WriteLine(new string('-', 75));
                    
                    while (reader.Read())
                    {
                        Console.WriteLine("{0,-5} {1,-30} {2,-15:C} {3,-10} {4,-12}", 
                            reader["Id"],
                            reader["Nome"],
                            reader["Preco"],
                            reader["Estoque"],
                            reader["CategoriaId"]);
                    }
                }
            }
        }
    }

    // EXERCÍCIO 2: Inserir novo produto
    public void InserirProduto(Produto produto)
    {
        string sql = "INSERT INTO Produtos (Nome, Preco, Estoque, CategoriaId) " +
                     "VALUES (@Nome, @Preco, @Estoque, @CategoriaId)";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);
                    
                int linhasAfetadas = cmd.ExecuteNonQuery();
                    
                Console.WriteLine($"\n✅ Produto inserido com sucesso! ({linhasAfetadas} registro(s) afetado(s))");
            }
        }
    }

    // EXERCÍCIO 3: Atualizar produto
    public void AtualizarProduto(Produto produto)
    {
        string sql = "UPDATE Produtos SET " +
                     "Nome = @Nome, " +
                     "Preco = @Preco, " +
                     "Estoque = @Estoque, " +
                     "CategoriaId = @CategoriaId " +
                     "WHERE Id = @Id";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", produto.Id);
                cmd.Parameters.AddWithValue("@Nome", produto.Nome);
                cmd.Parameters.AddWithValue("@Preco", produto.Preco);
                cmd.Parameters.AddWithValue("@Estoque", produto.Estoque);
                cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);
                    
                int linhasAfetadas = cmd.ExecuteNonQuery();
                    
                if (linhasAfetadas > 0)
                {
                    Console.WriteLine($"\n✅ Produto atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"\n⚠️ Nenhum produto encontrado com ID {produto.Id}");
                }
            }
        }
    }

    // EXERCÍCIO 4: Deletar produto
    public void DeletarProduto(int id)
    {
        string sql = "DELETE FROM Produtos WHERE Id = @Id";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                    
                int linhasAfetadas = cmd.ExecuteNonQuery();
                    
                if (linhasAfetadas > 0)
                {
                    Console.WriteLine($"\n✅ Produto deletado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"\n⚠️ Nenhum produto encontrado com ID {id}");
                }
            }
        }
    }

    // EXERCÍCIO 5: Buscar produto por ID
    public Produto? BuscarPorId(int id)
    {
        string sql = "SELECT * FROM Produtos WHERE Id = @Id";
        Produto? produto = null;
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                    
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        produto = new Produto
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString() ?? "",
                            Preco = Convert.ToDecimal(reader["Preco"]),
                            Estoque = Convert.ToInt32(reader["Estoque"]),
                            CategoriaId = Convert.ToInt32(reader["CategoriaId"])
                        };
                    }
                }
            }
        }
            
        return produto;
    }

    // EXERCÍCIO 6: Listar produtos por categoria
    public void ListarProdutosPorCategoria(int categoriaId)
    {
        Console.WriteLine("\n=== PRODUTOS POR CATEGORIA ===\n");
            
        string sql = @"SELECT p.*, c.Nome as NomeCategoria 
                          FROM Produtos p
                          INNER JOIN Categorias c ON p.CategoriaId = c.Id
                          WHERE p.CategoriaId = @CategoriaId";
            
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
                
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CategoriaId", categoriaId);
                    
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    bool encontrou = false;
                    string? nomeCategoria = null;
                    
                    Console.WriteLine("{0,-5} {1,-30} {2,-15} {3,-10}", 
                        "ID", "Nome", "Preço", "Estoque");
                    Console.WriteLine(new string('-', 65));
                    
                    while (reader.Read())
                    {
                        encontrou = true;
                        if (nomeCategoria == null)
                        {
                            nomeCategoria = reader["NomeCategoria"].ToString();
                        }
                        
                        Console.WriteLine("{0,-5} {1,-30} {2,-15:C} {3,-10}", 
                            reader["Id"],
                            reader["Nome"],
                            reader["Preco"],
                            reader["Estoque"]);
                    }
                    
                    if (!encontrou)
                    {
                        Console.WriteLine("Nenhum produto encontrado para esta categoria.");
                    }
                    else
                    {
                        Console.WriteLine($"\nCategoria: {nomeCategoria}");
                    }
                }
            }
        }
    }

    // DESAFIO 1: Buscar produtos com estoque baixo
    public void ListarProdutosEstoqueBaixo(int quantidadeMinima)
    {
        Console.WriteLine($"\n=== PRODUTOS COM ESTOQUE BAIXO (Menor que {quantidadeMinima}) ===\n");
        
        string sql = "SELECT * FROM Produtos WHERE Estoque < @QuantidadeMinima ORDER BY Estoque ASC";
        
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@QuantidadeMinima", quantidadeMinima);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("{0,-5} {1,-30} {2,-15} {3,-10}", 
                        "ID", "Nome", "Preço", "Estoque");
                    Console.WriteLine(new string('-', 65));
                    
                    bool encontrou = false;
                    while (reader.Read())
                    {
                        encontrou = true;
                        int estoque = Convert.ToInt32(reader["Estoque"]);
                        string aviso = estoque == 0 ? " ⚠️ SEM ESTOQUE!" : "";
                        
                        Console.ForegroundColor = estoque == 0 ? ConsoleColor.Red : ConsoleColor.Yellow;
                        Console.WriteLine("{0,-5} {1,-30} {2,-15:C} {3,-10}{4}", 
                            reader["Id"],
                            reader["Nome"],
                            reader["Preco"],
                            estoque,
                            aviso);
                        Console.ResetColor();
                    }
                    
                    if (!encontrou)
                    {
                        Console.WriteLine("✅ Nenhum produto com estoque baixo. Parabéns!");
                    }
                }
            }
        }
    }

    // DESAFIO 2: Buscar produtos por nome (LIKE)
    public void BuscarProdutosPorNome(string termoBusca)
    {
        Console.WriteLine($"\n=== BUSCA POR NOME: '{termoBusca}' ===\n");
        
        string sql = "SELECT * FROM Produtos WHERE Nome LIKE @TermoBusca ORDER BY Nome";
        
        using (SqlConnection conn = DatabaseConnection.GetConnection())
        {
            conn.Open();
            
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TermoBusca", "%" + termoBusca + "%");
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("{0,-5} {1,-30} {2,-15} {3,-10} {4,-12}", 
                        "ID", "Nome", "Preço", "Estoque", "CategoriaID");
                    Console.WriteLine(new string('-', 75));
                    
                    bool encontrou = false;
                    while (reader.Read())
                    {
                        encontrou = true;
                        Console.WriteLine("{0,-5} {1,-30} {2,-15:C} {3,-10} {4,-12}", 
                            reader["Id"],
                            reader["Nome"],
                            reader["Preco"],
                            reader["Estoque"],
                            reader["CategoriaId"]);
                    }
                    
                    if (!encontrou)
                    {
                        Console.WriteLine($"Nenhum produto encontrado com '{termoBusca}'.");
                    }
                }
            }
        }
    }
}