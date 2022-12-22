using Note.InkCanvasEx.ViewModels;
using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    public sealed partial class EraserControl : UserControl
    {
        private EraserViewModel viewModel;
        public EraserControl()
        {
            this.InitializeComponent();
            this.Loaded += EraserControl_Loaded;
        }
        private void EraserControl_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
            if (inkCanvaEx != null)
            {
                if (viewModel == null)
                {
                    viewModel = new EraserViewModel(inkCanvaEx);
                    this.DataContext = viewModel;
                }
            }
        }
        private void EraserSize_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var btn = (ImageRadioButton)sender;
            switch (btn.Name)
            {
                case "Stroke1Btn":
                    this.viewModel.SelectedEraserSize = this.viewModel.EraserSizes[0];
                    break;
                case "Stroke2Btn":
                    this.viewModel.SelectedEraserSize = this.viewModel.EraserSizes[1];
                    break;
                case "Stroke3Btn":
                    this.viewModel.SelectedEraserSize = this.viewModel.EraserSizes[2];
                    break;
                case "Stroke4Btn":
                    this.viewModel.SelectedEraserSize = this.viewModel.EraserSizes[3];
                    break;
                case "Stroke5Btn":
                    this.viewModel.SelectedEraserSize = this.viewModel.EraserSizes[4];
                    break;
            }
        }
    }
}
