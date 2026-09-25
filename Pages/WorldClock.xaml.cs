namespace TurboClock.Pages;

public partial class WorldClockPage : ContentPage
{
    private readonly TimeZoneInfo _localTimeZone;
    private readonly TimeZoneInfo _newYorkTimeZone;
    private readonly TimeZoneInfo _tokyoTimeZone;

    public WorldClockPage()
    {
        InitializeComponent();

        _localTimeZone = TimeZoneInfo.Local;
        _newYorkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        _tokyoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tokyo");

        LocalTimeZoneLabel.Text =
            _localTimeZone.DisplayName;

        UpdateClocks();

        Dispatcher.StartTimer(
            TimeSpan.FromMilliseconds(250),
            UpdateClocks);
    }

    private bool UpdateClocks()
    {
        if (Handler == null)
        {
            return false;
        }

        var utcNow = DateTimeOffset.UtcNow;

        var localNow =
            TimeZoneInfo.ConvertTime(
                utcNow,
                _localTimeZone);

        var newYorkNow = TimeZoneInfo.ConvertTime(utcNow, _newYorkTimeZone);

        var tokyoNow = TimeZoneInfo.ConvertTime(utcNow, _tokyoTimeZone);

        LocalClock.DateTime = localNow.DateTime;

        NewYorkClock.DateTime = newYorkNow.DateTime;

        TokyoClock.DateTime = tokyoNow.DateTime;

        return true;
    }
}