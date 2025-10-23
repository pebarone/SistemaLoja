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

# Wait for SQL Server to be ready before starting
COPY wait-for-sql.sh .
RUN chmod +x wait-for-sql.sh

ENTRYPOINT ["./wait-for-sql.sh"]
