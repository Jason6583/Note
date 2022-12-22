using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;

namespace Note.Models
{
    /// <summary>
    /// 直线起始点带箭头
    /// </summary>
    public class LineStartArrowStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            List<Point> points = GetPoints();

            var arrowPath = GetArrow(points[1], points[0]);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                pathFigure.Segments.Add(new LineSegment() { Point = points[i] });
            }
            pathFigure.IsClosed = false;

            _shapePath = new Path();
            var _pathGeometry = new PathGeometry();
            _pathGeometry.Figures.Add(arrowPath);
            _pathGeometry.Figures.Add(pathFigure);
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
