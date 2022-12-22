using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Note.Models
{
    /// <summary>
    /// 圆
    /// </summary>
    public class CircleStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            double centerX = Convert.ToDouble(ShapeDatas[1]);
            double centerY = Convert.ToDouble(ShapeDatas[2]);
            double width = Convert.ToDouble(ShapeDatas[3]);
            double height = Convert.ToDouble(ShapeDatas[4]);
            double degree = Convert.ToDouble(ShapeDatas[5]);

            var ellipse = new EllipseGeometry()
            {
                Center = new Point(centerX, centerY),
                RadiusX = width / 2,
                RadiusY = height / 2
            };
            this.Bounds = ellipse.Bounds;
            var translateTransform = new TranslateTransform();
            translateTransform.X = -this.Bounds.Left;
            translateTransform.Y = -this.Bounds.Top;
            ellipse.Transform = translateTransform;
            _shapePath = new Path();
            _shapePath.Data = ellipse;
            _shapePath.Stroke = this.PathBrush;
            _shapePath.StrokeThickness = this.StrokeThickness;
            _shapePath.RenderTransform = new RotateTransform
            {
                CenterX = ellipse.Center.X - this.Bounds.Left,
                CenterY = ellipse.Center.Y - this.Bounds.Top,
                Angle = degree,
            };
            return _shapePath;
        }
    }
}
