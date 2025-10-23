@echo off
REM Script para executar Sistema Loja de forma interativa
REM Garante que o banco de dados seja criado antes da aplicacao iniciar

echo ================================================
echo   Sistema Loja - Execucao Interativa
echo ================================================
echo.

REM Verificar se Docker esta rodando
docker info >nul 2>&1
if %errorlevel% neq 0 (
    echo [ERRO] Docker nao esta rodando!
    echo Por favor, inicie o Docker Desktop e tente novamente.
    pause
    exit /b 1
)

echo [1/5] Docker verificado - OK
echo.

echo [2/5] Iniciando SQL Server...
docker-compose up -d sqlserver
echo.

echo [3/5] Aguardando SQL Server ficar saudavel...
echo (Isso pode levar ate 60 segundos)
echo.

:CHECK_HEALTH
docker-compose ps sqlserver | findstr "healthy" >nul 2>&1
if %errorlevel% neq 0 (
    echo   - SQL Server ainda nao esta pronto, aguardando...
    timeout /t 5 /nobreak >nul
    goto CHECK_HEALTH
)

echo.
echo SQL Server esta pronto!
echo.

echo [4/5] Criando banco de dados LojaDB...
docker-compose up sqlserver-init
echo.

echo [5/5] Iniciando aplicacao de forma INTERATIVA...
echo.
echo ================================================
echo   Pressione Ctrl+C para sair da aplicacao
echo ================================================
echo.
timeout /t 3 /nobreak >nul

docker-compose run --rm app

echo.
echo ================================================
echo   Aplicacao encerrada
echo ================================================
echo.
echo Para parar os servicos:
echo   docker-compose down
echo.
echo Para reiniciar a aplicacao:
echo   docker-compose run --rm app
echo.
pause
