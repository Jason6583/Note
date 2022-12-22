using Note.ViewModels;
using System.Windows.Controls;
using Note.InkCanvasEx.ViewModels;

namespace Note.Views
{
    public partial class EraserView : UserControl
    {
        private EraserViewModel viewModel;
        public EraserView()
        {
            InitializeComponent();
            this.Loaded += EraserPopupView_Loaded;        
        }
        private void EraserPopupView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var inkCanvasEx = InkCanvasViewModel.InkCanvasEx;
            if (inkCanvasEx != null)
            {
                if (viewModel == null)
                {
                    viewModel = new EraserViewModel(inkCanvasEx);
                }
                this.DataContext = this.viewModel;
            }
        }
    }
}
