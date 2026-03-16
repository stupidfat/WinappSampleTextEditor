$menuKey = "HKCU:\Software\Classes\SystemFileAssociations\.txt\shell\WinAppSampleTextEditor"

if (Test-Path $menuKey) {
    Remove-Item -Path $menuKey -Recurse -Force
    Write-Output "Removed .txt context menu entry."
}
else {
    Write-Output "Context menu entry was not present."
}
