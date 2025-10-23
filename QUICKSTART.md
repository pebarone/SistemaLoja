# Quick Start - Sistema Loja

Guia rápido para executar o Sistema Loja sem clonar o repositório.

## 🚀 Execução Rápida (sem código fonte)

### Pré-requisito
- Docker Desktop instalado e rodando

### Passo a Passo

#### Windows (PowerShell)

```powershell
# 1. Criar pasta
mkdir sistemaloja-teste
cd sistemaloja-teste

# 2. Baixar arquivos necessários
Invoke-WebRequest -Uri "https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose-hub.yml" -OutFile "docker-compose.yml"
Invoke-WebRequest -Uri "https://raw.githubusercontent.com/pebarone/SistemaLoja/master/setup.sql" -OutFile "setup.sql"

# 3. Executar
docker-compose up
```

#### Linux/Mac

```bash
# 1. Criar pasta
mkdir sistemaloja-teste
cd sistemaloja-teste

# 2. Baixar arquivos necessários
curl -o docker-compose.yml https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose-hub.yml
curl -o setup.sql https://raw.githubusercontent.com/pebarone/SistemaLoja/master/setup.sql

# 3. Executar
docker-compose up
```

### O que acontece?

1. **SQL Server** é iniciado em um container
2. **Banco de dados** é criado automaticamente via `setup.sql`
3. **Aplicação** é baixada do Docker Hub (`pbrnx/cp5:latest`)
4. **Tudo conectado** automaticamente

### Comandos Úteis

```bash
# Parar tudo
docker-compose down

# Resetar (apagar dados)
docker-compose down -v

# Ver logs
docker-compose logs -f

# Executar em background
docker-compose up -d
```

## 🎯 Testando a Aplicação

Após executar `docker-compose up`, você verá o menu:

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

### Dados de Teste

O banco já vem com dados de exemplo:
- **20 produtos** em 5 categorias
- **5 clientes**
- **3 pedidos** de exemplo

## 🐛 Troubleshooting

### "No such file or directory: Dockerfile"

**Causa**: Você baixou o `docker-compose.yml` errado (que faz build).

**Solução**: Use o `docker-compose-hub.yml`:
```bash
curl -o docker-compose.yml https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose-hub.yml
```

### "Cannot connect to SQL Server"

**Causa**: SQL Server ainda está inicializando.

**Solução**: Aguarde 30-60 segundos e tente novamente.

### "Port 1433 already in use"

**Causa**: Você já tem um SQL Server rodando.

**Solução**: 
```bash
# Parar outro SQL Server
docker stop $(docker ps -q --filter "expose=1433")

# Ou mudar a porta no docker-compose.yml
ports:
  - "1434:1433"  # Usar porta 1434 no host
```

## 📝 Estrutura de Arquivos Mínima

Você só precisa de 2 arquivos:

```
sistemaloja-teste/
├── docker-compose.yml  (baixado do docker-compose-hub.yml)
└── setup.sql           (script de inicialização do banco)
```

## 🔗 Links

- Docker Hub: https://hub.docker.com/r/pbrnx/cp5
- GitHub: https://github.com/pebarone/SistemaLoja
- README Completo: https://github.com/pebarone/SistemaLoja/blob/master/README.md

---

**Desenvolvido para**: Lab 12 - C# Software Development  
**Autor**: Augusto Barone - Turma 3ESPY
