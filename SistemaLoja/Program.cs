

using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using SistemaLoja.Lab12_ConexaoSQLServer;

namespace SistemaLoja
{
    // ===============================================
    // MODELOS DE DADOS
    // ===============================================

    // ===============================================
    // CLASSE DE CONEXÃO
    // ===============================================

    // ===============================================
    // REPOSITÓRIO DE PRODUTOS
    // ===============================================

    // ===============================================
    // REPOSITÓRIO DE PEDIDOS
    // ===============================================

    // ===============================================
    // CLASSE PRINCIPAL
    // ===============================================
    
    class Program
    {
        static void Main(string[] args)
        {
            // IMPORTANTE: Antes de executar, crie o banco de dados!
            // Execute o script SQL fornecido no arquivo setup.sql
            
            Console.WriteLine("=== LAB 12 - CONEXÃO SQL SERVER ===\n");
            
            var produtoRepo = new ProdutoRepository();
            var pedidoRepo = new PedidoRepository();
            
            bool continuar = true;
            
            while (continuar)
            {
                MostrarMenu();
                string? opcao = Console.ReadLine();
                
                try
                {
                    switch (opcao)
                    {
                        case "1":
                            produtoRepo.ListarTodosProdutos();
                            break;
                            
                        case "2":
                            InserirNovoProduto(produtoRepo);
                            break;
                            
                        case "3":
                            AtualizarProdutoExistente(produtoRepo);
                            break;
                            
                        case "4":
                            DeletarProdutoExistente(produtoRepo);
                            break;
                            
                        case "5":
                            ListarPorCategoria(produtoRepo);
                            break;
                            
                        case "6":
                            CriarNovoPedido(pedidoRepo);
                            break;
                            
                        case "7":
                            ListarPedidosDeCliente(pedidoRepo);
                            break;
                            
                        case "8":
                            DetalhesDoPedido(pedidoRepo);
                            break;
                            
                        case "0":
                            continuar = false;
                            break;
                            
                        default:
                            Console.WriteLine("Opção inválida!");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"\n❌ Erro SQL: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n❌ Erro: {ex.Message}");
                }
                
                if (continuar)
                {
                    Console.WriteLine("\nPressione qualquer tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            
            Console.WriteLine("\nPrograma finalizado!");
        }

        static void MostrarMenu()
        {
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║       MENU PRINCIPAL               ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  PRODUTOS                          ║");
            Console.WriteLine("║  1 - Listar todos os produtos      ║");
            Console.WriteLine("║  2 - Inserir novo produto          ║");
            Console.WriteLine("║  3 - Atualizar produto             ║");
            Console.WriteLine("║  4 - Deletar produto               ║");
            Console.WriteLine("║  5 - Listar por categoria          ║");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║  PEDIDOS                           ║");
            Console.WriteLine("║  6 - Criar novo pedido             ║");
            Console.WriteLine("║  7 - Listar pedidos de cliente     ║");
            Console.WriteLine("║  8 - Detalhes de um pedido         ║");
            Console.WriteLine("║                                    ║");
            Console.WriteLine("║  0 - Sair                          ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("\nEscolha uma opção: ");
        }

        // TODO: Implemente os métodos auxiliares abaixo
        
        static void InserirNovoProduto(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== INSERIR NOVO PRODUTO ===");
            
            try
            {
                Console.Write("Nome: ");
                string nome = Console.ReadLine() ?? "";
                
                Console.Write("Preço: ");
                decimal preco = decimal.Parse(Console.ReadLine() ?? "0");
                
                Console.Write("Estoque: ");
                int estoque = int.Parse(Console.ReadLine() ?? "0");
                
                Console.Write("ID da Categoria: ");
                int categoriaId = int.Parse(Console.ReadLine() ?? "0");
                
                var produto = new Produto
                {
                    Nome = nome,
                    Preco = preco,
                    Estoque = estoque,
                    CategoriaId = categoriaId
                };
                
                repo.InserirProduto(produto);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }

        static void AtualizarProdutoExistente(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== ATUALIZAR PRODUTO ===");
            
            try
            {
                Console.Write("ID do produto: ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                
                // Buscar o produto existente
                var produto = repo.BuscarPorId(id);
                
                if (produto == null)
                {
                    Console.WriteLine($"\n⚠️ Produto com ID {id} não encontrado!");
                    return;
                }
                
                Console.WriteLine($"\nProduto atual: {produto.Nome} - {produto.Preco:C} - Estoque: {produto.Estoque}");
                Console.WriteLine("\nDigite os novos valores (Enter para manter o atual):");
                
                Console.Write($"Nome [{produto.Nome}]: ");
                string? nome = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nome))
                    produto.Nome = nome;
                
                Console.Write($"Preço [{produto.Preco:C}]: ");
                string? precoStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(precoStr))
                    produto.Preco = decimal.Parse(precoStr);
                
                Console.Write($"Estoque [{produto.Estoque}]: ");
                string? estoqueStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(estoqueStr))
                    produto.Estoque = int.Parse(estoqueStr);
                
                Console.Write($"ID da Categoria [{produto.CategoriaId}]: ");
                string? catStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(catStr))
                    produto.CategoriaId = int.Parse(catStr);
                
                repo.AtualizarProduto(produto);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }

        static void DeletarProdutoExistente(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== DELETAR PRODUTO ===");
            
            try
            {
                Console.Write("ID do produto: ");
                int id = int.Parse(Console.ReadLine() ?? "0");
                
                // Buscar o produto para confirmar
                var produto = repo.BuscarPorId(id);
                
                if (produto == null)
                {
                    Console.WriteLine($"\n⚠️ Produto com ID {id} não encontrado!");
                    return;
                }
                
                Console.WriteLine($"\nProduto: {produto.Nome} - {produto.Preco:C}");
                Console.Write("Confirma a exclusão? (S/N): ");
                string? confirmacao = Console.ReadLine()?.ToUpper();
                
                if (confirmacao == "S")
                {
                    repo.DeletarProduto(id);
                }
                else
                {
                    Console.WriteLine("\n❌ Exclusão cancelada!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }

        static void ListarPorCategoria(ProdutoRepository repo)
        {
            Console.WriteLine("\n=== PRODUTOS POR CATEGORIA ===");
            
            try
            {
                Console.Write("ID da Categoria: ");
                int categoriaId = int.Parse(Console.ReadLine() ?? "0");
                
                repo.ListarProdutosPorCategoria(categoriaId);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }

        static void CriarNovoPedido(PedidoRepository repo)
        {
            Console.WriteLine("\n=== CRIAR NOVO PEDIDO ===");
            
            try
            {
                Console.Write("ID do Cliente: ");
                int clienteId = int.Parse(Console.ReadLine() ?? "0");
                
                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                    DataPedido = DateTime.Now,
                    ValorTotal = 0
                };
                
                List<PedidoItem> itens = new List<PedidoItem>();
                decimal valorTotal = 0;
                
                bool adicionarMais = true;
                while (adicionarMais)
                {
                    Console.Write("\nID do Produto: ");
                    int produtoId = int.Parse(Console.ReadLine() ?? "0");
                    
                    Console.Write("Quantidade: ");
                    int quantidade = int.Parse(Console.ReadLine() ?? "0");
                    
                    Console.Write("Preço Unitário: ");
                    decimal precoUnitario = decimal.Parse(Console.ReadLine() ?? "0");
                    
                    var item = new PedidoItem
                    {
                        ProdutoId = produtoId,
                        Quantidade = quantidade,
                        PrecoUnitario = precoUnitario
                    };
                    
                    itens.Add(item);
                    valorTotal += quantidade * precoUnitario;
                    
                    Console.Write("\nAdicionar mais itens? (S/N): ");
                    adicionarMais = Console.ReadLine()?.ToUpper() == "S";
                }
                
                pedido.ValorTotal = valorTotal;
                
                Console.WriteLine($"\nValor total do pedido: {valorTotal:C}");
                Console.Write("Confirmar criação do pedido? (S/N): ");
                
                if (Console.ReadLine()?.ToUpper() == "S")
                {
                    repo.CriarPedido(pedido, itens);
                }
                else
                {
                    Console.WriteLine("\n❌ Pedido cancelado!");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }

        static void ListarPedidosDeCliente(PedidoRepository repo)
        {
            Console.WriteLine("\n=== PEDIDOS DO CLIENTE ===");
            
            try
            {
                Console.Write("ID do Cliente: ");
                int clienteId = int.Parse(Console.ReadLine() ?? "0");
                
                repo.ListarPedidosCliente(clienteId);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }

        static void DetalhesDoPedido(PedidoRepository repo)
        {
            Console.WriteLine("\n=== DETALHES DO PEDIDO ===");
            
            try
            {
                Console.Write("ID do Pedido: ");
                int pedidoId = int.Parse(Console.ReadLine() ?? "0");
                
                repo.ObterDetalhesPedido(pedidoId);
            }
            catch (FormatException)
            {
                Console.WriteLine("\n❌ Erro: Formato de número inválido!");
            }
        }
    }
}