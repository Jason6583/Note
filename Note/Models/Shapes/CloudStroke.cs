using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Note.Models
{
    /// <summary>
    /// 云朵
    /// </summary>
    public class CloudStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            double centerX = Convert.ToDouble(ShapeDatas[1]);
            double centerY = Convert.ToDouble(ShapeDatas[2]);
            double degree = Convert.ToDouble(ShapeDatas[3]);

            var _pathGeometry = new PathGeometry();
            for (int i = 4; i < ShapeDatas.Length; i += 5)
            {
                double cX = Convert.ToDouble(ShapeDatas[i]);
                double cY = Convert.ToDouble(ShapeDatas[i + 1]);
                double radius = Convert.ToDouble(ShapeDatas[i + 2]);
                double startAngle = Convert.ToDouble(ShapeDatas[i + 3]);
                double endAngle = Convert.ToDouble(ShapeDatas[i + 4]);

                PathFigure pathFigure = new PathFigure();
                pathFigure.StartPoint = new Point(cX + radius * Math.Cos(startAngle * Math.PI / 180), cY + radius * Math.Sin(startAngle * Math.PI / 180));
                pathFigure.Segments.Add(new ArcSegment()
                {
                    SweepDirection = SweepDirection.Clockwise,
                    Size = new Size(radius, radius),
                    Point = new Point(cX + radius * Math.Cos(endAngle * Math.PI / 180), cY + radius * Math.Sin(endAngle * Math.PI / 180)),
                });
                _pathGeometry.Figures.Add(pathFigure);
            }
            this.Bounds = _pathGeometry.Bounds;
            var translateTransform = new TranslateTransform();
            translateTransform.X = -this.Bounds.Left;
            translateTransform.Y = -this.Bounds.Top;
            _pathGeometry.Transform = translateTransform;
            _shapePath = new Path();
            _shapePath.Data = _pathGeometry;
            _shapePath.Stroke = this.PathBrush;
            _shapePath.StrokeThickness = this.StrokeThickness;
            _shapePath.RenderTransform = new RotateTransform
            {
                CenterX = centerX - this.Bounds.Left,
                CenterY = centerY - this.Bounds.Top,
                Angle = degree,
            };
            return _shapePath;
        }
    }
}
