# WinAppSampleTextEditor 專案分析

## 專案概述
`WinAppSampleTextEditor` 是一個以 **WinUI 3** 與 **.NET 8** 建立的 Windows 桌面文字編輯器範例。

它的定位是「簡潔但功能完整的文字編輯工具」，示範了 WinUI 桌面應用在檔案 I/O、使用者互動提示、狀態列資訊更新，以及從檔案總管直接開啟檔案等常見需求的實作方式。

## 技術與架構
- **應用型態**：WinUI 3 Windows 桌面應用（`WinExe`）
- **主要框架**：`net8.0-windows10.0.19041.0`
- **核心套件**：`Microsoft.WindowsAppSDK`
- **UI 組成**：
  - 上方 `CommandBar`（New/Open/Save/Save As/Word Wrap）
  - 中央 `TextBox` 作為編輯區
  - 下方狀態列顯示文件資訊與游標位置

## 主要功能
1. **新建文件（New）**
   - 清空編輯內容，重設目前檔案路徑與未儲存狀態。

2. **開啟檔案（Open）**
   - 使用 `FileOpenPicker` 選取檔案，支援 `.txt`、`.md`、`.log`。
   - 若目前內容有未儲存變更，會先提示是否捨棄。

3. **儲存（Save）與另存新檔（Save As）**
   - 已有檔案路徑時可直接覆寫儲存。
   - 無路徑或使用 Save As 時，透過 `FileSavePicker` 儲存。
   - 儲存格式預設支援 `.txt` 與 `.md`。

4. **未儲存變更追蹤（Dirty Tracking）**
   - 使用 `_hasUnsavedChanges` 追蹤內容是否修改。
   - 視窗標題會用 `*` 標記未儲存狀態。

5. **Word Wrap 切換**
   - 透過 ToggleButton 控制 `TextBox.TextWrapping`（換行/不換行）。

6. **狀態列資訊更新**
   - 顯示當前文件路徑（或 Unsaved document）、行數、字元數。
   - 顯示游標行列位置（Ln, Col）。

7. **啟動參數開檔**
   - 程式啟動時可讀取命令列檔案路徑，若存在則自動開啟。
   - 可配合檔案總管右鍵選單達到「以本程式開啟 .txt」。

## 使用者流程亮點
- 在切換檔案（New/Open）前，會透過對話框確認是否捨棄未儲存內容，降低資料遺失風險。
- 開檔失敗時有錯誤對話框提示，提供基本例外處理回饋。
- 狀態列持續反映編輯內容與游標資訊，提升可用性。

## 相關腳本與維運輔助
專案 `scripts/` 提供 PowerShell 腳本，可在 Windows 註冊/移除 `.txt` 右鍵選單項目：
- `register-txt-context-menu.ps1`
- `unregister-txt-context-menu.ps1`

這讓已建置完成的執行檔可被整合到檔案總管操作流程。

## 適合用途
- WinUI 3 初學者學習桌面應用基本結構。
- 示範文件編輯器常見功能（開檔、儲存、狀態列、未儲存提示）。
- 作為後續擴充（例如語法高亮、搜尋取代、自動儲存、分頁編輯）的起點。

## 總結
此專案的核心價值在於：
- 以小而完整的範例展示 WinUI 3 桌面編輯器的實務要點。
- 在不複雜的程式碼下，涵蓋了「檔案處理 + UI 互動 + 使用者保護機制」三個關鍵面向。
- 具備作為教學與實作模板的可讀性與可擴充性。
