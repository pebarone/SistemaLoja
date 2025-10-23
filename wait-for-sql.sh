#!/bin/bash
set -e

host="${SQL_SERVER:-sqlserver}"
password="SqlServer2024!"

echo "======================================="
echo "  Sistema Loja - Iniciando..."
echo "======================================="
echo ""
echo "Aguardando SQL Server ($host) ficar disponível..."

# Aguardar SQL Server responder
until /opt/mssql-tools18/bin/sqlcmd -S $host -U sa -P $password -Q "SELECT 1" -C &> /dev/null
do
  echo "  - SQL Server ainda não disponível, aguardando..."
  sleep 2
done

echo "✓ SQL Server está respondendo!"
echo ""
echo "Verificando se banco LojaDB existe..."

# Aguardar banco de dados LojaDB estar pronto
max_attempts=30
attempt=0
until /opt/mssql-tools18/bin/sqlcmd -S $host -U sa -P $password -d LojaDB -Q "SELECT 1" -C &> /dev/null
do
  attempt=$((attempt+1))
  if [ $attempt -ge $max_attempts ]; then
    echo ""
    echo "❌ ERRO: Banco LojaDB não foi criado após $max_attempts tentativas"
    echo ""
    echo "Execute manualmente:"
    echo "  docker-compose up sqlserver-init"
    echo ""
    exit 1
  fi
  echo "  - Aguardando banco LojaDB ser criado... (tentativa $attempt/$max_attempts)"
  sleep 2
done

echo "✓ Banco de dados LojaDB está pronto!"
echo ""
echo "======================================="
echo "  Iniciando aplicação..."
echo "======================================="
echo ""

dotnet SistemaLoja.dll
