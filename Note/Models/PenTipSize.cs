using Note.InkCanvasEx.Commons;

namespace Note.Models
{
    public class PenTipSize : BindableBase
    {
        private double penSize;
        public double PenSize
        {
            get => this.penSize;
            set => this.SetProperty(ref this.penSize, value);
        }

        private double iconSize;
        public double IconSize
        {
            get => this.iconSize;
            set => this.SetProperty(ref this.iconSize, value);
        }
    }
}
