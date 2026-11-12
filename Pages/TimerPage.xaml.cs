namespace TurboClock.Pages;

public partial class TimerPage : ContentPage
{
    private TimeSpan _remainingTime;
    private bool _isRunning;

    public TimerPage()
    {
        InitializeComponent();

        _remainingTime = TimeSpan.Zero;

        UpdateDisplay();

        Dispatcher.StartTimer(
            TimeSpan.FromMilliseconds(100),
            UpdateTimer);
    }

    private bool UpdateTimer()
    {
        if (Handler == null)
        {
            return false;
        }

        if (_isRunning)
        {
            _remainingTime -= TimeSpan.FromMilliseconds(100);

            if (_remainingTime <= TimeSpan.Zero)
            {
                _remainingTime = TimeSpan.Zero;
                _isRunning = false;
            }

            UpdateDisplay();
        }

        return true;
    }

    private void OnStartClicked(
        object sender,
        EventArgs e)
    {
        if (!_isRunning && _remainingTime == TimeSpan.Zero)
        {
            _remainingTime = ReadInputTime();
        }

        if (_remainingTime > TimeSpan.Zero)
        {
            _isRunning = true;
        }
    }

    private void OnPauseClicked(
        object sender,
        EventArgs e)
    {
        _isRunning = false;
    }

    private void OnResetClicked(
        object sender,
        EventArgs e)
    {
        _isRunning = false;
        _remainingTime = ReadInputTime();

        UpdateDisplay();
    }

    private TimeSpan ReadInputTime()
    {
        var hours = ParseValue(HoursEntry.Text);
        var minutes = ParseValue(MinutesEntry.Text);
        var seconds = ParseValue(SecondsEntry.Text);

        minutes = Math.Clamp(minutes, 0, 59);
        seconds = Math.Clamp(seconds, 0, 59);

        return new TimeSpan(
            hours,
            minutes,
            seconds);
    }

    private int ParseValue(string? value)
    {
        if (int.TryParse(value, out var result))
        {
            return Math.Max(result, 0);
        }

        return 0;
    }

    private void UpdateDisplay()
    {
        TimerClock.Time = _remainingTime;
    }
}