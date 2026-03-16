using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace WinAppSampleTextEditor;

public sealed partial class MainWindow : Window
{
    private string? _currentFilePath;
    private bool _hasUnsavedChanges;
    private bool _suppressDirtyTracking;

    public MainWindow()
    {
        InitializeComponent();
        UpdateWordWrap();
        UpdateWindowTitle();
        UpdateStatus();
    }

    private async void NewFile_Click(object sender, RoutedEventArgs e)
    {
        if (!await ConfirmLoseUnsavedChangesAsync())
        {
            return;
        }

        _suppressDirtyTracking = true;
        Editor.Text = string.Empty;
        _suppressDirtyTracking = false;

        _currentFilePath = null;
        _hasUnsavedChanges = false;
        UpdateWindowTitle();
        UpdateStatus();
    }

    private async void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        if (!await ConfirmLoseUnsavedChangesAsync())
        {
            return;
        }

        var picker = new FileOpenPicker();
        picker.FileTypeFilter.Add(".txt");
        picker.FileTypeFilter.Add(".md");
        picker.FileTypeFilter.Add(".log");
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));

        var file = await picker.PickSingleFileAsync();
        if (file is null)
        {
            return;
        }

        _suppressDirtyTracking = true;
        Editor.Text = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
        _suppressDirtyTracking = false;

        _currentFilePath = file.Path;
        _hasUnsavedChanges = false;
        UpdateWindowTitle();
        UpdateStatus();
    }

    private async void SaveFile_Click(object sender, RoutedEventArgs e)
    {
        await SaveAsync(saveAs: string.IsNullOrWhiteSpace(_currentFilePath));
    }

    private async void SaveAsFile_Click(object sender, RoutedEventArgs e)
    {
        await SaveAsync(saveAs: true);
    }

    private void Editor_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_suppressDirtyTracking)
        {
            return;
        }

        _hasUnsavedChanges = true;
        UpdateWindowTitle();
        UpdateStatus();
    }

    private void Editor_SelectionChanged(object sender, RoutedEventArgs e)
    {
        UpdateStatus();
    }

    private void WordWrapToggle_Changed(object sender, RoutedEventArgs e)
    {
        UpdateWordWrap();
    }

    private async Task SaveAsync(bool saveAs)
    {
        StorageFile? file = null;

        if (!saveAs && !string.IsNullOrWhiteSpace(_currentFilePath))
        {
            file = await StorageFile.GetFileFromPathAsync(_currentFilePath);
        }

        if (file is null)
        {
            var picker = new FileSavePicker();
            picker.FileTypeChoices.Add("Text Document", new List<string> { ".txt" });
            picker.FileTypeChoices.Add("Markdown Document", new List<string> { ".md" });
            picker.SuggestedFileName = "notes";
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            InitializeWithWindow.Initialize(picker, WindowNative.GetWindowHandle(this));

            file = await picker.PickSaveFileAsync();
        }

        if (file is null)
        {
            return;
        }

        await FileIO.WriteTextAsync(file, Editor.Text, Windows.Storage.Streams.UnicodeEncoding.Utf8);

        _currentFilePath = file.Path;
        _hasUnsavedChanges = false;
        UpdateWindowTitle();
        UpdateStatus();
    }

    private async Task<bool> ConfirmLoseUnsavedChangesAsync()
    {
        if (!_hasUnsavedChanges)
        {
            return true;
        }

        var dialog = new ContentDialog
        {
            Title = "Discard unsaved changes?",
            Content = "You have unsaved changes in the current document.",
            PrimaryButtonText = "Discard",
            CloseButtonText = "Cancel",
            XamlRoot = Content.XamlRoot
        };

        return await dialog.ShowAsync() == ContentDialogResult.Primary;
    }

    private void UpdateWordWrap()
    {
        Editor.TextWrapping = WordWrapToggle.IsChecked == true ? TextWrapping.Wrap : TextWrapping.NoWrap;
    }

    private void UpdateWindowTitle()
    {
        var fileName = string.IsNullOrWhiteSpace(_currentFilePath)
            ? "Untitled"
            : Path.GetFileName(_currentFilePath);
        var dirtyMarker = _hasUnsavedChanges ? "*" : string.Empty;
        Title = $"{dirtyMarker}{fileName} - Sample Text Editor";
    }

    private void UpdateStatus()
    {
        var content = Editor.Text ?? string.Empty;
        var lines = content.Length == 0 ? 1 : content.Split(Environment.NewLine).Length;
        var characters = content.Length;

        DocumentStateText.Text = string.IsNullOrWhiteSpace(_currentFilePath)
            ? $"Unsaved document | {lines} lines | {characters} chars"
            : $"{_currentFilePath} | {lines} lines | {characters} chars";

        CursorStateText.Text = BuildCursorSummary(content, Editor.SelectionStart);
    }

    private static string BuildCursorSummary(string content, int selectionStart)
    {
        var safePosition = Math.Clamp(selectionStart, 0, content.Length);
        var line = 1;
        var column = 1;

        for (var i = 0; i < safePosition; i++)
        {
            if (content[i] == '\n')
            {
                line++;
                column = 1;
            }
            else
            {
                column++;
            }
        }

        return $"Ln {line}, Col {column}";
    }
}
