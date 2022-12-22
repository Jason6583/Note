using System;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;

namespace Note.Models
{
    /// <summary>
    /// 心形
    /// </summary>
    public class HeartStroke : ShapeStroke
    {
        protected override Path GetShape()
        {
            double centerX = Convert.ToDouble(ShapeDatas[1]);
            double centerY = Convert.ToDouble(ShapeDatas[2]);
            double degree = Convert.ToDouble(ShapeDatas[3]);

            double leftCircleX = Convert.ToDouble(ShapeDatas[4]);
            double leftCircleY = Convert.ToDouble(ShapeDatas[5]);
            double leftCircleRadius = Convert.ToDouble(ShapeDatas[6]);
            double leftStartAngle = Convert.ToDouble(ShapeDatas[7]);
            double leftEndAngle = Convert.ToDouble(ShapeDatas[8]);

            double rightCircleX = Convert.ToDouble(ShapeDatas[9]);
            double rightCircleY = Convert.ToDouble(ShapeDatas[10]);
            double rightCircleRadius = Convert.ToDouble(ShapeDatas[11]);
            double rightStartAngle = Convert.ToDouble(ShapeDatas[12]);
            double rightEndAngle = Convert.ToDouble(ShapeDatas[13]);

            List<Point> points = new List<Point>();
            for (int i = 14; i < ShapeDatas.Length - 1; i++)
            {
                points.Add(new Point(Convert.ToDouble(ShapeDatas[i]), Convert.ToDouble(ShapeDatas[++i])));
            }
            var leftPathFigure = new PathFigure();
            leftPathFigure.StartPoint = new Point(leftCircleX + leftCircleRadius * Math.Cos(leftStartAngle * Math.PI / 180), leftCircleY + leftCircleRadius * Math.Sin(leftStartAngle * Math.PI / 180));
            leftPathFigure.Segments.Add(new ArcSegment()
            {
                SweepDirection = SweepDirection.Clockwise,
                Size = new Size(leftCircleRadius, leftCircleRadius),
                Point = new Point(leftCircleX + leftCircleRadius * Math.Cos(leftEndAngle * Math.PI / 180), leftCircleY + leftCircleRadius * Math.Sin(leftEndAngle * Math.PI / 180)),
            });

            var rightPathFigure = new PathFigure();
            rightPathFigure.StartPoint = new Point(rightCircleX + rightCircleRadius * Math.Cos(rightStartAngle * Math.PI / 180), rightCircleY + rightCircleRadius * Math.Sin(rightStartAngle * Math.PI / 180));
            rightPathFigure.Segments.Add(new ArcSegment()
            {
                SweepDirection = SweepDirection.Clockwise,
                Size = new Size(rightCircleRadius, rightCircleRadius),
                Point = new Point(rightCircleX + rightCircleRadius * Math.Cos(rightEndAngle * Math.PI / 180), rightCircleY + rightCircleRadius * Math.Sin(rightEndAngle * Math.PI / 180)),
            });

            var lineFigure = new PathFigure();
            lineFigure.StartPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                lineFigure.Segments.Add(new LineSegment() { Point = points[i] });
            }
            //lineFigure.IsClosed = true;
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(leftPathFigure);
            pathGeometry.Figures.Add(rightPathFigure);
            pathGeometry.Figures.Add(lineFigure);
            this.Bounds = pathGeometry.Bounds;
            var translateTransform = new TranslateTransform();
            translateTransform.X = -this.Bounds.Left;
            translateTransform.Y = -this.Bounds.Top;
            pathGeometry.Transform = translateTransform;
            var pathData = pathGeometry;
            _shapePath = new Path();
            _shapePath.Data = pathData;
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
