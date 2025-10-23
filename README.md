# Sistema Loja - Lab 12 C# SQL Server

[![Docker](https://img.shields.io/badge/Docker-Ready-blue)](https://hub.docker.com/r/pbrnx/cp5)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-red)](https://www.microsoft.com/sql-server)

Sistema completo de gerenciamento de loja com SQL Server, desenvolvido para o Lab 12 da disciplina C# Software Development.

## 🚀 Execução Rápida com Docker

### ⚡ Super Rápido (1 comando)

**Windows**: Baixe e execute [`quickstart.bat`](./quickstart.bat)

```bash
# Baixar e executar
curl -o quickstart.bat https://raw.githubusercontent.com/pebarone/SistemaLoja/master/quickstart.bat
quickstart.bat
```

### Opção 1: Ambiente Completo do Docker Hub

**IMPORTANTE**: Para terminal interativo, use `docker-compose run` em vez de `docker-compose up`.

```bash
# 1. Criar uma pasta para o projeto
mkdir sistemaloja-teste
cd sistemaloja-teste

# 2. Baixar o arquivo standalone
curl -o docker-compose.yml https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose-standalone.yml

# 3a. Iniciar serviços em background
docker-compose up -d sqlserver sqlserver-init

# 3b. Aguardar banco inicializar (30-60 segundos)
# Verificar status: docker-compose ps

# 3c. Executar aplicação de forma INTERATIVA
docker-compose run --rm app

# Alternativa: Executar tudo junto (não interativo)
docker-compose up
```

**Vantagem**: Este arquivo é standalone e **NÃO** precisa de setup.sql ou Dockerfile externos!

### Opção 2: Docker Compose (Recomendado - Build Local)

```bash
# Clonar o repositório
git clone https://github.com/pebarone/SistemaLoja.git
cd SistemaLoja

# Iniciar serviços (SQL Server + inicialização do banco)
docker-compose up -d sqlserver sqlserver-init

# Aguardar inicialização (verificar: docker-compose ps)

# Executar aplicação de forma INTERATIVA
docker-compose run --rm app

# Alternativa: Build local e executar tudo junto (não interativo)
docker-compose up --build
```

### Opção 3: Imagem do Docker Hub

```bash
# Pull da imagem pronta
docker pull pbrnx/cp5:latest

# Executar (requer SQL Server rodando)
docker run -it --rm \
  -e SQL_SERVER=host.docker.internal \
  pbrnx/cp5:latest
```



## 📦 O que está incluído?

### Funcionalidades Implementadas

✅ **CRUD Completo de Produtos**
- Listar todos os produtos
- Inserir novo produto
- Atualizar produto existente
- Deletar produto
- Buscar produto por ID

✅ **Gestão de Pedidos**
- Criar pedido com múltiplos itens
- Listar pedidos de cliente
- Detalhes completos do pedido
- Atualização automática de estoque

✅ **Recursos Avançados**
- Listagem por categoria com JOIN
- Transações ACID (SqlTransaction)
- Proteção contra SQL Injection
- Validação de entrada
- Tratamento de erros

### Tecnologias

- **Backend**: .NET 9.0 / C#
- **Banco de Dados**: SQL Server 2022
- **ORM**: ADO.NET (SqlClient)
- **Containerização**: Docker + Docker Compose
- **CI/CD**: GitHub Actions ready

## 🏗️ Estrutura do Projeto

```
SistemaLoja/
├── SistemaLoja/
│   ├── Program.cs              # Classe principal e menu
│   ├── DatabaseConnection.cs   # Gerenciamento de conexão
│   ├── ProdutoRepository.cs    # CRUD de produtos
│   ├── PedidoRepository.cs     # CRUD de pedidos
│   ├── Produto.cs              # Model
│   ├── Pedido.cs               # Model
│   ├── PedidoItem.cs           # Model
│   ├── Cliente.cs              # Model
│   └── Categoria.cs            # Model
├── setup.sql                   # Script de inicialização do banco
├── Dockerfile                  # Build da aplicação
├── docker-compose.yml          # Orquestração dos containers
└── README.md                   # Este arquivo
```

## 💻 Desenvolvimento Local (sem Docker)

### Pré-requisitos

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) ou Docker
- [Azure Data Studio](https://docs.microsoft.com/sql/azure-data-studio/download) (opcional)

### Setup do Banco de Dados

```bash
# 1. Iniciar SQL Server via Docker
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=SqlServer2024!" \
  -p 1433:1433 --name sqlserver2022 -d \
  mcr.microsoft.com/mssql/server:2022-latest

# 2. Executar script de setup
# Use Azure Data Studio ou sqlcmd para executar setup.sql
```

### Executar a Aplicação

```bash
# Restaurar dependências
dotnet restore

# Compilar
dotnet build

# Executar
cd SistemaLoja
dotnet run
```

## 🗄️ Modelo de Dados

```
┌─────────────┐
│  Clientes   │
└──────┬──────┘
       │ 1:N
       ▼
┌─────────────┐      N:M      ┌──────────────┐
│   Pedidos   │◄─────────────►│ PedidoItens  │
└─────────────┘                └──────┬───────┘
                                      │
                                      │ N:1
                                      ▼
┌──────────────┐      1:N      ┌─────────────┐
│  Categorias  │◄──────────────┤  Produtos   │
└──────────────┘                └─────────────┘
```

## 🎯 Funcionalidades do Menu

```
╔════════════════════════════════════╗
║       MENU PRINCIPAL               ║
╠════════════════════════════════════╣
║  PRODUTOS                          ║
║  1 - Listar todos os produtos      ║
║  2 - Inserir novo produto          ║
║  3 - Atualizar produto             ║
║  4 - Deletar produto               ║
║  5 - Listar por categoria          ║
║                                    ║
║  PEDIDOS                           ║
║  6 - Criar novo pedido             ║
║  7 - Listar pedidos de cliente     ║
║  8 - Detalhes de um pedido         ║
║                                    ║
║  0 - Sair                          ║
╚════════════════════════════════════╝
```

## 🔒 Segurança

✅ Queries parametrizadas (proteção SQL Injection)  
✅ Validação de entrada do usuário  
✅ Tratamento de exceções adequado  
✅ Transações para operações críticas  
✅ Connection string com TrustServerCertificate

## 🐳 Docker

### Build Local

```bash
# Build da imagem
docker build -t sistemaloja:latest .

# Executar com docker-compose
docker-compose up --build
```

### Push para Docker Hub

```bash
# Login no Docker Hub
docker login

# Tag da imagem
docker tag sistemaloja:latest pbrnx/cp5:latest
docker tag sistemaloja:latest pbrnx/cp5:v1.0

# Push
docker push pbrnx/cp5:latest
docker push pbrnx/cp5:v1.0
```

## 🛠️ Comandos Úteis

```bash
# Parar containers
docker-compose down

# Parar e remover volumes (reset completo)
docker-compose down -v

# Ver logs
docker-compose logs -f

# Logs de um serviço específico
docker-compose logs sqlserver
docker-compose logs app

# Acessar container em execução
docker exec -it sistemaloja-app bash

# Conectar ao SQL Server
docker exec -it sistemaloja-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P SqlServer2024! -C
```

## 📊 Dados de Teste

O script `setup.sql` cria automaticamente:

- **3 Categorias**: Eletrônicos, Roupas, Alimentos
- **6 Produtos**: Notebook, Mouse, Camiseta, Calça Jeans, Arroz, Feijão
- **2 Clientes**: João Silva, Maria Santos
- **Dados de exemplo** para testes

## 🔧 Troubleshooting

### Container não inicia
```bash
docker-compose logs
docker-compose down -v
docker-compose up --build
```

### Erro de conexão com banco
```bash
# Verificar se SQL Server está pronto
docker-compose logs sqlserver-init

# Aguardar mais tempo para inicialização
```

### Resetar tudo
```bash
docker-compose down -v
docker system prune -f
docker-compose up --build
```

## 📚 Lab 12 - Requisitos Atendidos

- ✅ **Exercício 1**: Connection String configurada
- ✅ **Exercício 2**: Listar produtos com ExecuteReader
- ✅ **Exercício 3**: Inserir produto com parâmetros
- ✅ **Exercício 4**: Atualizar produto
- ✅ **Exercício 5**: Deletar produto
- ✅ **Exercício 6**: Buscar por ID
- ✅ **Exercício 7**: Listar por categoria (JOIN)
- ✅ **Exercício 8**: Criar pedido com transação
- ✅ **Exercício 9**: Listar pedidos de cliente
- ✅ **Exercício 10**: Detalhes do pedido com JOIN

## 👨‍💻 Autor

**Augusto Barone**  
Turma: 3ESPY  
Disciplina: C# Software Development  
Professor: Charles

## 📄 Licença

Este projeto foi desenvolvido para fins educacionais.

## 🔗 Links

- **[Quick Start Guide](./QUICKSTART.md)** - Guia de início rápido sem clonar repositório
- **[Docker Hub](https://hub.docker.com/r/pbrnx/cp5)** - Imagens prontas
- **[GitHub Repository](https://github.com/pebarone/SistemaLoja)** - Código fonte
- **[Documentação Docker](./README-DOCKER.md)** - Guia completo Docker
- **[Deployment Guide](./DOCKER-HUB-DEPLOYMENT.md)** - Como foi feito o deployment

---

**Nota**: Certifique-se de ter o Docker instalado e rodando antes de executar os comandos acima.
