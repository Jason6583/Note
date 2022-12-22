using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Note.Models
{
    /// <summary>
    /// 抛物线终点带箭头
    /// </summary>
    public class ParabolaEndArrowStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            var points = GetPoints();

            var arrowPath = GetArrow(points[points.Count - 3], points[points.Count - 2]);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = points[0];

            var polyBezierSegment = new PolyBezierSegment();
            for (int i = 1; i < points.Count; i++)
            {
                polyBezierSegment.Points.Add(points[i]);
            }
            pathFigure.Segments.Add(polyBezierSegment);

            pathFigure.IsClosed = false;

            _shapePath = new Path();
            var _pathGeometry = new PathGeometry();
            _pathGeometry.Figures.Add(pathFigure);
            _pathGeometry.Figures.Add(arrowPath);
            this.Bounds = _pathGeometry.Bounds;
            var translateTransform = new TranslateTransform();
            translateTransform.X = -this.Bounds.Left;
            translateTransform.Y = -this.Bounds.Top;
            _pathGeometry.Transform = translateTransform;
            _shapePath.Data = _pathGeometry;
            _shapePath.Stroke = this.PathBrush;
            _shapePath.StrokeThickness = this.StrokeThickness;
            return _shapePath;
        }
    }
}
