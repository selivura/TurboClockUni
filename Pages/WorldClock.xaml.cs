using TurboClock.Controls;
using System.Text.Json;

namespace TurboClock.Pages;

public partial class WorldClockPage : ContentPage
{
    private const string SavedTimezonesKey = "WorldClock.Timezones";

    private readonly List<TimeZoneInfo> _availableTimezones;
    private readonly List<TimeZoneInfo> _currentTimezones = [];
    private readonly List<TimeZoneClock> _clocks = [];

    public WorldClockPage()
    {
        // Сделать дропдаун, который будет добавлять тайм зону
        // Сделать кнопку которая будет убирать таймзону
        // Сделать чтобы тайм зоны сохранялись между запусками приложения

        InitializeComponent();

        _availableTimezones = TimeZoneInfo.GetSystemTimeZones().ToList();
        TimezonePicker.ItemsSource = _availableTimezones;

        LoadSavedTimezones();
        UpdateClocks();

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(250), UpdateClocks);
    }

    private void OnAddTimezoneClicked(object? sender, EventArgs e)
    {
        if (TimezonePicker.SelectedItem is not TimeZoneInfo timezone)
        {
            return;
        }

        if (_currentTimezones.Any(existing => existing.Id == timezone.Id))
        {
            return;
        }

        _currentTimezones.Add(timezone);
        AddClock(timezone);
        SaveTimezones();

        TimezonePicker.SelectedItem = null;
    }

    private void OnRemoveTimezoneRequested(object? sender, EventArgs e)
    {
        if (sender is not TimeZoneClock clock)
        {
            return;
        }

        clock.RemoveRequested -= OnRemoveTimezoneRequested;
        _clocks.Remove(clock);
        _currentTimezones.RemoveAll(timezone => timezone.Id == clock.Timezone.Id);
        ClocksLayout.Children.Remove(clock);
        SaveTimezones();
    }

    private void AddClock(TimeZoneInfo timezone)
    {
        var clock = new TimeZoneClock
        {
            Timezone = timezone,
            HorizontalOptions = LayoutOptions.Fill
        };

        clock.RemoveRequested += OnRemoveTimezoneRequested;

        _clocks.Add(clock);
        ClocksLayout.Children.Add(clock);
    }

    private void LoadSavedTimezones()
    {
        var savedValue = Preferences.Get(SavedTimezonesKey, string.Empty);
        if (string.IsNullOrWhiteSpace(savedValue))
        {
            return;
        }

        var savedIds = JsonSerializer.Deserialize<List<string>>(savedValue);
        if (savedIds == null)
        {
            return;
        }

        foreach (var id in savedIds)
        {
            var timezone = _availableTimezones.FirstOrDefault(timezone => timezone.Id == id);
            if (timezone == null)
            {
                continue;
            }

            _currentTimezones.Add(timezone);
            AddClock(timezone);
        }
    }

    private void SaveTimezones()
    {
        var timezoneIds = _currentTimezones
            .Select(timezone => timezone.Id)
            .ToList();

        Preferences.Set(SavedTimezonesKey, JsonSerializer.Serialize(timezoneIds));
    }

    private bool UpdateClocks()
    {
        if (Handler == null)
        {
            return false;
        }

        var utcNow = DateTimeOffset.UtcNow;

        foreach (var clock in _clocks)
        {
            clock.DateTime = TimeZoneInfo.ConvertTime(utcNow, clock.Timezone).DateTime;
        }

        return true;
    }
}