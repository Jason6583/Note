using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Note.Services.ScreenShot
{
    public class CropArea
    {
        public readonly Size OriginalSize;
        public readonly Rect CroppedRectAbsolute;
        public CropArea(Size originalSize, Rect croppedRectAbsolute)
        {
            OriginalSize = originalSize;
            CroppedRectAbsolute = croppedRectAbsolute;
        }
    }
    public class CropService
    {
        private readonly CropAdorner cropAdorner;
        private readonly Canvas canvas;
        private readonly CropTool cropTool;
        //private bool isLoaded = false;
        public Adorner Adorner => cropAdorner;
        public event EventHandler OnCropAreaChanged;
        private enum TouchPoint
        {
            OutsideRectangle,
            InsideRectangle
        }
        public CropService(FrameworkElement adornedElement)
        {
            canvas = new Canvas
            {
                Height = adornedElement.ActualHeight,
                Width = adornedElement.ActualWidth
            };
            cropAdorner = new CropAdorner(adornedElement, canvas);
            var adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            Debug.Assert(adornerLayer != null, nameof(adornerLayer) + " != null");
            adornerLayer.Add(cropAdorner);
            cropTool = new CropTool(canvas, this);
            //cropAdorner.MouseLeftButtonDown += AdornerOnMouseLeftButtonDown;
            //cropAdorner.MouseMove += AdornerOnMouseMove;
            //cropAdorner.MouseLeftButtonUp += AdornerOnMouseLeftButtonUp;
            cropTool.Redraw(0, 0, adornedElement.ActualWidth, adornedElement.ActualHeight);
        }
        public CropArea GetCroppedArea() =>
            new CropArea(
                cropAdorner.RenderSize,
                new Rect(cropTool.TopLeftX, cropTool.TopLeftY, cropTool.Width, cropTool.Height)
            );

        public void ShowAdorner()
        {
            cropAdorner.Visibility = Visibility.Visible;
        }

        public void HideAdorner()
        {
            cropAdorner.Visibility = Visibility.Collapsed;
        }

        public void CropAreaChanged()
        {
            OnCropAreaChanged?.Invoke(this, EventArgs.Empty);
        }

        private void AdornerOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvas.ReleaseMouseCapture();
        }
        private void AdornerOnMouseMove(object sender, MouseEventArgs e)
        {
            //if (!isLoaded)
            //{
            //    cropTool.Redraw(0, 0, canvas.ActualWidth, canvas.ActualHeight);
            //    isLoaded = true;
            //}
        }
        private void AdornerOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            canvas.CaptureMouse();
            var point = e.GetPosition(canvas);
            var touch = GetTouchPoint(point);
        }
        //判断鼠标区域：
        private TouchPoint GetTouchPoint(Point mousePoint)
        {
            //left
            if (mousePoint.X < cropTool.TopLeftX)
                return TouchPoint.OutsideRectangle;
            //right
            if (mousePoint.X > cropTool.BottomRightX)
                return TouchPoint.OutsideRectangle;
            //top
            if (mousePoint.Y < cropTool.TopLeftY)
                return TouchPoint.OutsideRectangle;
            //bottom
            if (mousePoint.Y > cropTool.BottomRightY)
                return TouchPoint.OutsideRectangle;
            return TouchPoint.InsideRectangle;
        }
    }
}
