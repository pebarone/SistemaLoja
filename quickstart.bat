@echo off
REM Script de Quick Start - Sistema Loja
REM Baixa e executa o ambiente completo do Docker Hub

echo ================================================
echo   Sistema Loja - Quick Start
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

echo [1/4] Docker verificado - OK
echo.

REM Criar pasta temporária
set TEMP_DIR=%TEMP%\sistemaloja-quickstart
if not exist "%TEMP_DIR%" mkdir "%TEMP_DIR%"
cd /d "%TEMP_DIR%"

echo [2/4] Baixando arquivos necessarios...
echo.

REM Baixar docker-compose-hub.yml
powershell -Command "Invoke-WebRequest -Uri 'https://raw.githubusercontent.com/pebarone/SistemaLoja/master/docker-compose-hub.yml' -OutFile 'docker-compose.yml'"
if %errorlevel% neq 0 (
    echo [ERRO] Falha ao baixar docker-compose.yml
    pause
    exit /b 1
)
echo   - docker-compose.yml baixado

REM Baixar setup.sql
powershell -Command "Invoke-WebRequest -Uri 'https://raw.githubusercontent.com/pebarone/SistemaLoja/master/setup.sql' -OutFile 'setup.sql'"
if %errorlevel% neq 0 (
    echo [ERRO] Falha ao baixar setup.sql
    pause
    exit /b 1
)
echo   - setup.sql baixado
echo.

echo [3/4] Arquivos baixados com sucesso!
echo Localizacao: %TEMP_DIR%
echo.

echo [4/4] Iniciando ambiente Docker...
echo.
echo AGUARDE: SQL Server pode levar 30-60 segundos para inicializar
echo.

REM Iniciar serviços em background primeiro
docker-compose up -d sqlserver sqlserver-init

echo Aguardando banco de dados inicializar...
timeout /t 15 /nobreak >nul

echo.
echo Iniciando aplicacao de forma interativa...
echo.

REM Executar app de forma interativa
docker-compose run --rm app

echo.
echo ================================================
echo   Ambiente encerrado
echo ================================================
echo.
echo Para parar os servicos:
echo   cd %TEMP_DIR%
echo   docker-compose down
echo.
echo Para reiniciar:
echo   cd %TEMP_DIR%
echo   docker-compose run --rm app
echo.
pause
