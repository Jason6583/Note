using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    public sealed partial class MessageContainer : UserControl
    {
        public MessageContainer(string message)
        {
            this.InitializeComponent();
            this.txt.Text = message;
        }
    }
}
