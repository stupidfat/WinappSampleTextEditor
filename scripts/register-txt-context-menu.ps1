param(
    [Parameter(Mandatory = $true)]
    [string]$ExecutablePath
)

$resolvedExe = (Resolve-Path $ExecutablePath).Path
if (-not (Test-Path $resolvedExe -PathType Leaf)) {
    throw "Executable not found: $ExecutablePath"
}

$menuKey = "HKCU:\Software\Classes\SystemFileAssociations\.txt\shell\WinAppSampleTextEditor"
$commandKey = Join-Path $menuKey "command"

New-Item -Path $menuKey -Force | Out-Null
Set-Item -Path $menuKey -Value "Open with Sample Text Editor"
New-ItemProperty -Path $menuKey -Name "Icon" -Value $resolvedExe -PropertyType String -Force | Out-Null

New-Item -Path $commandKey -Force | Out-Null
Set-Item -Path $commandKey -Value "`"$resolvedExe`" `"%1`""

Write-Output "Registered .txt context menu for $resolvedExe"
