# Test Ordering API

$baseUrl = "http://localhost:5003"

Write-Host "=== Testing Ordering API ===" -ForegroundColor Green
Write-Host "Base URL: $baseUrl" -ForegroundColor Cyan

# Test 1: Health Check
Write-Host "`n1. Testing Health Endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/health" -Method GET -TimeoutSec 10
    Write-Host "Health: OK" -ForegroundColor Green
    Write-Host "Response: $($response | ConvertTo-Json)" -ForegroundColor Gray
} catch {
    Write-Host "Health Check Failed: $_" -ForegroundColor Red
}

# Test 2: Get Orders
Write-Host "`n2. Testing GET /orders..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/orders?PageIndex=0&PageSize=2" -Method GET -TimeoutSec 30
    Write-Host "GET Orders: SUCCESS" -ForegroundColor Green
    Write-Host "Total Orders: $($response.orders.count)" -ForegroundColor Cyan
    Write-Host "Page: $($response.orders.pageIndex) of $($response.orders.pageSize)" -ForegroundColor Cyan
    if ($response.orders.data.Count -gt 0) {
        Write-Host "First Order: $($response.orders.data[0].orderName)" -ForegroundColor Cyan
    }
} catch {
    Write-Host "GET Orders Failed: $_" -ForegroundColor Red
}

# Test 3: Get Orders by Customer
Write-Host "`n3. Testing GET /orders/customer/{id}..." -ForegroundColor Yellow
$customerId = "58c49479-ec65-4de2-86e7-033c546291aa"
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/orders/customer/$customerId" -Method GET -TimeoutSec 30
    Write-Host "GET Orders by Customer: SUCCESS" -ForegroundColor Green
    Write-Host "Orders found: $($response.orders.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "GET Orders by Customer Failed: $_" -ForegroundColor Red
}

# Test 4: Get Orders by Name
Write-Host "`n4. Testing GET /orders/name/{name}..." -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/orders/name/ORD" -Method GET -TimeoutSec 30
    Write-Host "GET Orders by Name: SUCCESS" -ForegroundColor Green
    Write-Host "Orders found: $($response.orders.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "GET Orders by Name Failed: $_" -ForegroundColor Red
}

Write-Host "`n=== Test Complete ===" -ForegroundColor Green
