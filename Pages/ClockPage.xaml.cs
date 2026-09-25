using TurboClock.Controls;

namespace TurboClock.Pages;

public partial class ClockPage : ContentPage
{
    public ClockPage()
    {
        InitializeComponent();

        UpdateClock();

        Dispatcher.StartTimer(
            TimeSpan.FromMilliseconds(250),
            UpdateClock);
    }

    private bool UpdateClock()
    {
        if (Handler == null)
        {
            return false;
        }

        Clock.DateTime = DateTime.Now;

        return true;
    }
}