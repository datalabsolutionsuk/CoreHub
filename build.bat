@echo off
REM CoreHub - Build and Validation Script (Windows)

echo ================================================================
echo                     CoreHub Build Script
echo ================================================================
echo.

REM Check dotnet
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [X] dotnet SDK not found
    exit /b 1
)
echo [OK] dotnet SDK installed

echo.

REM Clean
echo Cleaning previous builds...
dotnet clean --nologo >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo [X] Clean failed
    exit /b 1
)
echo [OK] Clean completed

echo.

REM Restore
echo Restoring NuGet packages...
dotnet restore --nologo
if %ERRORLEVEL% NEQ 0 (
    echo [X] Restore failed
    exit /b 1
)
echo [OK] Restore completed

echo.

REM Build
echo Building solution...
dotnet build --configuration Release --no-restore --nologo
if %ERRORLEVEL% NEQ 0 (
    echo [X] Build failed
    exit /b 1
)
echo [OK] Build completed

echo.

REM Test
echo Running tests...
dotnet test --configuration Release --no-build --verbosity quiet --nologo
if %ERRORLEVEL% NEQ 0 (
    echo [X] Tests failed
    exit /b 1
)
echo [OK] Tests passed

echo.
echo ================================================================
echo                    Build Successful!
echo ================================================================
echo.
echo Next steps:
echo   1. Set up database:  cd src\CoreHub.Infrastructure ^&^& dotnet ef database update --startup-project ..\CoreHub.Web
echo   2. Seed demo data:   cd src\CoreHub.Seed ^&^& dotnet run
echo   3. Run application:  cd src\CoreHub.Web ^&^& dotnet run
echo.
echo Then open: https://localhost:5001
echo Login as:  admin@demo.com / Admin123!
echo.
