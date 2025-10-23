#!/bin/bash
set -e

echo "Aguardando SQL Server inicializar..."
sleep 5

echo "Iniciando aplicação Sistema Loja..."
dotnet SistemaLoja.dll
