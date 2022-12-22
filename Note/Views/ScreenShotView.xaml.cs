using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows;
using Note.ViewModels;
using Microsoft.Win32;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Collections.Generic;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.Controls;
using Note.InkCanvasEx.ViewModels;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;

namespace Note.Views
{
    public partial class ScreenShotView : Window
    {
        private InkCanvasViewModel _inkCanvasEx;
        private BitmapImage _bitmapImage;
        private string _dirDesktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        private string _dirTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Note", "tmp");
        public bool IsBeforeApp { get; set; }
        public ScreenShotView(InkCanvasViewModel inkCanvasEx)
        {
            InitializeComponent();
            this._inkCanvasEx = inkCanvasEx;
            if (!Directory.Exists(_dirTemp))
            {
                Directory.CreateDirectory(_dirTemp);
            }
            MediaPlayer player = new MediaPlayer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "CameraShutter.mp3");
            player.Open(new Uri(path));
            player.Play();
            _bitmapImage = CaptureScreen();
            this.RectCrop.SetImage(_bitmapImage, _bitmapImage.Width / 4 * 3, _bitmapImage.Height / 4 * 3);
            this.FullImage.Source = _bitmapImage;
            this.FullImage.Width = _bitmapImage.PixelWidth;
            this.FullImage.Height = _bitmapImage.PixelHeight;
            ContentRendered += ScreenShotView_ContentRendered;
        }
        private void ScreenShotView_ContentRendered(object sender, EventArgs e)
        {
            UpdateRectCropAndCursor();
        }
        private BitmapImage CaptureScreen()
        {
            BitmapImage bitmapImage = new BitmapImage();
            var width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
            var height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = ms;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                    }
                }
            }
            return bitmapImage;
        }
        public void UpdateRectCropAndCursor()
        {
            var storyboard = new Storyboard();
            var duration = new Duration(TimeSpan.FromMilliseconds(1000));
            var opacityAnim = new DoubleAnimation(1, duration);
            Storyboard.SetTargetProperty(opacityAnim, new PropertyPath(nameof(Opacity)));
            storyboard.Children.Add(opacityAnim);
            var widthAnim = new DoubleAnimation(RectCrop.ActualWidth, duration);
            Storyboard.SetTargetProperty(widthAnim, new PropertyPath(nameof(Width)));
            storyboard.Children.Add(widthAnim);
            var heightAnim = new DoubleAnimation(RectCrop.ActualHeight, duration);
            Storyboard.SetTargetProperty(heightAnim, new PropertyPath(nameof(Height)));
            storyboard.Children.Add(heightAnim);
            storyboard.Completed += Storyboard_Completed;
            RectCrop.BeginStoryboard(storyboard);
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {

        }
        void BeginClose()
        {
            var duration = new Duration(TimeSpan.FromMilliseconds(200));
            var opacityAnim = new DoubleAnimation(0, duration);
            BeginAnimation(OpacityProperty, opacityAnim);
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private async void SaveToDesktop_Click(object sender, RoutedEventArgs e)
        {
            var source = await GetCroppedImage();
            if (source != null)
            {
                string imgPath = Path.Combine(_dirDesktop, DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".png");
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                using (FileStream stream = new FileStream(imgPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }
                DoExit();
            }
        }
        private async void Copy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var source = await GetCroppedImage();
                if (source != null)
                {
                    Clipboard.SetImage(source);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void DoExit()
        {
            this.Topmost = false;
            this.Close();
        }
        /// <summary>
        /// 获取相对屏幕的Geometry
        /// </summary>
        /// <returns></returns>
        private EllipseGeometry TranslateGeometry()
        {
            List<System.Windows.Point> points = new List<System.Windows.Point>();
            double sx = FullImage.Width / RectCrop.Width;
            var first = RectCrop.LassoStroke.StylusPoints.First().ToPoint();
            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new System.Windows.Point(first.X * sx, first.Y * sx),
                IsClosed = true
            };
            foreach (var item in RectCrop.LassoStroke.StylusPoints.Skip(1))
            {
                var newPoint = item.ToPoint();
                pathFigure.Segments.Add(new LineSegment(new System.Windows.Point(newPoint.X * sx, newPoint.Y * sx), false));
            }
            PathGeometry path = new PathGeometry
            {
                Figures = new PathFigureCollection
                   {
                       pathFigure
                   }
            };
            var width = path.Bounds.Width;
            var height = path.Bounds.Height;
            var radiusX = width / 2;
            var radiusY = height / 2;
            var x = path.Bounds.X;
            var y = path.Bounds.Y;
            var center = new System.Windows.Point(x + radiusX, y + radiusY);
            var ellipse = new EllipseGeometry(center, radiusX, radiusY);
            return ellipse;
        }
        /// <summary>
        /// 获取相对屏幕的RectangleGeometry
        /// </summary>
        /// <returns></returns>
        private RectangleGeometry TranslateRectangleGeometry(RectangleGeometry rectG)
        {
            double sx = FullImage.Width / RectCrop.Width;
            var rect = rectG.Rect;
            var rectZoom = new Rect(new System.Windows.Point(rect.TopLeft.X * sx, rect.TopLeft.Y * sx), new System.Windows.Point(rect.BottomRight.X * sx, rect.BottomRight.Y * sx));
            var ret = new RectangleGeometry(rectZoom);
            return ret;
        }
        public async Task<BitmapSource> GetCroppedImage()
        {
            try
            {
                Geometry geometry = null;
                if (RectCrop.ClipImage.Clip is PathGeometry)
                {
                    geometry = TranslateGeometry();
                }
                else if (RectCrop.ClipImage.Clip is RectangleGeometry rectg)
                {
                    geometry = TranslateRectangleGeometry(rectg);
                }
                if (geometry != null)
                {
                    FullImage.Visibility = Visibility.Visible;
                    FullImage.Clip = geometry;
                    FullImage.InvalidateVisual();
                    await Task.Delay(100);
                    var bounds = geometry.Bounds;
                    var scale = VisualTreeHelper.GetDpi(this);
                    RenderTargetBitmap render_bmp = new RenderTargetBitmap((int)_bitmapImage.PixelWidth, (int)_bitmapImage.PixelHeight, 96, 96, PixelFormats.Pbgra32);
                    render_bmp.Render(FullImage);
                    FullImage.Visibility = Visibility.Collapsed;
                    //裁切图片，结果图片，不带边框和画刷
                    return new CroppedBitmap(render_bmp, new Int32Rect((int)(bounds.Left), (int)(bounds.Top), (int)bounds.Width, (int)bounds.Height));
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public void SaveImage(BitmapSource image)
        {
            if (image == null) return;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image Files (*.png, *.bmp, *.jpg)|*.png;*.bmp;*.jpg | All Files | *.*";
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog().GetValueOrDefault())
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                {
                    encoder.Save(stream);
                }
            }
        }
        public Bitmap ImageSourceToBitmap(BitmapSource bitmapSource)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                return new Bitmap(ms);
            }
        }

        private async void Insert_Click(object sender, RoutedEventArgs e)
        {
            var source = await GetCroppedImage();
            if (source != null)
            {
                string imgPath = Path.Combine(_dirTemp, DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".png");
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                using (FileStream stream = new FileStream(imgPath, FileMode.Create))
                {
                    encoder.Save(stream);
                }
                Task.Delay(300).Wait();
                ImageContainer image = new ImageContainer(imgPath);
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    var left = 200;
                    var top = 200;
                    inkCanvaEx.InsertImage(image, left, top);
                }
                image.RecognizeCallback += (object o, RecogizeEventArgs e) =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        var ocrText = e.Result;
                        var textview = new TextRecognizeView();
                        var textviewModel = new TextRecognizeViewModel(ocrText,this._inkCanvasEx);
                        textview.DataContext = textviewModel;
                        textview.ShowDialog();
                    });
                };
                DoExit();
            }
        }
    }
}
