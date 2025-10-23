# ✅ Docker Hub - Deployment Completo

## 🎉 Imagens Publicadas com Sucesso!

### Docker Hub Repository
**URL**: https://hub.docker.com/r/pbrnx/cp5

### Tags Disponíveis
- `pbrnx/cp5:latest` - Versão mais recente
- `pbrnx/cp5:v1.0` - Versão 1.0 (estável)

---

## 🚀 Como Testar

### Opção 1: Pull e Run (Simples)

```bash
# Pull da imagem
docker pull pbrnx/cp5:latest

# Executar (requer SQL Server local)
docker run -it --rm pbrnx/cp5:latest
```

### Opção 2: Docker Compose (Ambiente Completo)

```bash
# Clonar o repositório
git clone https://github.com/pebarone/SistemaLoja.git
cd SistemaLoja

# Iniciar tudo
docker-compose up
```

### Opção 3: Docker Compose direto do GitHub

```bash
# Baixar docker-compose.yml
curl -o docker-compose.yml https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose.yml

# Executar
docker-compose up
```

---

## 📊 Informações da Imagem

```bash
# Ver detalhes da imagem
docker inspect pbrnx/cp5:latest

# Ver histórico
docker history pbrnx/cp5:latest

# Ver tamanho
docker images pbrnx/cp5
```

---

## 🔍 Verificação

### 1. Verificar se a imagem existe no Docker Hub

Acesse: https://hub.docker.com/r/pbrnx/cp5/tags

Você deve ver:
- ✅ latest
- ✅ v1.0

### 2. Testar Pull

```bash
docker pull pbrnx/cp5:latest
```

**Output esperado:**
```
latest: Pulling from pbrnx/cp5
...
Status: Downloaded newer image for pbrnx/cp5:latest
docker.io/pbrnx/cp5:latest
```

### 3. Testar Execução

```bash
# Com docker-compose (recomendado)
docker-compose up

# Ou standalone (requer SQL Server)
docker run -it --rm \
  -e SQL_SERVER=host.docker.internal \
  pbrnx/cp5:latest
```

---

## 📝 Metadados da Imagem

- **Nome**: pbrnx/cp5
- **Tags**: latest, v1.0
- **Base**: mcr.microsoft.com/dotnet/runtime:9.0
- **Plataforma**: linux/amd64
- **Tamanho**: ~220MB
- **Aplicação**: Sistema Loja - Lab 12 C#
- **Banco**: SQL Server 2022

---

## 🔗 Links Úteis

- **Docker Hub**: https://hub.docker.com/r/pbrnx/cp5
- **GitHub**: https://github.com/pebarone/SistemaLoja
- **README Principal**: [README.md](./README.md)
- **README Docker**: [README-DOCKER.md](./README-DOCKER.md)

---

## 🎓 Informações Acadêmicas

- **Projeto**: Sistema Loja - Lab 12
- **Disciplina**: C# Software Development
- **Turma**: 3ESPY
- **Aluno**: Augusto Barone
- **Professor**: Charles

---

## 📦 Conteúdo da Imagem

A imagem contém:
- ✅ Aplicação .NET 9.0 compilada
- ✅ Dependências: Microsoft.Data.SqlClient
- ✅ Script wait-for-sql.sh
- ✅ Configuração automática de conexão

---

## 🔄 Atualizações Futuras

Para publicar uma nova versão:

```bash
# Build
docker build -t sistemaloja:latest .

# Tag
docker tag sistemaloja:latest pbrnx/cp5:latest
docker tag sistemaloja:latest pbrnx/cp5:v1.1

# Push
docker push pbrnx/cp5:latest
docker push pbrnx/cp5:v1.1
```

Ou use o script automatizado:

```bash
# Windows
.\docker-push.bat

# Linux/Mac
./docker-push.sh
```

---

**Data de Publicação**: 22 de Outubro de 2025  
**Digest (latest)**: sha256:177cc319e282d92e7a3f861ea09d288b71d0ed72c1dfbc9e9b6876040cdfd302  
**Digest (v1.0)**: sha256:177cc319e282d92e7a3f861ea09d288b71d0ed72c1dfbc9e9b6876040cdfd302
