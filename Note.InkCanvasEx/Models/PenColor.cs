using Note.InkCanvasEx.Commons;
using Windows.UI.Xaml.Media.Imaging;

namespace Note.InkCanvasEx.Models
{
    public class PenColor : BindableBase
    {
        private string color;
        public string Color
        {
            get { return color; }
            set { this.SetProperty(ref  color , value); }
        }
        private BitmapImage icon;
        public BitmapImage Icon
        {
            get { return icon; }
            set { this.SetProperty(ref icon, value); }
        }
        private BitmapImage checkedIcon;
        public BitmapImage CheckedIcon
        {
            get { return checkedIcon; }
            set { this.SetProperty(ref checkedIcon, value); }
        }
    }
}
