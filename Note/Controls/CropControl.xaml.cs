using System;
using System.Linq;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;
using Note.Services.ScreenShot;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Note.Controls
{
    public partial class CropControl : UserControl
    {
        public CropService CropService { get; private set; }
        public Stroke LassoStroke { get; set; }
        private Point startPoint = new Point(0, 0);
        private Point endPoint = new Point(0, 0);
        private bool isMouseDown = false;
        public CropControl()
        {
            InitializeComponent();
            this.inkCanvas.EditingMode = InkCanvasEditingMode.None;
        }
        private void RootGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //CropService.Adorner.RaiseEvent(e);
        }
        private void RootGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //CropService.Adorner.RaiseEvent(e);
        }
        private void RootGrid_Loaded(object sender, RoutedEventArgs e)
        {
            CropService = new CropService(this);
            ClipImage.Clip = new RectangleGeometry(CropService.GetCroppedArea().CroppedRectAbsolute);
            CropService.OnCropAreaChanged += CropService_OnCropAreaChanged;
        }
        private void CropService_OnCropAreaChanged(object sender, EventArgs e)
        {
            ClipImage.Clip = new RectangleGeometry(CropService.GetCroppedArea().CroppedRectAbsolute);
        }
        public void SetImage(BitmapImage bitmapImage, double w, double h)
        {
            this.Width = w;
            this.Height = h;
            SourceImage.Source = bitmapImage;
            ClipImage.Source = bitmapImage;
        }
        private void InkCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = true;
            CropService.HideAdorner();
            startPoint = new Point(0, 0);
            endPoint = new Point(0, 0);
            inkCanvas.Strokes.Remove(LassoStroke);
            LassoStroke = null;
            ClipImage.Clip = new RectangleGeometry(new Rect(0, 0, 0, 0));
            startPoint = e.GetPosition(inkCanvas);
            StylusPointCollection pts = new StylusPointCollection();
            pts.Add(new StylusPoint(startPoint.X, startPoint.Y));
            LassoStroke = new Stroke(pts, new DrawingAttributes()
            {
                Color = Colors.White,
                IgnorePressure = true,
                Width = 2,
                Height = 2,
                StylusTip = StylusTip.Rectangle
            });
            inkCanvas.Strokes.Add(LassoStroke);
        }
        private void InkCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if ((e.LeftButton == MouseButtonState.Pressed || (e.RightButton == MouseButtonState.Pressed)) && isMouseDown)
            {
                if (startPoint == new Point(0, 0))
                {
                    startPoint = e.GetPosition(inkCanvas);
                }
                endPoint = e.GetPosition(inkCanvas);
                if (LassoStroke == null)
                {
                    StylusPointCollection pts = new StylusPointCollection();
                    pts.Add(new StylusPoint(startPoint.X, startPoint.Y));
                    pts.Add(new StylusPoint(endPoint.X, endPoint.Y));
                    LassoStroke = new Stroke(pts, new DrawingAttributes());
                }
                else
                {
                    this.LassoStroke.StylusPoints.Add(new StylusPoint(endPoint.X, endPoint.Y));
                }
            }
        }
        //收集点并裁切图片
        private void InkCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.ClipImageWithLassoStroke();
        }
        private void ClipImageWithLassoStroke()
        {
            if (isMouseDown && LassoStroke != null)
            {
                if (LassoStroke.StylusPoints != null && LassoStroke.StylusPoints.Count > 0)
                {
                    var firstPoint = LassoStroke.StylusPoints.FirstOrDefault();
                    if (firstPoint != null)
                    {
                        LassoStroke.StylusPoints.Add(firstPoint);
                        //处理裁切
                        IEnumerable<Point> stylusPoints =
                            from x in this.LassoStroke.StylusPoints
                            select x.ToPoint();
                        PathFigure pathFigure = new PathFigure
                        {
                            StartPoint = stylusPoints.First(),
                            IsClosed = true
                        };
                        foreach (Point current in stylusPoints.Skip(1))
                        {
                            pathFigure.Segments.Add(new LineSegment(current, false));
                        }
                        ClipImage.Clip = new PathGeometry
                        {
                            Figures = new PathFigureCollection
                               {
                                   pathFigure
                               }
                        };
                    }
                }
            }
            isMouseDown = false;
        }
        private void InkCanvas_MouseLeave(object sender, MouseEventArgs e)
        {
            this.ClipImageWithLassoStroke();
        }
    }
}
