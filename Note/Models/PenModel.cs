using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Enums;
using Windows.UI.Input.Inking;
using System.Windows.Media.Imaging;

namespace Note.Models
{
    public class PenModel : BindableBase
    {
        private string pen;
        public string Pen
        {
            get => this.pen;
            set => this.SetProperty(ref this.pen, value);
        }

        private BitmapImage image;
        public BitmapImage Image
        {
            get => this.image;
            set => this.SetProperty(ref image, value);
        }

        private BitmapImage checkedImage;
        public BitmapImage CheckedImage
        {
            get => this.checkedImage;
            set => this.SetProperty(ref this.checkedImage, value);
        }

        private InkDrawingAttributes inkDrawingAttribute;
        public InkDrawingAttributes InkDrawingAttributes
        {
            get => this.inkDrawingAttribute;
            set => this.SetProperty(ref this.inkDrawingAttribute, value);
        }

        private PenType penType;
        public PenType PenType
        {
            get => this.penType;
            set => this.SetProperty(ref this.penType, value);
        }
    }
}
