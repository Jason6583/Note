using System.Windows;

namespace Note.Views
{
    public partial class TextRecognizeView : Window
    {
        public TextRecognizeView()
        {
            InitializeComponent();
        }
        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
