$body = @{
    Email = 'BobbyB@gmail.com'
    Password = 'TestPassword123!'
} | ConvertTo-Json

Write-Host "Testing Login with Bobby's account..."
Write-Host "Body: $body"

try {
    $response = Invoke-WebRequest -Uri 'https://traffikapi-a0bhabb4bag8g3g6.southafricanorth-01.azurewebsites.net/api/Auth/Login' -Method POST -Body $body -ContentType 'application/json' -UseBasicParsing
    Write-Host "SUCCESS!"
    Write-Host "Status: $($response.StatusCode)"
    Write-Host "Body: $($response.Content)"
} catch {
    Write-Host "ERROR!"
    Write-Host "Status Code: $($_.Exception.Response.StatusCode.value__)"
    Write-Host "Error Message: $($_.Exception.Message)"
    
    if ($_.Exception.Response) {
        try {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $responseBody = $reader.ReadToEnd()
            Write-Host "Response Body: $responseBody"
        } catch {
            Write-Host "Could not read response body"
        }
    }
}

