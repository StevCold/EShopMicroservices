# Run Ordering API Locally

Write-Host "=== Stopping any running processes ===" -ForegroundColor Yellow

# Kill any running Ordering.API processes
Get-Process | Where-Object { $_.ProcessName -like "*Ordering.API*" } | Stop-Process -Force -ErrorAction SilentlyContinue

# Kill any processes using port 5003 or 6003
$ports = @(5003, 6003, 6063)
foreach ($port in $ports) {
    $connection = netstat -ano | findstr ":$port"
    if ($connection) {
        $parts = $connection -split '\s+'
        $pid = $parts[$parts.Length - 1]
        if ($pid -match '^\d+$') {
            Write-Host "Killing process on port $port (PID: $pid)" -ForegroundColor Red
            taskkill /F /PID $pid 2>$null
        }
    }
}

Write-Host "`n=== Starting SQL Server ===" -ForegroundColor Yellow

# Go to project directory
$projectPath = "C:\Users\stefa\source\repos\EShopMicroservices"
cd $projectPath

# Start only SQL Server in Docker
docker-compose up orderdb -d

Write-Host "Waiting 20 seconds for SQL Server to start..." -ForegroundColor Yellow
Start-Sleep -Seconds 20

# Check if SQL Server is running
$sqlRunning = docker ps | findstr "orderdb"
if (-not $sqlRunning) {
    Write-Host "ERROR: SQL Server container failed to start!" -ForegroundColor Red
    exit 1
}

Write-Host "SQL Server is running!" -ForegroundColor Green

Write-Host "`n=== Building Solution ===" -ForegroundColor Yellow

dotnet clean
$buildResult = dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "BUILD FAILED! Check errors above." -ForegroundColor Red
    exit 1
}

Write-Host "Build successful!" -ForegroundColor Green

Write-Host "`n=== Starting Ordering.API ===" -ForegroundColor Green
Write-Host "API will be available at: http://localhost:5003" -ForegroundColor Cyan
Write-Host "Test URL: http://localhost:5003/orders?PageIndex=0&PageSize=2" -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop the API" -ForegroundColor Yellow
Write-Host ""

# Run the API
cd "$projectPath\src\Ordering.API"
dotnet run --launch-profile http
