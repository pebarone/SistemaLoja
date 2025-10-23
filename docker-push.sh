#!/bin/bash
# Script para build e push da imagem Docker para o Docker Hub
# Autor: Sistema Loja - Lab 12

set -e

echo "================================================"
echo "  Sistema Loja - Docker Build and Push"
echo "================================================"
echo ""

# Verificar se Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "[ERRO] Docker não está rodando!"
    echo "Por favor, inicie o Docker e tente novamente."
    exit 1
fi

echo "[1/6] Docker verificado - OK"
echo ""

# Build da imagem local
echo "[2/6] Fazendo build da imagem..."
docker build -t sistemaloja:latest .
echo "Build concluído - OK"
echo ""

# Tag para Docker Hub - latest
echo "[3/6] Criando tag pbrnx/cp5:latest..."
docker tag sistemaloja:latest pbrnx/cp5:latest
echo "Tag latest criada - OK"
echo ""

# Tag para Docker Hub - v1.0
echo "[4/6] Criando tag pbrnx/cp5:v1.0..."
docker tag sistemaloja:latest pbrnx/cp5:v1.0
echo "Tag v1.0 criada - OK"
echo ""

# Login no Docker Hub (se necessário)
echo "[5/6] Verificando login no Docker Hub..."
echo "Se solicitado, faça login no Docker Hub:"
echo ""

# Push da imagem - latest
echo "[6/6] Fazendo push da imagem pbrnx/cp5:latest..."
docker push pbrnx/cp5:latest
echo "Push latest concluído - OK"
echo ""

# Push da imagem - v1.0
echo "Fazendo push da imagem pbrnx/cp5:v1.0..."
docker push pbrnx/cp5:v1.0
echo "Push v1.0 concluído - OK"
echo ""

echo "================================================"
echo "  SUCESSO! Imagem publicada no Docker Hub"
echo "================================================"
echo ""
echo "Imagens disponíveis:"
echo "  - docker pull pbrnx/cp5:latest"
echo "  - docker pull pbrnx/cp5:v1.0"
echo ""
echo "Acesse: https://hub.docker.com/r/pbrnx/cp5"
echo ""
