using System;
using Windows.UI;
using System.Linq;
using Note.Models;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Note.InkCanvasEx.Enums;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.Commons;
using System.Collections.Generic;
using Note.InkCanvasEx.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Wpf.UI.XamlHost;

namespace Note.ViewModels
{
    public class PenControlViewModel : BindableBase
    {
        private InkCanvas SliderCanvas;

        private ObservableCollection<PenTipSize> penSizes = new ObservableCollection<PenTipSize>();
        public ObservableCollection<PenTipSize> PenSizes
        {
            get => this.penSizes;
            set => this.SetProperty(ref this.penSizes, value);
        }
        private PenTipSize selectedPenSize;
        public PenTipSize SelectedPenSize
        {
            get => this.selectedPenSize;
            set
            {
                this.DrawingAttributes.Size = new Size(value.PenSize, value.PenSize);
                this.UpdateCanvas();
                this.SetProperty(ref this.selectedPenSize, value);
            }
        }
        private ObservableCollection<PenColor> penColors = new ObservableCollection<PenColor>();
        public ObservableCollection<PenColor> PenColors
        {
            get => this.penColors;
            set => this.SetProperty(ref this.penColors, value);
        }
        private PenColor selectedPenColor;
        public PenColor SelectedPenColor
        {
            get => this.selectedPenColor;
            set
            {
                this.DrawingAttributes.Color = GetColor(value.Color);
                this.UpdateCanvas();
                this.SetProperty(ref this.selectedPenColor, value);
            }
        }
        public SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            return new SolidColorBrush(Color.FromArgb(a, r, g, b));
        }
        public Color GetColor(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            return Color.FromArgb(255, r, g, b);
        }
        private ObservableCollection<PenModel> pens = new ObservableCollection<PenModel>();
        public ObservableCollection<PenModel> Pens
        {
            get => this.pens;
            set => this.SetProperty(ref this.pens, value);
        }
        private PenModel selectedPen;
        public PenModel SelectedPen
        {
            get => this.selectedPen;
            set
            {
                this.SetProperty(ref this.selectedPen, value);
                if(this.SelectedPenSize!=null)
                {
                    this.SelectedPenSize = this.PenSizes[2];
                }
                if(this.selectedPen != null)
                {
                    this.DrawingAttributes = value.InkDrawingAttributes;
                }
                this.UpdateCanvas();
            }
        }
        private InkDrawingAttributes drawingAttributes=new InkDrawingAttributes();
        public InkDrawingAttributes DrawingAttributes
        {
            get { return this.drawingAttributes; }
            set { this.SetProperty(ref this.drawingAttributes, value); }
        }
        public PenControlViewModel(WindowsXamlHost xamlHost)
        {
            var xamlGrid = new Grid();
            xamlGrid.Background = new SolidColorBrush(Colors.White);
            SliderCanvas = new InkCanvas();
            SliderCanvas.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.None;
            xamlGrid.Children.Add(SliderCanvas);
            xamlHost.Child = xamlGrid;
            this.InitPenSizes();
            this.InitPenColors();
            this.InitPenType();
            this.UpdateCanvas();
        }
        private void InitPenType()
        {
            var ballPen = new PenModel();
            ballPen.PenType = PenType.BallPen;
            ballPen.Pen = "圆珠笔";
            ballPen.Image = new BitmapImage();
            ballPen.Image = (BitmapImage)System.Windows.Application.Current.FindResource("BallPen");
            ballPen.CheckedImage = new BitmapImage();
            ballPen.CheckedImage = (BitmapImage)System.Windows.Application.Current.FindResource("BallPenSelected");
            ballPen.InkDrawingAttributes = CreateBallPenDrawingAttributes(new SolidColorBrush(Colors.Black), 6);
            this.Pens.Add(ballPen);

            var pencil = new PenModel();
            pencil.PenType= PenType.Pencil;
            pencil.Pen = "铅笔";
            pencil.Image = new BitmapImage();
            pencil.Image = (BitmapImage)System.Windows.Application.Current.FindResource("Pencil");
            pencil.CheckedImage = new BitmapImage();
            pencil.CheckedImage = (BitmapImage)System.Windows.Application.Current.FindResource("PencilSelected");
            pencil.InkDrawingAttributes = InkDrawingAttributes.CreateForPencil();
            pencil.InkDrawingAttributes.Color = Colors.Black;
            pencil.InkDrawingAttributes.FitToCurve = true;
            pencil.InkDrawingAttributes.IgnorePressure = false;
            pencil.InkDrawingAttributes.IgnoreTilt = false;
            pencil.InkDrawingAttributes.Size = new Size(6, 6);
            pencil.InkDrawingAttributes.PencilProperties.Opacity = 0.8f;
            this.Pens.Add(pencil);

            var parkPen = new PenModel();
            parkPen.Pen = "钢笔";
            parkPen.PenType = PenType.ParkPen;
            parkPen.Image = new BitmapImage();
            parkPen.Image = (BitmapImage)System.Windows.Application.Current.FindResource("ParkPen");
            parkPen.CheckedImage = new BitmapImage();
            parkPen.CheckedImage = (BitmapImage)System.Windows.Application.Current.FindResource("ParkPenSelected");
            parkPen.InkDrawingAttributes = CreateInkDrawingAttributesCore(new SolidColorBrush(Colors.Black), 6);
            this.Pens.Add(parkPen);
            this.SelectedPen = ballPen;
            this.SelectedPenSize = this.PenSizes[2];
        }
        private void InitPenColors()
        {
            PenColors.Add(new PenColor { Color = "#000000" });
            PenColors.Add(new PenColor { Color = "#707070" });
            PenColors.Add(new PenColor { Color = "#B8B8B8" });
            PenColors.Add(new PenColor { Color = "#DCDCDC" });
            PenColors.Add(new PenColor { Color = "#FFFFFF" });
            PenColors.Add(new PenColor { Color = "#A36ED3" });
            PenColors.Add(new PenColor { Color = "#C943C6" });
            PenColors.Add(new PenColor { Color = "#FFC171" });
            PenColors.Add(new PenColor { Color = "#F4EE22" });
            PenColors.Add(new PenColor { Color = "#89E08A" });
            this.SelectedPenColor = PenColors.First();
        }
        private void InitPenSizes()
        {
            this.PenSizes.Clear();
            var penSize = new PenTipSize();
            penSize.IconSize = 1.5;
            penSize.PenSize = 1.5;
            this.PenSizes.Add(penSize);
            penSize = new PenTipSize();
            penSize.IconSize = 3;
            penSize.PenSize = 3;
            this.PenSizes.Add(penSize);
            penSize = new PenTipSize();
            penSize.IconSize = 4.5;
            penSize.PenSize = 4.5;
            this.PenSizes.Add(penSize);
            penSize = new PenTipSize();
            penSize.IconSize = 9;
            penSize.PenSize = 6;
            this.PenSizes.Add(penSize);
            penSize = new PenTipSize();
            penSize.IconSize = 12;
            penSize.PenSize = 12;
            this.PenSizes.Add(penSize);
            this.SelectedPenSize = this.PenSizes.First();
        }
        public void UpdateCanvas()
        {
            try
            {
                SliderCanvas.InkPresenter.StrokeContainer.Clear();
                var strokeBuilder = new InkStrokeBuilder();
                if (DrawingAttributes == null)
                    DrawingAttributes = new InkDrawingAttributes();
                strokeBuilder.SetDefaultDrawingAttributes(DrawingAttributes);
                List<Point> Points;
                Points = new List<Point>();
                Points.Add(new Point(22, 30));
                Points.Add(new Point(57, 10));
                Points.Add(new Point(92, 20));
                Points.Add(new Point(127, 30));
                Points.Add(new Point(162, 10));
                InkStroke stkA = strokeBuilder.CreateStroke(Points);
                SliderCanvas.InkPresenter.StrokeContainer.AddStroke(stkA);
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.InkCanvas.InkPresenter.UpdateDefaultDrawingAttributes(DrawingAttributes);
                    inkCanvaEx.CurrentPenType = this.SelectedPen.PenType;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        protected InkDrawingAttributes CreateInkDrawingAttributesCore(Brush brush, double strokeWidth)
        {
            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.DrawAsHighlighter = false;
            inkDrawingAttributes.FitToCurve = true;
            inkDrawingAttributes.PenTip = PenTipShape.Circle;
            inkDrawingAttributes.IgnorePressure = false;
            SolidColorBrush solidColorBrush = (SolidColorBrush)brush;
            inkDrawingAttributes.Color = solidColorBrush.Color;
            inkDrawingAttributes.Size = new Size(strokeWidth * 1.5f, 0.3f * strokeWidth * 1.5f);
            inkDrawingAttributes.PenTipTransform = Matrix3x2.CreateRotation((float)(Math.PI * 45 / 180));
            return inkDrawingAttributes;
        }
        private InkDrawingAttributes CreateBallPenDrawingAttributes(Brush brush, double strokeWidth)
        {
            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.DrawAsHighlighter = false;
            inkDrawingAttributes.FitToCurve = true;
            inkDrawingAttributes.PenTip = PenTipShape.Circle;
            inkDrawingAttributes.IgnorePressure = false;
            SolidColorBrush solidColorBrush = (SolidColorBrush)brush;
            inkDrawingAttributes.Color = solidColorBrush.Color;
            inkDrawingAttributes.Size = new Size(strokeWidth,strokeWidth);
            inkDrawingAttributes.PenTipTransform = Matrix3x2.Identity;
            return inkDrawingAttributes;
        }
    }
}
