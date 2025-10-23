-- =============================================
-- LAB 12 - SETUP DO BANCO DE DADOS
-- Sistema de Loja - SQL Server
-- =============================================

-- Criar banco de dados
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'LojaDB')
BEGIN
    CREATE DATABASE LojaDB;
    PRINT 'Banco de dados LojaDB criado com sucesso!';
END
GO

USE LojaDB;
GO

-- =============================================
-- TABELAS
-- =============================================

-- Tabela de Categorias
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Categorias')
BEGIN
    CREATE TABLE Categorias (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(100) NOT NULL,
        Descricao NVARCHAR(500)
    );
    PRINT 'Tabela Categorias criada!';
END
GO

-- Tabela de Produtos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Produtos')
BEGIN
    CREATE TABLE Produtos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(200) NOT NULL,
        Preco DECIMAL(18,2) NOT NULL,
        Estoque INT NOT NULL DEFAULT 0,
        CategoriaId INT NOT NULL,
        CONSTRAINT FK_Produtos_Categorias FOREIGN KEY (CategoriaId) 
            REFERENCES Categorias(Id)
    );
    PRINT 'Tabela Produtos criada!';
END
GO

-- Tabela de Clientes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
BEGIN
    CREATE TABLE Clientes (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Nome NVARCHAR(200) NOT NULL,
        Email NVARCHAR(200) NOT NULL,
        Telefone NVARCHAR(20)
    );
    PRINT 'Tabela Clientes criada!';
END
GO

-- Tabela de Pedidos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pedidos')
BEGIN
    CREATE TABLE Pedidos (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ClienteId INT NOT NULL,
        DataPedido DATETIME NOT NULL DEFAULT GETDATE(),
        ValorTotal DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId) 
            REFERENCES Clientes(Id)
    );
    PRINT 'Tabela Pedidos criada!';
END
GO

-- Tabela de Itens de Pedido
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PedidoItens')
BEGIN
    CREATE TABLE PedidoItens (
        Id INT PRIMARY KEY IDENTITY(1,1),
        PedidoId INT NOT NULL,
        ProdutoId INT NOT NULL,
        Quantidade INT NOT NULL,
        PrecoUnitario DECIMAL(18,2) NOT NULL,
        CONSTRAINT FK_PedidoItens_Pedidos FOREIGN KEY (PedidoId) 
            REFERENCES Pedidos(Id),
        CONSTRAINT FK_PedidoItens_Produtos FOREIGN KEY (ProdutoId) 
            REFERENCES Produtos(Id)
    );
    PRINT 'Tabela PedidoItens criada!';
END
GO

-- =============================================
-- DADOS DE EXEMPLO
-- =============================================

-- Inserir Categorias
IF NOT EXISTS (SELECT * FROM Categorias)
BEGIN
    INSERT INTO Categorias (Nome, Descricao) VALUES
        ('Eletrônicos', 'Produtos eletrônicos e tecnologia'),
        ('Livros', 'Livros e publicações'),
        ('Roupas', 'Vestuário e acessórios'),
        ('Alimentos', 'Produtos alimentícios'),
        ('Casa e Jardim', 'Itens para casa e jardinagem');
    
    PRINT 'Categorias inseridas!';
END
GO

-- Inserir Produtos
IF NOT EXISTS (SELECT * FROM Produtos)
BEGIN
    INSERT INTO Produtos (Nome, Preco, Estoque, CategoriaId) VALUES
        -- Eletrônicos
        ('Notebook Dell Inspiron 15', 3499.99, 15, 1),
        ('Mouse Logitech MX Master', 349.90, 50, 1),
        ('Teclado Mecânico Redragon', 289.90, 30, 1),
        ('Monitor LG 24"', 899.00, 20, 1),
        ('Webcam Logitech C920', 459.90, 25, 1),
        
        -- Livros
        ('Clean Code - Robert Martin', 89.90, 100, 2),
        ('Design Patterns - Gang of Four', 120.00, 45, 2),
        ('The Pragmatic Programmer', 95.50, 60, 2),
        ('Refactoring - Martin Fowler', 110.00, 35, 2),
        ('Domain-Driven Design', 130.00, 28, 2),
        
        -- Roupas
        ('Camiseta Básica Preta', 49.90, 200, 3),
        ('Calça Jeans Masculina', 159.90, 80, 3),
        ('Tênis Esportivo', 299.90, 45, 3),
        ('Jaqueta Corta-Vento', 189.90, 35, 3),
        
        -- Alimentos
        ('Café Premium 500g', 29.90, 150, 4),
        ('Chocolate Suíço', 15.90, 200, 4),
        ('Biscoito Integral', 8.90, 300, 4),
        
        -- Casa e Jardim
        ('Vaso Decorativo', 45.00, 40, 5),
        ('Almofada Estampada', 35.90, 60, 5),
        ('Tapete Sala 2x3m', 249.90, 15, 5);
    
    PRINT 'Produtos inseridos!';
END
GO

-- Inserir Clientes
IF NOT EXISTS (SELECT * FROM Clientes)
BEGIN
    INSERT INTO Clientes (Nome, Email, Telefone) VALUES
        ('João Silva', 'joao.silva@email.com', '11987654321'),
        ('Maria Santos', 'maria.santos@email.com', '11976543210'),
        ('Pedro Oliveira', 'pedro.oliveira@email.com', '11965432109'),
        ('Ana Costa', 'ana.costa@email.com', '11954321098'),
        ('Carlos Souza', 'carlos.souza@email.com', '11943210987');
    
    PRINT 'Clientes inseridos!';
END
GO

-- Inserir Pedidos de Exemplo
IF NOT EXISTS (SELECT * FROM Pedidos)
BEGIN
    -- Pedido 1 - João Silva
    INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) 
    VALUES (1, '2024-10-15 10:30:00', 3849.89);
    
    INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario) VALUES
        (1, 1, 1, 3499.99),  -- Notebook
        (1, 2, 1, 349.90);   -- Mouse
    
    -- Pedido 2 - Maria Santos
    INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) 
    VALUES (2, '2024-10-16 14:20:00', 305.40);
    
    INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario) VALUES
        (2, 6, 2, 89.90),    -- Clean Code (2x)
        (2, 8, 1, 95.50),    -- The Pragmatic Programmer
        (2, 15, 1, 29.90);   -- Café
    
    -- Pedido 3 - Pedro Oliveira
    INSERT INTO Pedidos (ClienteId, DataPedido, ValorTotal) 
    VALUES (3, '2024-10-17 16:45:00', 509.70);
    
    INSERT INTO PedidoItens (PedidoId, ProdutoId, Quantidade, PrecoUnitario) VALUES
        (3, 11, 2, 49.90),   -- Camiseta (2x)
        (3, 12, 1, 159.90),  -- Calça Jeans
        (3, 20, 1, 249.90);  -- Tapete
    
    PRINT 'Pedidos de exemplo inseridos!';
    
    -- Atualizar estoque dos produtos vendidos
    UPDATE Produtos SET Estoque = Estoque - 1 WHERE Id = 1;  -- Notebook
    UPDATE Produtos SET Estoque = Estoque - 1 WHERE Id = 2;  -- Mouse
    UPDATE Produtos SET Estoque = Estoque - 2 WHERE Id = 6;  -- Clean Code
    UPDATE Produtos SET Estoque = Estoque - 1 WHERE Id = 8;  -- The Pragmatic
    UPDATE Produtos SET Estoque = Estoque - 1 WHERE Id = 15; -- Café
    UPDATE Produtos SET Estoque = Estoque - 2 WHERE Id = 11; -- Camiseta
    UPDATE Produtos SET Estoque = Estoque - 1 WHERE Id = 12; -- Calça
    UPDATE Produtos SET Estoque = Estoque - 1 WHERE Id = 20; -- Tapete
END
GO

-- =============================================
-- VERIFICAÇÃO
-- =============================================

PRINT '';
PRINT '========================================';
PRINT 'SETUP CONCLUÍDO COM SUCESSO!';
PRINT '========================================';
PRINT '';
PRINT 'Resumo do banco de dados:';
SELECT 'Categorias' as Tabela, COUNT(*) as Registros FROM Categorias
UNION ALL
SELECT 'Produtos', COUNT(*) FROM Produtos
UNION ALL
SELECT 'Clientes', COUNT(*) FROM Clientes
UNION ALL
SELECT 'Pedidos', COUNT(*) FROM Pedidos
UNION ALL
SELECT 'PedidoItens', COUNT(*) FROM PedidoItens;
GO

PRINT '';
PRINT 'Banco de dados pronto para uso!';
PRINT 'Connection String: Server=localhost,1433;Database=LojaDB;User Id=sa;Password=SqlServer2024!;TrustServerCertificate=True;';
GO
