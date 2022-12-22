using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.ViewModels;
namespace Note.InkCanvasEx.Controls
{
    public sealed partial class PenControl : UserControl
    {
        private double penSize;
        private Color penColor;
        private InkDrawingAttributes drawingAttributes;
        private PenViewModel viewModel;
        public PenControl()
        {
            this.InitializeComponent();
            this.Loaded += PenControl_Loaded;
        }
        private void PenControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(viewModel == null)
            {
                drawingAttributes = new InkDrawingAttributes();
                viewModel = new PenViewModel(this.inkCanvas);
                this.DataContext = viewModel;
                this.BallPenBtn.IsChecked = true;
                this.Stroke1Btn.IsChecked = true;
                this.BlackBtn.IsChecked = true;
            }
        }
        private void PenType_Checked(object sender, RoutedEventArgs e)
        {
            var btn=(ImageRadioButton)sender;
            switch(btn.Name)
            {
                case "ParkPenBtn":
                    this.viewModel.SelectedPen = this.viewModel.Pens[2];
                    break;
                case "BallPenBtn":
                    this.viewModel.SelectedPen = this.viewModel.Pens[0];
                    break ;
                case "PencilBtn":
                    this.viewModel.SelectedPen = this.viewModel.Pens[1];
                    break;
            }
        }
        private void PenSize_Checked(object sender, RoutedEventArgs e)
        {
            var btn = (ImageRadioButton)sender;
            switch (btn.Name)
            {
                case "Stroke1Btn":
                    this.viewModel.SelectedPenSize = this.viewModel.PenSizes[0];
                    break;
                case "Stroke2Btn":
                    this.viewModel.SelectedPenSize = this.viewModel.PenSizes[1];
                    break;
                case "Stroke3Btn":
                    this.viewModel.SelectedPenSize = this.viewModel.PenSizes[2];
                    break;
                case "Stroke4Btn":
                    this.viewModel.SelectedPenSize = this.viewModel.PenSizes[3];
                    break;
                case "Stroke5Btn":
                    this.viewModel.SelectedPenSize = this.viewModel.PenSizes[4];
                    break;
            }
        }
        private void PenColor_Checked(object sender, RoutedEventArgs e)
        {
            //RedBtn
            var btn = (ImageRadioButton)sender;
            switch (btn.Name)
            {
                case "RedBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Red;
                    this.viewModel.UpdateCanvas();
                    break;
                case "OrangeBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Orange;
                    this.viewModel.UpdateCanvas();
                    break;
                case "YellowBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Yellow;
                    this.viewModel.UpdateCanvas();
                    break;
                case "GreenBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Green;
                    this.viewModel.UpdateCanvas();
                    break;
                case "BlueBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Blue;
                    this.viewModel.UpdateCanvas();
                    break;
                case "CyanBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Cyan;
                    this.viewModel.UpdateCanvas();
                    break;
                case "VioletBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Violet;
                    this.viewModel.UpdateCanvas();
                    break;
                case "WhiteBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.White;
                    this.viewModel.UpdateCanvas();
                    break;
                case "GrayBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Gray;
                    this.viewModel.UpdateCanvas();
                    break;
                case "BlackBtn":
                    this.viewModel.DrawingAttributes.Color = Colors.Black;
                    this.viewModel.UpdateCanvas();
                    break;
            }
        }
    }
}
