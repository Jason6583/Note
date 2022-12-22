using System.Windows;
using System.Windows.Controls;

namespace Note.Views
{
    public partial class AudioPanel : UserControl
    {
        public AudioPanel()
        {
            InitializeComponent();
            this.Loaded += AudioPanel_Loaded;
        }
        private void AudioPanel_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
