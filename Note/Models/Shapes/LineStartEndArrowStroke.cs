using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;

namespace Note.Models
{
    /// <summary>
    /// 直线两端带箭头
    /// </summary>
    public class LineStartEndArrowStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            List<Point> points = GetPoints();

            var startArrowPath = GetArrow(points[1], points[0]);
            var endArrowPath = GetArrow(points[0], points[1]);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                pathFigure.Segments.Add(new LineSegment() { Point = points[i] });
            }
            pathFigure.IsClosed = false;

            _shapePath = new Path();
            var _pathGeometry = new PathGeometry();
            _pathGeometry.Figures.Add(startArrowPath);
            _pathGeometry.Figures.Add(pathFigure);
            _pathGeometry.Figures.Add(endArrowPath);
            this.Bounds = _pathGeometry.Bounds;
            _shapePath.Stroke = this.PathBrush;
            var translateTransform = new TranslateTransform();
            translateTransform.X = -this.Bounds.Left;
            translateTransform.Y = -this.Bounds.Top;
            _pathGeometry.Transform = translateTransform;
            _shapePath.Data = _pathGeometry;
            _shapePath.StrokeThickness = this.StrokeThickness;
            return _shapePath;
        }
    }
}
