using Note.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Note.Views
{
    public partial class PenControl : UserControl
    {
        private PenControlViewModel viewModel;
        public PenControl()
        {
            InitializeComponent();
            this.Loaded += PenControl_Loaded;
        }
        private void PenControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.viewModel == null)
            {
                this.viewModel = new PenControlViewModel(this.windowsXamlHost);
                this.DataContext = this.viewModel;
            }
        }
    }
}
