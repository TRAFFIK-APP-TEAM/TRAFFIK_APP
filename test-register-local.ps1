$body = @{
    FullName = "Test User"
    Email = "test@example.com"
    Password = "TestPassword123!"
    PhoneNumber = "1234567890"
    RoleId = 2
} | ConvertTo-Json

Write-Host "Registering user locally..."
Write-Host "Body: $body"

try {
    $response = Invoke-WebRequest -Uri 'http://localhost:5027/api/Auth/Register' -Method POST -Body $body -ContentType 'application/json' -UseBasicParsing
    Write-Host "SUCCESS!"
    Write-Host "Status: $($response.StatusCode)"
    Write-Host "Body: $($response.Content)"
} catch {
    Write-Host "ERROR!"
    Write-Host "Status Code: $($_.Exception.Response.StatusCode.value__)"
    Write-Host "Error: $($_.Exception.Message)"
}

Write-Host "`n`nNow testing login with same credentials..."
$loginBody = @{
    Email = "test@example.com"
    Password = "TestPassword123!"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri 'http://localhost:5027/api/Auth/Login' -Method POST -Body $loginBody -ContentType 'application/json' -UseBasicParsing
    Write-Host "LOGIN SUCCESS!"
    Write-Host "Response: $($response.Content)"
} catch {
    Write-Host "LOGIN FAILED!"
    Write-Host "Error: $($_.Exception.Message)"
}

