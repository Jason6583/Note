using System;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    public partial class ShapeContainer : UserControl
    {
        private bool _isResizing = false;
        private Path _path;
        private double _width;
        private double _height;
        public string ShapeStr { get; set; }
        public EventHandler CloseHandler { get; set; }
        public ShapeContainer(Path path, string content, Rect rect)
        {
            this.InitializeComponent();
            _path = path;
            this.ElementGrid.Children.Add(path);
            this.Width = path.Width;
            this.Height = path.Height;
            this.ShapeStr = content;
            this.border.Width = rect.Width;
            this.border.Height = rect.Height;
        }
        private void Manipulator_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.Width = this.ActualWidth;
            this.Height = this.ActualHeight;
            this._width = this.ActualWidth;
            this._height = this.ActualHeight;
            if (e.Position.X > Width - resizer.Width && e.Position.Y > Height - resizer.Height) _isResizing = true;
            else _isResizing = false;
        }
        public static Matrix4x4 ToMatrix4x4(Matrix3x2 matrix)
        {
            return new Matrix4x4(
               matrix.M11, matrix.M12, 0, 0,
               matrix.M21, matrix.M22, 0, 0,
               0, 0, 1, 0,
               matrix.M31, matrix.M32, 0, 1);
        }
        private void Manipulator_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            try
            {
                if (_isResizing)
                {
                    Width += e.Delta.Translation.X;
                    Height += e.Delta.Translation.Y;
                    var width = this.border.Width;
                    var height = this.border.Height;
                    this.border.Width += e.Delta.Translation.X;
                    this.border.Height += e.Delta.Translation.Y;
                    var scaleX = this.Width / _width;
                    var scaleY = this.Height / _height;
                    var scaleTransform = new ScaleTransform();
                    scaleTransform.ScaleX = scaleX;
                    scaleTransform.ScaleY = scaleY;
                    this._path.RenderTransform = scaleTransform;
                }
                else
                {
                    var scale = Matrix3x2.CreateScale(e.Delta.Scale);
                    Matrix3x2 transformX;
                    transformX = Matrix3x2.CreateTranslation((float)-e.Position.X, (float)-e.Position.Y) *
                                          scale *
                                          Matrix3x2.CreateTranslation((float)e.Position.X, (float)e.Position.Y) *
                                          Matrix3x2.CreateTranslation((float)(e.Delta.Translation.X), (float)(e.Delta.Translation.Y));
                    this.TransformMatrix *= ToMatrix4x4(transformX);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void ContainerGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            resizer.Visibility = Visibility.Visible;
            closer.Visibility = Visibility.Visible;
            border.Visibility = Visibility.Visible;
        }
        private void ContainerGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            resizer.Visibility = Visibility.Collapsed;
            closer.Visibility = Visibility.Collapsed;
            border.Visibility = Visibility.Collapsed;
        }
        private void CloseImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CloseHandler?.Invoke(sender, new EventArgs());
        }
    }
}
