using Note.ViewModels;
using System.Windows;
using System.Windows.Input;
using Note.InkCanvasEx.ViewModels;

namespace Note
{
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
       
            this.Loaded += MainWindow_Loaded;
            this.ContentRendered += MainWindow_ContentRendered;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = new MainViewModel();
            }
            this.DataContext = viewModel;
        }
        private void MainWindow_ContentRendered(object sender, System.EventArgs e)
        {
            var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
            if (inkCanvaEx != null)
            {
                if (viewModel == null)
                {
                    viewModel = new MainViewModel();
                    this.DataContext = viewModel;
                }
                this.viewModel.LoadInkCanvasEx(inkCanvaEx);
            }
        }
        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MaximizeButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
        private void MinimizeButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
