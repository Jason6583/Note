using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Note.InkCanvasEx.Enums;

namespace Note.Models
{
    public class StrokeRect
    {
        public int WordId { get; set; }
        public string Word { get; set; }
        public RectType RectType { get; set; }
        public Rect DrawRect { get; set; }
        public bool IsFinded { get; set; }
        private Path _rectPath;
        public Path RectPath
        {
            get
            {
                if (_rectPath == null)
                {
                    _rectPath = PrepareDrawPath();
                }
                return _rectPath;
            }
        }
        public Brush RectBrush
        {
            get
            {
                return PrepareDrawBrush();
            }
        }
        public Rect ActualRect
        {
            get
            {
                return PrepareDrawRect();
            }
        }
        private Rect PrepareDrawRect()
        {
            switch (RectType)
            {
                case RectType.Split:
                case RectType.Multi:
                    break;
                case RectType.Result:
                    return new Rect(DrawRect.X + 2, DrawRect.Y + 2, DrawRect.Width, DrawRect.Height);
                case RectType.Row:
                    return new Rect(DrawRect.X - 2, DrawRect.Y - 2, DrawRect.Width, DrawRect.Height);
                default:
                    break;
            }
            return DrawRect;
        }
        private Brush PrepareDrawBrush()
        {
            switch (RectType)
            {
                case RectType.Split:
                    return new SolidColorBrush(Colors.OrangeRed);
                case RectType.Result:
                    return new SolidColorBrush(Colors.LightGreen);
                case RectType.Row:
                    return new SolidColorBrush(Colors.Blue);
                case RectType.Multi:
                    return new SolidColorBrush(Colors.Black);
                default:
                    break;
            }
            return new SolidColorBrush(Colors.LightGray);
        }
        private Path PrepareDrawPath()
        {
            if (DrawRect.IsEmpty) return null;
            if (_rectPath == null)
            {
                _rectPath = new Path();

                PathGeometry g = new PathGeometry();

                PathFigure f = new PathFigure();
                f.IsFilled = true;
                f.IsClosed = true;
                f.StartPoint = new Point(this.ActualRect.Left, this.ActualRect.Top);
                LineSegment s1 = new LineSegment();
                s1.Point = new Point(this.ActualRect.Right, this.ActualRect.Top);
                LineSegment s2 = new LineSegment();
                s2.Point = new Point(this.ActualRect.Right, this.ActualRect.Bottom);
                LineSegment s3 = new LineSegment();
                s3.Point = new Point(this.ActualRect.Left, this.ActualRect.Bottom);
                LineSegment s4 = new LineSegment();
                s4.Point = new Point(this.ActualRect.Left, this.ActualRect.Top);
                f.Segments.Add(s1);
                f.Segments.Add(s2);
                f.Segments.Add(s3);
                f.Segments.Add(s4);
                g.Figures.Add(f);
                _rectPath.Data = g;
                _rectPath.Stroke = this.RectBrush;
                _rectPath.StrokeThickness = 1;
            }
            return _rectPath;
        }
        public void ResetPath()
        {
            this.IsFinded = false;
            RectPath.Stroke = this.RectBrush;
        }
        public void HighlightFindedPath()
        {
            RectPath.Stroke =new SolidColorBrush(Colors.Black);
            this.IsFinded = true;
        }
    }
}
