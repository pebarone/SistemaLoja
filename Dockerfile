# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY SistemaLoja/SistemaLoja.csproj SistemaLoja/
RUN dotnet restore SistemaLoja/SistemaLoja.csproj

# Copy everything else and build
COPY SistemaLoja/ SistemaLoja/
WORKDIR /src/SistemaLoja
RUN dotnet build SistemaLoja.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish SistemaLoja.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar sqlcmd para verificação do banco de dados
RUN apt-get update && apt-get install -y curl gnupg2 \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools18 \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Adicionar sqlcmd ao PATH
ENV PATH="$PATH:/opt/mssql-tools18/bin"

# Wait for SQL Server to be ready before starting
COPY wait-for-sql.sh .
RUN chmod +x wait-for-sql.sh

ENTRYPOINT ["./wait-for-sql.sh"]
