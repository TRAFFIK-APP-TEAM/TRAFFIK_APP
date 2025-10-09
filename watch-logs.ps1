# Live view TRAFFIK_APP logs
$desktopPath = [Environment]::GetFolderPath("Desktop")
$logsFolder = Join-Path $desktopPath "TRAFFIK_APP_Logs"
$todayLog = Join-Path $logsFolder "debug_$(Get-Date -Format 'yyyy-MM-dd').log"

Write-Host "Watching log file: $todayLog" -ForegroundColor Cyan
Write-Host "Press Ctrl+C to stop watching" -ForegroundColor Yellow
Write-Host "============================================" -ForegroundColor Green
Write-Host ""

if (Test-Path $todayLog) {
    # Show last 20 lines first, then follow new content
    Get-Content $todayLog -Tail 20 -Wait
} else {
    Write-Host "Log file doesn't exist yet. Waiting for it to be created..." -ForegroundColor Yellow
    # Wait for the file to be created
    while (-not (Test-Path $todayLog)) {
        Start-Sleep -Seconds 1
    }
    Write-Host "Log file created! Now monitoring..." -ForegroundColor Green
    Get-Content $todayLog -Wait
}

