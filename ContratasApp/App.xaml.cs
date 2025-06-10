namespace ContratasApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        // Force the theme to Light or Dark
        if (Current != null)
            Current.UserAppTheme = AppTheme.Light;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}