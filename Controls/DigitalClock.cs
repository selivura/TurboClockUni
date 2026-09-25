using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace TurboClock.Controls
{
    public class DigitalClock : ContentView
    {
        private readonly GraphicsView _graphicsView;
        private readonly DigitalClockDrawable _drawable;

        public static readonly BindableProperty TimeProperty = BindableProperty.Create(
                nameof(Time),
                typeof(TimeSpan),
                typeof(DigitalClock),
                TimeSpan.Zero,
                propertyChanged: OnTimeChanged);

        public static readonly BindableProperty ShowSecondsProperty = BindableProperty.Create(
                nameof(ShowSeconds),
                typeof(bool),
                typeof(DigitalClock),
                true,
                propertyChanged: OnShowSecondsChanged);

        public static readonly BindableProperty SegmentColorProperty = BindableProperty.Create(
                nameof(SegmentColor),
                typeof(Color),
                typeof(DigitalClock),
                Colors.White,
                propertyChanged: OnSegmentColorChanged);

        public static readonly BindableProperty ClockBackgroundColorProperty = BindableProperty.Create(
                nameof(ClockBackgroundColor),
                typeof(Color),
                typeof(DigitalClock),
                Colors.Black,
                propertyChanged: OnClockBackgroundColorChanged);

        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
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

        public DigitalClock()
        {
            _drawable = new DigitalClockDrawable
            {
                Time = Time,
                ShowSeconds = ShowSeconds,
                SegmentColor = SegmentColor,
                BackgroundColor = ClockBackgroundColor
            };

            _graphicsView = new GraphicsView
            {
                Drawable = _drawable,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            Content = _graphicsView;
        }

        private static void OnTimeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var clock = (DigitalClock)bindable;

            clock._drawable.Time = (TimeSpan)newValue;
            clock._graphicsView.Invalidate();
        }

        private static void OnShowSecondsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var clock = (DigitalClock)bindable;

            clock._drawable.ShowSeconds = (bool)newValue;
            clock._graphicsView.Invalidate();
        }

        private static void OnSegmentColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var clock = (DigitalClock)bindable;

            clock._drawable.SegmentColor = (Color)newValue;
            clock._graphicsView.Invalidate();
        }

        private static void OnClockBackgroundColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var clock = (DigitalClock)bindable;

            clock._drawable.BackgroundColor = (Color)newValue;
            clock._graphicsView.Invalidate();
        }
    }
}