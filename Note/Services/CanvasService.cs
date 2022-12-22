using System;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Note.Services
{
    public class CanvasInteraction
    {
        private Canvas Canvas;
        public CanvasInteraction(Canvas c)
        {
            this.Canvas = c;
        }
        public void DrawLine(Point fromPoint, Point toPoint, String color, int strockeThickness)
        {
            byte r = Convert.ToByte(color.Substring(1, 2), 16);
            byte g = Convert.ToByte(color.Substring(3, 2), 16);
            byte b = Convert.ToByte(color.Substring(5, 2), 16);
            DrawLine(fromPoint, toPoint, Color.FromArgb(255, r, g, b), strockeThickness);
        }
        public void DrawLine(Point fromPoint, Point toPoint, Color color, int strockeThickness)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(color);
            line.StrokeStartLineCap = PenLineCap.Round;
            line.StrokeEndLineCap = PenLineCap.Round;
            line.StrokeThickness = strockeThickness;
            line.X1 = fromPoint.X;
            line.Y1 = Math.Max(strockeThickness / 2, fromPoint.Y);
            line.X2 = toPoint.X;
            line.Y2 = Math.Max(strockeThickness / 2, toPoint.Y);
            Canvas.Children.Add(line);
        }
        public void DrawImage(Point center, Size s, BitmapImage image)
        {
            Image img = new Image();
            img.Source = (ImageSource)image;
            int imageHeight = image.PixelHeight == 0 ? 320 : image.PixelWidth;
            int imageWidth = image.PixelWidth == 0 ? 180 : image.PixelWidth;
            ScaleTransform resizeTransform = new ScaleTransform();
            resizeTransform.ScaleX = (s.Width / imageHeight) * Canvas.Width;
            resizeTransform.ScaleY = (s.Height / imageWidth) * Canvas.Height;
            TranslateTransform posTransform = new TranslateTransform();
            posTransform.X = center.X * Canvas.Width;
            posTransform.Y = center.Y * Canvas.Height;
            TransformGroup transform = new TransformGroup();
            transform.Children.Add(resizeTransform);
            transform.Children.Add(posTransform);
            img.RenderTransform = transform;
            Canvas.Children.Add(img);
        }
        public void DrawImage(Point center, Size s, Byte[] img_bytes)
        {
            InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream();
            DataWriter writer = new DataWriter(ms.GetOutputStreamAt(0));
            writer.WriteBytes(img_bytes);
            writer.StoreAsync().GetResults();
            BitmapImage image = new BitmapImage();
            image.SetSource(ms);
            DrawImage(center, s, image);
        }
        public void Clear()
        {
            Canvas.Children.Clear();
        }
    }
}
