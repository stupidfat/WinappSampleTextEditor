# TXT Context Menu Report

- Date: 2026-03-17
- Task: Push the initial WinUI sample and add Windows `.txt` right-click integration.
- Status: Completed

## Git

- Committed the initial project and pushed branch `codex/dev` to `origin`

## App changes

- Added launch-argument support so the app can open a file passed in from Explorer
- Refactored open-file logic so both the toolbar action and startup argument use the same file-loading path
- Added error handling for invalid or unavailable file paths

## Windows shell integration

- Added `scripts/register-txt-context-menu.ps1`
- Added `scripts/unregister-txt-context-menu.ps1`
- The registration script creates a current-user Explorer context menu entry for `.txt` files
- The context menu launches the built app with the selected file path as an argument

## Limitation

- This repository now contains the app-side support and the registration scripts
- Actual Explorer integration still requires a built executable path when running the registration script
