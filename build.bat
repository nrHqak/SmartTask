@echo off
setlocal

echo [1/2] Restoring NuGet packages...
dotnet restore SmartTaskDistributor.csproj
if errorlevel 1 (
  echo Restore failed.
  exit /b 1
)

echo [2/2] Building project...
dotnet build SmartTaskDistributor.csproj -c Debug --no-restore
if errorlevel 1 (
  echo Build failed.
  exit /b 1
)

echo Build completed successfully.
exit /b 0
