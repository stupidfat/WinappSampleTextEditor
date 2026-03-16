using Microsoft.UI.Xaml;

namespace WinAppSampleTextEditor;

public partial class App : Application
{
    private MainWindow? _window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();

        var launchFilePath = Environment.GetCommandLineArgs()
            .Skip(1)
            .FirstOrDefault(static path => File.Exists(path));

        if (!string.IsNullOrWhiteSpace(launchFilePath))
        {
            _ = _window.OpenFileFromPathAsync(launchFilePath);
        }
    }
}
