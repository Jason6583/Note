using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;

namespace Note.Models
{
    /// <summary>
    /// 普通曲线
    /// </summary>
    public class CurveStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            List<Point> points = GetPoints();
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
            this.Bounds = _pathGeometry.Bounds;
            var translateTransform = new TranslateTransform();
            translateTransform.X = -this.Bounds.Left;
            translateTransform.Y = -this.Bounds.Top;
            _pathGeometry.Transform = translateTransform;
            _shapePath.Data =_pathGeometry;
            _shapePath.Stroke = this.PathBrush;
            _shapePath.StrokeThickness = this.StrokeThickness;
            return _shapePath;
        }
    }
}
