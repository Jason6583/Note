using Note.InkCanvasEx.ViewModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    public sealed partial class MoreColors : UserControl
    {
        public MoreColors()
        {
            this.InitializeComponent();
        }
        private void BackColor_Checked(object sender, RoutedEventArgs e)
        {
            var btn = (ImageRadioButton)sender;
            var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
            if (inkCanvaEx == null)
            {
                return;
            }
            switch (btn.Name)
            {
                case "BackColor1":
                    inkCanvaEx.ChangeBackground(Colors.Red);
                    break;
                case "BackColor2":
                    inkCanvaEx.ChangeBackground(Colors.Orange);
                    break;
                case "BackColor3":
                    inkCanvaEx.ChangeBackground(Colors.Yellow);
                    break;
                case "BackColor4":
                    inkCanvaEx.ChangeBackground(Colors.Green);
                    break;
                case "BackColor5":
                    inkCanvaEx.ChangeBackground(Colors.Blue);
                    break;
                case "BackColor6":
                    inkCanvaEx.ChangeBackground(Colors.Cyan);
                    break;
                case "BackColor7":
                    inkCanvaEx.ChangeBackground(Colors.Violet);
                    break;
                case "BackColor8":
                    inkCanvaEx.ChangeBackground(Colors.White);
                    break;
                case "BackColor9":
                    inkCanvaEx.ChangeBackground(Colors.Gray);
                    break;
                case "BackColor10":
                    inkCanvaEx.ChangeBackground(Colors.Black);
                    break;
            }
        }
    }
}
