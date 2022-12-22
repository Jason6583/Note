using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Note.Services.ScreenShot
{
    public class CropAdorner : Adorner
    {
        private readonly Canvas overlayCanvas;
        private readonly VisualCollection visualCollection;
        public CropAdorner(UIElement adornedElement, Canvas overlayCanvas) : base(adornedElement)
        {
            this.overlayCanvas = overlayCanvas;
            visualCollection = new VisualCollection(this);
            visualCollection.Add(this.overlayCanvas);
        }
        protected override int VisualChildrenCount => visualCollection.Count;
        protected override Visual GetVisualChild(int index) => visualCollection[index];
        protected override Size ArrangeOverride(Size size)
        {
            Size finalSize = base.ArrangeOverride(size);
            overlayCanvas.Arrange(new Rect(0, 0, AdornedElement.RenderSize.Width, AdornedElement.RenderSize.Height));
            return finalSize;
        }
    }
}
