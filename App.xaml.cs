using Microsoft.Extensions.DependencyInjection;

namespace TurboClock;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell());

        window.Width = 500;
        window.Height = 850;
        
        return window;
    }
}