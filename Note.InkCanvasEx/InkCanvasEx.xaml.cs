using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.ViewModels;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Note.InkCanvasEx
{
    public sealed partial class InkCanvasEx : UserControl
    {
        private InkCanvasViewModel viewModel;
        public InkCanvasEx()
        {
            this.InitializeComponent();
            this.Loaded += InkCanvasEx_Loaded;
        }
        private void InkCanvasEx_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.inkCanvas.InkPresenter.InputDeviceTypes = 
                Windows.UI.Core.CoreInputDeviceTypes.Mouse| 
                Windows.UI.Core.CoreInputDeviceTypes.Pen;
            if(this.viewModel==null)
                this.viewModel = new InkCanvasViewModel(this.mainView,this.scrollViewer,this.shapeCanvas, this.inkCanvas, this.selectionCanvas, this.imageCanvas);
            this.DataContext = this.viewModel;

        }
        void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {

        }
    }
}
