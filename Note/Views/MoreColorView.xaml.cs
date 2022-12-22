using Note.ViewModels;
using System.Windows.Controls;
namespace Note.Views
{
    public partial class MoreColorView : UserControl
    {
        private MoreColorViewModel viewModel;
        public MoreColorView()
        {
            InitializeComponent();
            this.Loaded += MoreColorPopupView_Loaded;
        }
        private void MoreColorPopupView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.viewModel == null)
            {
                this.viewModel = new MoreColorViewModel();
                this.DataContext = this.viewModel;
            }
        }
    }
}
