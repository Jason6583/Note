using Note.InkCanvasEx.Commons;

namespace Note.Models
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
    }
}
