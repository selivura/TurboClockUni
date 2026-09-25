using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace TurboClock.Controls
{
    public class ClockWithDate : ContentView
    {
        private readonly DigitalClock _clock;
        private readonly Label _dateLabel;

        public static readonly BindableProperty DateTimeProperty =
            BindableProperty.Create(
                nameof(DateTime),
                typeof(DateTime),
                typeof(ClockWithDate),
                System.DateTime.Now,
                propertyChanged: OnDateTimeChanged);

        public static readonly BindableProperty ShowSecondsProperty =
            BindableProperty.Create(
                nameof(ShowSeconds),
                typeof(bool),
                typeof(ClockWithDate),
                true,
                propertyChanged: OnShowSecondsChanged);

        public static readonly BindableProperty SegmentColorProperty =
            BindableProperty.Create(
                nameof(SegmentColor),
                typeof(Color),
                typeof(ClockWithDate),
                Colors.White,
                propertyChanged: OnSegmentColorChanged);

        public static readonly BindableProperty ClockBackgroundColorProperty =
            BindableProperty.Create(
                nameof(ClockBackgroundColor),
                typeof(Color),
                typeof(ClockWithDate),
                Colors.Black,
                propertyChanged: OnClockBackgroundColorChanged);

        public static readonly BindableProperty DateColorProperty =
            BindableProperty.Create(
                nameof(DateColor),
                typeof(Color),
                typeof(ClockWithDate),
                Colors.White,
                propertyChanged: OnDateColorChanged);

        public static readonly BindableProperty DateFontSizeProperty =
            BindableProperty.Create(
                nameof(DateFontSize),
                typeof(double),
                typeof(ClockWithDate),
                18.0,
                propertyChanged: OnDateFontSizeChanged);

        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set => SetValue(DateTimeProperty, value);
        }

        public bool ShowSeconds
        {
            get => (bool)GetValue(ShowSecondsProperty);
            set => SetValue(ShowSecondsProperty, value);
        }

        public Color SegmentColor
        {
            get => (Color)GetValue(SegmentColorProperty);
            set => SetValue(SegmentColorProperty, value);
        }

        public Color ClockBackgroundColor
        {
            get => (Color)GetValue(ClockBackgroundColorProperty);
            set => SetValue(ClockBackgroundColorProperty, value);
        }

        public Color DateColor
        {
            get => (Color)GetValue(DateColorProperty);
            set => SetValue(DateColorProperty, value);
        }

        public double DateFontSize
        {
            get => (double)GetValue(DateFontSizeProperty);
            set => SetValue(DateFontSizeProperty, value);
        }

        public ClockWithDate()
        {
            _clock = new DigitalClock
            {
                HeightRequest = 150,
                HorizontalOptions = LayoutOptions.Fill,
                ShowSeconds = ShowSeconds,
                SegmentColor = SegmentColor,
                ClockBackgroundColor = ClockBackgroundColor
            };

            _dateLabel = new Label
            {
                TextColor = DateColor,
                FontSize = DateFontSize,
                HorizontalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Fill
            };

            var layout = new VerticalStackLayout
            {
                Spacing = 8,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center
            };

            layout.Children.Add(_clock);
            layout.Children.Add(_dateLabel);

            Content = layout;

            UpdateDisplay();
        }

        private static void OnDateTimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ClockWithDate)bindable;
            control.UpdateDisplay();
        }

        private static void OnShowSecondsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ClockWithDate)bindable;
            control._clock.ShowSeconds = (bool)newValue;
        }

        private static void OnSegmentColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ClockWithDate)bindable;
            control._clock.SegmentColor = (Color)newValue;
        }

        private static void OnClockBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ClockWithDate)bindable;
            control._clock.ClockBackgroundColor = (Color)newValue;
        }

        private static void OnDateColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ClockWithDate)bindable;
            control._dateLabel.TextColor = (Color)newValue;
        }

        private static void OnDateFontSizeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ClockWithDate)bindable;
            control._dateLabel.FontSize = (double)newValue;
        }

        private void UpdateDisplay()
        {
            _clock.Time = DateTime.TimeOfDay;
            _dateLabel.Text = DateTime.ToString("dddd, dd MMMM yyyy");
        }
    }
}