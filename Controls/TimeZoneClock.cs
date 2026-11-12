namespace TurboClock.Controls;

public class TimeZoneClock : ContentView
{
    private readonly Label _timezoneLabel;
    private readonly ClockWithDate _clock;
    public event EventHandler? RemoveRequested;

    public static readonly BindableProperty TimezoneProperty =
        BindableProperty.Create(
            nameof(Timezone),
            typeof(TimeZoneInfo),
            typeof(TimeZoneClock),
            TimeZoneInfo.Local,
            propertyChanged: OnTimezoneChanged);

    public static readonly BindableProperty DateTimeProperty =
        BindableProperty.Create(
            nameof(DateTime),
            typeof(DateTime),
            typeof(TimeZoneClock),
            System.DateTime.Now,
            propertyChanged: OnDateTimeChanged);

    public TimeZoneInfo Timezone
    {
        get => (TimeZoneInfo)GetValue(TimezoneProperty);
        set => SetValue(TimezoneProperty, value);
    }

    public DateTime DateTime
    {
        get => (DateTime)GetValue(DateTimeProperty);
        set => SetValue(DateTimeProperty, value);
    }

    private static void OnTimezoneChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TimeZoneClock)bindable;
        var timezone = (TimeZoneInfo)newValue;
        control._timezoneLabel.Text = timezone.DisplayName;
    }

    private static void OnDateTimeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TimeZoneClock)bindable;
        control._clock.DateTime = (DateTime)newValue;
    }

    public TimeZoneClock()
    {
        _clock = new ClockWithDate()
        {
            WidthRequest = WidthRequest,
            HeightRequest = HeightRequest,
        };
        _timezoneLabel = new Label
        {
            TextColor = _clock.DateColor,
            FontSize = _clock.DateFontSize,
            HorizontalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill
        };

        var layout = new VerticalStackLayout
        {
            Spacing = 4,
            HorizontalOptions = LayoutOptions.Fill
        };
        
        var removeButton = new Button
        {
            Text = "X",
            TextColor = Colors.White,
            BackgroundColor = Colors.DarkRed,
            HorizontalOptions = LayoutOptions.Center
        };

        removeButton.Clicked += (_, _) =>
        {
            RemoveRequested?.Invoke(this, EventArgs.Empty);
        };

        layout.Children.Add(_timezoneLabel);
        layout.Children.Add(_clock);
        layout.Children.Add(removeButton);
        Content = layout;

        _timezoneLabel.Text = Timezone.DisplayName;
        _clock.DateTime = DateTime;
    }
}