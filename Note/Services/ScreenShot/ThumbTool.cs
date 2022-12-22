using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Note.Services.ScreenShot
{
    public class ThumbCrop : Thumb
    {
        public ThumbCrop(double thumbSize, Cursor cursor)
        {
            ThumbSize = thumbSize;
            Cursor = cursor;
            _brush = new SolidColorBrush(Color.FromRgb(37, 126, 255));
            ; PreviewMouseLeftButtonDown += ThumbCrop_PreviewMouseLeftButtonDown;
            PreviewMouseLeftButtonUp += ThumbCrop_PreviewMouseLeftButtonUp;
        }
        private void ThumbCrop_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            thumb?.ReleaseMouseCapture();
        }
        private void ThumbCrop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            thumb?.CaptureMouse();
        }
        public double ThumbSize { get; }
        private SolidColorBrush _brush;
        /// <summary>
        ///     设置Thumb坐标位置
        /// </summary>
        /// <param name="x">X 坐标</param>
        /// <param name="y">Y 坐标</param>
        public void Redraw(double x, double y)
        {
            Canvas.SetTop(this, y - ThumbSize / 2);
            Canvas.SetLeft(this, x - ThumbSize / 2);
        }
        protected override Visual GetVisualChild(int index) => null;
        /// <summary>
        /// 自定义Thumb的样式
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(_brush, new Pen(Brushes.Blue, 0), new Rect(0, 0, 16, 16));
        }
    }
    public class ThumbTool
    {
        private readonly CropTool cropTool;
        public ThumbCrop BottomMiddle { get; }
        public ThumbCrop LeftMiddle { get; }
        public ThumbCrop TopMiddle { get; }
        public ThumbCrop RightMiddle { get; }
        public ThumbCrop TopLeft { get; }
        public ThumbCrop TopRight { get; }
        public ThumbCrop BottomLeft { get; }
        public ThumbCrop BottomRight { get; }
        private readonly Canvas canvas;
        private readonly double thumbSize = 16;
        public ThumbTool(Canvas canvas, CropTool cropTool)
        {
            this.canvas = canvas;
            this.cropTool = cropTool;
            BottomMiddle = new ThumbCrop(thumbSize, Cursors.SizeNS);
            LeftMiddle = new ThumbCrop(thumbSize, Cursors.SizeWE);
            TopMiddle = new ThumbCrop(thumbSize, Cursors.SizeNS);
            RightMiddle = new ThumbCrop(thumbSize, Cursors.SizeWE);
            TopLeft = new ThumbCrop(thumbSize, Cursors.SizeNWSE);
            TopRight = new ThumbCrop(thumbSize, Cursors.SizeNESW);
            BottomLeft = new ThumbCrop(thumbSize, Cursors.SizeNESW);
            BottomRight = new ThumbCrop(thumbSize, Cursors.SizeNWSE);

            LeftMiddle.DragDelta += LeftMiddle_DragDelta;
            BottomMiddle.DragDelta += BottomMiddle_DragDelta;
            TopMiddle.DragDelta += TopMiddle_DragDelta;
            RightMiddle.DragDelta += RightMiddle_DragDelta;
            TopLeft.DragDelta += TopLeft_DragDelta;
            TopRight.DragDelta += TopRight_DragDelta;
            BottomLeft.DragDelta += BottomLeft_DragDelta;
            BottomRight.DragDelta += BottomRight_DragDelta;
        }
        private void BottomRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double resultThumbLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;
            if (resultThumbLeft > canvas.ActualWidth)
                resultThumbLeft = canvas.ActualWidth;
            double thumbResultTop = Canvas.GetTop(thumb) + e.VerticalChange;
            if (thumbResultTop + thumbSize / 2 > canvas.ActualHeight)
                thumbResultTop = canvas.ActualHeight - thumbSize / 2;
            double resultHeight = thumbResultTop - cropTool.TopLeftY + thumbSize / 2;
            double resultWidth = resultThumbLeft - cropTool.TopLeftX;
            cropTool.Redraw(cropTool.TopLeftX, cropTool.TopLeftY, resultWidth, resultHeight);
        }
        private void BottomLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double thumbResultTop = Canvas.GetTop(thumb) + e.VerticalChange;
            if (thumbResultTop + thumbSize / 2 > canvas.ActualHeight)
                thumbResultTop = canvas.ActualHeight - thumbSize / 2;
            double resultHeight = thumbResultTop - cropTool.TopLeftY + thumbSize / 2;
            double resultThumbLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;
            if (resultThumbLeft < 0)
                resultThumbLeft = -thumbSize / 2;
            double offset = Canvas.GetLeft(thumb) - resultThumbLeft;
            double resultLeft = resultThumbLeft + thumbSize / 2;
            double resultWidth = cropTool.Width + offset;
            cropTool.Redraw(resultLeft, cropTool.TopLeftY, resultWidth, resultHeight);
        }
        private void TopRight_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double newTop = Canvas.GetTop(thumb) + e.VerticalChange;
            double newLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;

            if (newTop < 0)
                newTop = -thumbSize / 2;
            double offset = Canvas.GetTop(thumb) - newTop;
            double resultHeight = cropTool.Height + offset;
            double resultTop = newTop + thumbSize / 2;
            if (newLeft > canvas.ActualWidth)
                newLeft = canvas.ActualWidth;
            double resultWidth = newLeft - cropTool.TopLeftX;
            cropTool.Redraw(cropTool.TopLeftX, resultTop, resultWidth, resultHeight);
        }
        private void TopLeft_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double newTop = Canvas.GetTop(thumb) + e.VerticalChange;
            double newLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;
            if (newTop < 0)
                newTop = -thumbSize / 2;
            if (newLeft < 0)
                newLeft = -thumbSize / 2;
            double offsetTop = Canvas.GetTop(thumb) - newTop;
            double resultHeight = cropTool.Height + offsetTop;
            double resultTop = newTop + thumbSize / 2;
            double offsetLeft = Canvas.GetLeft(thumb) - newLeft;
            double resultLeft = newLeft + thumbSize / 2;
            double resultWidth = cropTool.Width + offsetLeft;
            cropTool.Redraw(resultLeft, resultTop, resultWidth, resultHeight);
        }

        private void RightMiddle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double resultThumbLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;
            if (resultThumbLeft > canvas.ActualWidth)
                resultThumbLeft = canvas.ActualWidth;
            double resultWidth = resultThumbLeft - cropTool.TopLeftX;
            cropTool.Redraw(cropTool.TopLeftX, cropTool.TopLeftY, resultWidth, cropTool.Height);
        }

        private void TopMiddle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double resultThumbTop = Canvas.GetTop(thumb) + e.VerticalChange;
            if (resultThumbTop < 0)
                resultThumbTop = -thumbSize / 2;
            double offset = Canvas.GetTop(thumb) - resultThumbTop;
            double resultHeight = cropTool.Height + offset;
            double resultTop = resultThumbTop + thumbSize / 2;
            cropTool.Redraw(cropTool.TopLeftX, resultTop, cropTool.Width, resultHeight);
        }

        private void LeftMiddle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double resultThumbLeft = Canvas.GetLeft(thumb) + e.HorizontalChange;
            if (resultThumbLeft < 0)
                resultThumbLeft = -thumbSize / 2;
            double offset = Canvas.GetLeft(thumb) - resultThumbLeft;
            double resultLeft = resultThumbLeft + thumbSize / 2;
            double resultWidth = cropTool.Width + offset;
            cropTool.Redraw(resultLeft, cropTool.TopLeftY, resultWidth, cropTool.Height);
        }

        private void BottomMiddle_DragDelta(object sender, DragDeltaEventArgs e)
        {
            ThumbCrop thumb = sender as ThumbCrop;
            double thumbResultTop = Canvas.GetTop(thumb) + e.VerticalChange;
            if (thumbResultTop > canvas.ActualHeight)
                thumbResultTop = canvas.ActualHeight;
            cropTool.Redraw(cropTool.TopLeftX, cropTool.TopLeftY, cropTool.Width, thumbResultTop - cropTool.TopLeftY);
        }

        public void Redraw()
        {
            if (cropTool.Height <= 0 && cropTool.Width <= 0)
            {
                ShowThumbs(false);
                return;
            }
            //BottomMiddle.Redraw(cropTool.TopLeftX + cropTool.Width / 2, cropTool.TopLeftY + cropTool.Height);
            //LeftMiddle.Redraw(cropTool.TopLeftX, cropTool.TopLeftY + cropTool.Height / 2);
            //TopMiddle.Redraw(cropTool.TopLeftX + cropTool.Width / 2, cropTool.TopLeftY);
            //RightMiddle.Redraw(cropTool.TopLeftX + cropTool.Width, cropTool.TopLeftY + cropTool.Height / 2);
            TopLeft.Redraw(cropTool.TopLeftX, cropTool.TopLeftY);
            TopRight.Redraw(cropTool.TopLeftX + cropTool.Width, cropTool.TopLeftY);
            BottomLeft.Redraw(cropTool.TopLeftX, cropTool.TopLeftY + cropTool.Height);
            BottomRight.Redraw(cropTool.TopLeftX + cropTool.Width, cropTool.TopLeftY + cropTool.Height);
            ShowThumbs(true);
        }
        private void ShowThumbs(bool isVisible)
        {
            if (cropTool.Height > 0 && cropTool.Width > 0)
            {
                //BottomMiddle.Visibility = Visibility.Visible;
                //LeftMiddle.Visibility = Visibility.Visible;
                //TopMiddle.Visibility = Visibility.Visible;
                //RightMiddle.Visibility = Visibility.Visible;
                TopLeft.Visibility = Visibility.Visible;
                TopRight.Visibility = Visibility.Visible;
                BottomLeft.Visibility = Visibility.Visible;
                BottomRight.Visibility = Visibility.Visible;
            }
            else
            {
                //BottomMiddle.Visibility = Visibility.Hidden;
                //LeftMiddle.Visibility = Visibility.Hidden;
                //TopMiddle.Visibility = Visibility.Hidden;
                //RightMiddle.Visibility = Visibility.Hidden;
                TopLeft.Visibility = Visibility.Hidden;
                TopRight.Visibility = Visibility.Hidden;
                BottomLeft.Visibility = Visibility.Hidden;
                BottomRight.Visibility = Visibility.Hidden;
            }
        }
    }
}
