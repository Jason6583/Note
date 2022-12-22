using Note.InkCanvasEx.Commons;

namespace Note.Models
{
    public class PenColor : BindableBase
    {
        private string color;
        public string Color
        {
            get => this.color;
            set => this.SetProperty(ref color, value);
        }
    }
}
