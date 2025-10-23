@echo off
REM Script para build e push da imagem Docker para o Docker Hub
REM Autor: Sistema Loja - Lab 12

echo ================================================
echo   Sistema Loja - Docker Build and Push
echo ================================================
echo.

REM Verificar se Docker está rodando
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERRO] Docker nao esta rodando!
    echo Por favor, inicie o Docker Desktop e tente novamente.
    pause
    exit /b 1
)

echo [1/6] Docker verificado - OK
echo.

REM Build da imagem local
echo [2/6] Fazendo build da imagem...
docker build -t sistemaloja:latest .
if %errorlevel% neq 0 (
    echo [ERRO] Falha no build da imagem!
    pause
    exit /b 1
)
echo Build concluido - OK
echo.

REM Tag para Docker Hub - latest
echo [3/6] Criando tag pbrnx/cp5:latest...
docker tag sistemaloja:latest pbrnx/cp5:latest
if %errorlevel% neq 0 (
    echo [ERRO] Falha ao criar tag!
    pause
    exit /b 1
)
echo Tag latest criada - OK
echo.

REM Tag para Docker Hub - v1.0
echo [4/6] Criando tag pbrnx/cp5:v1.0...
docker tag sistemaloja:latest pbrnx/cp5:v1.0
if %errorlevel% neq 0 (
    echo [ERRO] Falha ao criar tag!
    pause
    exit /b 1
)
echo Tag v1.0 criada - OK
echo.

REM Login no Docker Hub (se necessário)
echo [5/6] Verificando login no Docker Hub...
echo Se solicitado, faca login no Docker Hub:
echo.

REM Push da imagem - latest
echo [6/6] Fazendo push da imagem pbrnx/cp5:latest...
docker push pbrnx/cp5:latest
if %errorlevel% neq 0 (
    echo [ERRO] Falha no push da imagem latest!
    echo Certifique-se de estar logado: docker login
    pause
    exit /b 1
)
echo Push latest concluido - OK
echo.

REM Push da imagem - v1.0
echo [6/6] Fazendo push da imagem pbrnx/cp5:v1.0...
docker push pbrnx/cp5:v1.0
if %errorlevel% neq 0 (
    echo [ERRO] Falha no push da imagem v1.0!
    pause
    exit /b 1
)
echo Push v1.0 concluido - OK
echo.

echo ================================================
echo   SUCESSO! Imagem publicada no Docker Hub
echo ================================================
echo.
echo Imagens disponiveis:
echo   - docker pull pbrnx/cp5:latest
echo   - docker pull pbrnx/cp5:v1.0
echo.
echo Acesse: https://hub.docker.com/r/pbrnx/cp5
echo.
pause
