using System;
using Windows.UI;
using Windows.Foundation;
using Note.InkCanvasEx.SDK;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Collections.Generic;

namespace Note.Models
{
    /// <summary>
    /// 形状绘制基类
    /// </summary>
    public class ShapeStroke
    {
        /// <summary>
        /// 形状绘制颜色
        /// </summary>
        public Brush PathBrush /*{ get; set; } */= new SolidColorBrush(Colors.Blue);
        /// <summary>
        /// 形状绘制粗细
        /// </summary>
        public double StrokeThickness { get; set; } = 3;
        /// <summary>
        /// 箭头高度
        /// </summary>
        public double ArrowHeight { get; set; } = 20;
        /// <summary>
        /// 箭头底边
        /// </summary>
        public double ArrowBottom { get; set; } = 6;
        /// <summary>
        /// 形状类型
        /// </summary>
        public ShapeType Shape { get; set; }
        /// <summary>
        /// 形状数据
        /// </summary>
        public string[] ShapeDatas { get; set; }
        /// <summary>
        /// 形状绘制路径
        /// </summary>
        protected Path _shapePath;
        public Path ShapePath
        {
            get
            {
                if (_shapePath == null)
                {
                    try
                    {
                        _shapePath = GetShape();
                        //_shapePath.IsHitTestVisible = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                return _shapePath;
            }
        }
        public Rect Bounds { get; set; }   
        /// <summary>
        /// 生成形状路径
        /// </summary>
        /// <returns></returns>
        protected virtual Path GetShape()
        {
            List<Point> points = GetPoints();
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                pathFigure.Segments.Add(new LineSegment() { Point = points[i] });
            }
            pathFigure.IsClosed = false;
            _shapePath = new Path();
            var _pathGeometry = new PathGeometry();
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
        protected virtual List<Point> GetPoints()
        {
            List<Point> points = new List<Point>();
            for (int i = 1; i < ShapeDatas.Length - 1; i++)
            {
                points.Add(new Point(Convert.ToDouble(ShapeDatas[i]), Convert.ToDouble(ShapeDatas[++i])));
            }
            return points;
        }
        protected virtual PathFigure GetArrow(Point from, Point to)
        {
            double distance = Math.Sqrt((to.X - from.X) * (to.X - from.X) + (to.Y - from.Y) * (to.Y - from.Y));
            double dX = to.X - from.X;
            double dY = to.Y - from.Y;
            double pointX = to.X - (ArrowHeight / distance * dX);
            double pointY = to.Y - (ArrowHeight / distance * dY);

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(pointX + (ArrowBottom / distance * dY), pointY - (ArrowBottom / distance * dX));
            pathFigure.Segments.Add(new LineSegment() { Point = to });
            pathFigure.Segments.Add(new LineSegment() { Point = new Point(pointX - (ArrowBottom / distance * dY), pointY + (ArrowBottom / distance * dX)) });
            pathFigure.IsClosed = false;

            return pathFigure;
        }

        public ShapeStroke()
        {
        }
    }
}
