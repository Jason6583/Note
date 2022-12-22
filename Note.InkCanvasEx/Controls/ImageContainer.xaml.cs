using System;
using System.Text;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.YouDao;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Note.InkCanvasEx.Controls
{
    public delegate void RecognizeEventHandler(object sender, RecogizeEventArgs e);
    public partial class ImageContainer : UserControl
    {
        private bool _isResizing = false;
        private double _scaleX = 1;
        private double _scaleY = 1;
        public string FileName { get; set; }
        public EventHandler CloseHandler;
        public EventHandler RecognizeHandler;
        public RecognizeEventHandler RecognizeCallback;
        public ImageContainer(string fileName)
        {
            this.InitializeComponent();
            this.FileName = fileName;
            this.Loaded += ImageContainer_Loaded;
        }
        private void ImageContainer_Loaded(object sender, RoutedEventArgs e)
        {
            var bitmap = new BitmapImage();
            bitmap.UriSource = new Uri(this.FileName);
            this.image.Source = bitmap;
        }
        private void Manipulator_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.Width = this.ActualWidth;
            this.Height = this.ActualHeight;
            this.border.Width = this.border.ActualWidth;
            this.border.Height = this.border.ActualHeight;
            if (e.Position.X > Width - scaleImage.Width && e.Position.Y > Height - scaleImage.Height) _isResizing = true;
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
                    //this.border.Width += e.Delta.Translation.X;
                    //this.border.Height += e.Delta.Translation.Y;
                    //var scaleDeltaX = e.Delta.Translation.X / Width;
                    //var scaleDeltaY = e.Delta.Translation.Y / Height;
                    //Width += e.Delta.Translation.X;
                    //Height += e.Delta.Translation.Y;
                    //_scaleX += scaleDeltaX;
                    //_scaleY += scaleDeltaY;
                    //if (_scaleX > 0.3 && _scaleY > 0.3)
                    //{
                    //    //var scaleTransform = new ScaleTransform();
                    //    //scaleTransform.ScaleX = _scaleX;
                    //    //scaleTransform.ScaleY = _scaleY;
                    //    //this.ElementGrid.RenderTransform = scaleTransform;
                    //    this.ElementTransform.ScaleX = _scaleX;
                    //    this.ElementTransform.ScaleY = _scaleY;
                    //}
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
            this.border.Visibility = Visibility.Visible;
            this.closeImage.Visibility = Visibility.Visible;
            this.scaleImage.Visibility = Visibility.Visible;
            this.toolBar.Visibility = Visibility.Visible;
        }
        private void ContainerGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.border.Visibility = Visibility.Collapsed;
            this.closeImage.Visibility = Visibility.Collapsed;
            this.scaleImage.Visibility = Visibility.Collapsed;
            this.toolBar.Visibility= Visibility.Collapsed;
        }
        private void CloseImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CloseHandler?.Invoke(this, new EventArgs());
        }
        private void Recognize_Click(object sender, TappedRoutedEventArgs e)
        {
            if (this.FileName != null)
            {
                Task.Run(() =>
                {
                    this.RecognizeHandler?.Invoke(sender, new EventArgs());
                    var base64 = YouDaoOcrService.LoadAsBase64(this.FileName);
                    var result = YouDaoOcrService.GetOcrAsync(base64).Result;
                    var regions = result.Result.Regions;
                    var sb = new StringBuilder();
                    foreach (var region in regions)
                    {
                        foreach (var line in region.Lines)
                        {
                            sb.AppendLine(line.Text);
                        }
                    }
                    var ocrText = sb.ToString();
                    RecognizeCallback?.Invoke(this, new RecogizeEventArgs(ocrText));
                });
            }
        }
        private void Manipulator_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }
    }
}
