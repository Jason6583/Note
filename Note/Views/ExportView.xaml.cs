using System.Windows.Controls;
using Note.InkCanvasEx.ViewModels;

namespace Note.Views
{
    public partial class ExportView : UserControl
    {
        private ExportViewModel viewModel;
        public ExportView()
        {
            InitializeComponent();
            this.Loaded += ExportView_Loaded;
        }
        private void ExportView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var inkCanvasEx = InkCanvasViewModel.InkCanvasEx;
            if(inkCanvasEx != null)
            {
                if (viewModel == null)
                {                
                    viewModel = new ExportViewModel(inkCanvasEx);
                }
                this.DataContext = this.viewModel;
            }
        }
    }
}
