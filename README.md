# WinAppSampleTextEditor

This repository contains a WinUI 3 sample text editor with basic editing, open/save, save as, unsaved-change prompts, word wrap toggle, and a simple status bar.

## Build prerequisites

- .NET 8 SDK
- Windows App SDK workload or a recent Visual Studio installation with WinUI 3 support

## Project

- Solution: `WinAppSampleTextEditor.sln`
- App project: `WinAppSampleTextEditor/WinAppSampleTextEditor.csproj`

## Open files from Explorer

The app supports a file path as a launch argument. That means a `.txt` file can be opened directly when Explorer launches the app with the selected file path.

To add a right-click menu item for `.txt` files after you have a built executable:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\register-txt-context-menu.ps1 -ExecutablePath "C:\path\to\WinAppSampleTextEditor.exe"
```

This creates a current-user context menu entry named `Open with Sample Text Editor`.

To remove it later:

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\unregister-txt-context-menu.ps1
```
