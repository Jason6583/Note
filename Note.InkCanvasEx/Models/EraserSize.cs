using Note.InkCanvasEx.Commons;
using Windows.UI.Xaml.Media.Imaging;

namespace Note.InkCanvasEx.Models
{
    public class EraserSize : BindableBase
    {
        private double size;
        public double Size
        {
            get => this.size;
            set => this.SetProperty(ref this.size, value);
        }
        private double iconSize;
        public double IconSize
        {
            get => this.iconSize;
            set => this.SetProperty(ref this.iconSize, value);
        }
        private BitmapImage icon;
        public BitmapImage Icon
        {
            get => this.icon;
            set => this.SetProperty(ref this.icon, value);
        }
        private BitmapImage checkedIcon;
        public BitmapImage CheckedIcon
        {
            get => this.checkedIcon;
            set => this.SetProperty(ref this.checkedIcon, value);
        }
    }
}
