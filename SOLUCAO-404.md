# SOLUÇÃO RÁPIDA - Copie este arquivo

## Problema
Erro "404" ou "non-string key" ao executar docker-compose up

## Causa
Tentou baixar arquivos do GitHub que ainda não foram enviados (pushed)

## ✅ SOLUÇÃO IMEDIATA

### Opção 1: Copiar arquivo standalone (MAIS FÁCIL)

1. **Copie o arquivo `docker-compose-standalone.yml` para sua pasta de teste**:

```powershell
# Na pasta C:\Users\augus\Desktop\teste-docker
Copy-Item "C:\Users\augus\Desktop\SistemaLoja\docker-compose-standalone.yml" -Destination ".\docker-compose.yml"
```

2. **Execute**:
```bash
docker-compose up
```

**PRONTO!** Este arquivo é standalone e não precisa de nenhum arquivo externo.

---

### Opção 2: Usar imagem diretamente

```bash
# 1. Parar tudo
docker-compose down -v

# 2. Executar containers manualmente
docker network create loja-network

# SQL Server
docker run -d --name sistemaloja-sqlserver \
  --network loja-network \
  -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=SqlServer2024!" \
  -p 1433:1433 \
  mcr.microsoft.com/mssql/server:2022-latest

# Aguardar 30 segundos
timeout /t 30

# Aplicação
docker run -it --rm --name sistemaloja-app \
  --network loja-network \
  -e "SQL_SERVER=sistemaloja-sqlserver" \
  pbrnx/cp5:latest
```

---

### Opção 3: Clonar repositório completo

```bash
cd C:\Users\augus\Desktop
git clone https://github.com/pebarone/SistemaLoja.git
cd SistemaLoja
docker-compose up
```

---

## 🎯 RECOMENDAÇÃO

**Use a Opção 1** - é a mais simples e funciona offline!

```powershell
# Execute isto na pasta teste-docker:
cd C:\Users\augus\Desktop\teste-docker
Copy-Item "C:\Users\augus\Desktop\SistemaLoja\docker-compose-standalone.yml" -Destination ".\docker-compose.yml" -Force
docker-compose up
```

---

## 📝 O que mudou?

O arquivo `docker-compose-standalone.yml`:
- ✅ **NÃO** precisa de setup.sql externo
- ✅ **NÃO** precisa de Dockerfile
- ✅ **NÃO** precisa baixar nada da internet
- ✅ Script SQL está embutido no próprio docker-compose
- ✅ Usa imagem pronta do Docker Hub (pbrnx/cp5:latest)

---

## ⚠️ Nota

Depois que você fizer `git push` do projeto para o GitHub, aí sim os comandos com URLs funcionarão:

```bash
curl -o docker-compose.yml https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose-standalone.yml
docker-compose up
```

Mas por enquanto, use a cópia local!
