using Microsoft.Toolkit.Win32.UI.XamlHost;
namespace Note.InkCanvasEx
{
    sealed partial class App : XamlApplication
    {
        public App()
        {
            base.Initialize();
            this.InitializeComponent();
        }
    }
}
