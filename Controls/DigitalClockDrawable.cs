//          0
//       -------
//     5         1
//     |         |
//       -------
//          6
//     |         |
//     4         2
//       -------
//          3

namespace TurboClock.Controls
{
    public class DigitalClockDrawable : IDrawable
    {
        private static readonly bool[][] _possibleSegments =
        {         //  0     1     2     3     4     5     6
            new[] { true, true, true, true, true, true, false },    //0
            new[] { false, true, true, false, false, false, false },//1
            new[] { true, true, false, true, true, false, true },   //2
            new[] { true, true, true, true, false, false, true },   //3
            new[] { false, true, true, false, false, true, true },  //4
            new[] { true, false, true, true, false, true, true },   //5
            new[] { true, false, true, true, true, true, true },    //6
            new[] { true, true, true, false, false, false, false }, //7
            new[] { true, true, true, true, true, true, true },     //8
            new[] { true, true, true, true, false, true, true }     //9
        };

        public TimeSpan Time { get; set; }

        public bool ShowSeconds { get; set; }

        public Color SegmentColor { get; set; } = Colors.White;

        public Color BackgroundColor { get; set; } = Colors.Black;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = BackgroundColor;
            canvas.FillRectangle(dirtyRect);

            var time = Time;
            var hours = time.Hours % 24;
            var minutes = time.Minutes;
            var seconds = time.Seconds;
            var text = ShowSeconds ? $"{hours:00}{minutes:00}{seconds:00}" : $"{hours:00}{minutes:00}";

            var digitCount = text.Length;
            var colonCount = ShowSeconds ? 2 : 1;
            
            var width = dirtyRect.Width;
            var height = dirtyRect.Height;
            var horizontalPadding = width * 0.05f;
            
            var spacing = width * 0.025f;
            var colonWidth = width * 0.035f;

            var totalSpacing = spacing * (digitCount + colonCount - 1);

            var totalColonWidth = colonWidth * colonCount;
            var availableWidth = width - (horizontalPadding * 2) - totalSpacing - totalColonWidth;

            var digitWidth = availableWidth / digitCount;
            var digitHeight = height * 0.75f;
            var digitTop = (height - digitHeight) / 2f;

            var x = horizontalPadding;

            for (var i = 0; i < text.Length; i++)
            {
                var digit = text[i] - '0';
                DrawDigit(canvas, digit, new RectF(x, digitTop, digitWidth, digitHeight));

                x += digitWidth + spacing;

                var showColon = ShowSeconds ? i == 1 || i == 3 : i == 1;

                if (!showColon) continue;
                DrawColon(canvas, x, digitTop, colonWidth, digitHeight);

                x += colonWidth + spacing;
            }
        }

        private void DrawDigit(ICanvas canvas, int digit, RectF bounds)
        {
            var segmentThickness = Math.Min(bounds.Width, bounds.Height) * 0.12f;

            canvas.StrokeColor = SegmentColor;
            canvas.StrokeSize = segmentThickness;
            canvas.StrokeLineCap = LineCap.Round;

            var horizontalInset = bounds.Width * 0.12f;
            var verticalInset = bounds.Height * 0.08f;
            
            var left = bounds.Left + horizontalInset;
            var right = bounds.Right - horizontalInset;
            var top = bounds.Top + verticalInset;
            var bottom = bounds.Bottom - verticalInset;
            var middle = bounds.Center.Y;
            
            var topLeft = new PointF(left, top);
            var topRight = new PointF(right, top);
            
            var middleLeft = new PointF(left, middle);
            var middleRight = new PointF(right, middle);
            
            var bottomLeft = new PointF(left, bottom);
            var bottomRight = new PointF(right, bottom);

            var segments = _possibleSegments[digit];

            if (segments[0])
            {
                canvas.DrawLine(topLeft, topRight);
            }

            if (segments[1])
            {
                canvas.DrawLine(topRight, middleRight);
            }

            if (segments[2])
            {
                canvas.DrawLine(middleRight, bottomRight);
            }

            if (segments[3])
            {
                canvas.DrawLine(bottomLeft, bottomRight);
            }

            if (segments[4])
            {
                canvas.DrawLine(middleLeft, bottomLeft);
            }

            if (segments[5])
            {
                canvas.DrawLine(topLeft, middleLeft);
            }

            if (segments[6])
            {
                canvas.DrawLine(middleLeft, middleRight);
            }
        }

        private void DrawColon(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = SegmentColor;

            var radius = width * 0.35f;

            var centerX = x + width / 2f;

            var topY = y + height * 0.35f;

            var bottomY = y + height * 0.65f;

            canvas.FillCircle(centerX, topY, radius);

            canvas.FillCircle(centerX, bottomY, radius);
        }
    }
}