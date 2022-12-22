using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Note.Services.ScreenShot
{
    public class CropTool
    {
        private readonly Canvas canvas;
        private readonly CropShape cropShape;
        //private readonly ShadeTool shadeService;
        private readonly ThumbTool thumbService;
        public double TopLeftX => Canvas.GetLeft(cropShape.Shape);
        public double TopLeftY => Canvas.GetTop(cropShape.Shape);
        public double BottomRightX => Canvas.GetLeft(cropShape.Shape) + cropShape.Shape.Width;
        public double BottomRightY => Canvas.GetTop(cropShape.Shape) + cropShape.Shape.Height;
        public double Height => cropShape.Shape.Height;
        public double Width => cropShape.Shape.Width;
        public CropService _service;
        public CropTool(Canvas canvas, CropService service)
        {
            this.canvas = canvas;
            this._service = service;
            cropShape = new CropShape(
                //边框主线
                new Rectangle
                {
                    Height = 0,
                    Width = 0,
                    Stroke = Brushes.White,
                    StrokeThickness = 2
                },
                //Dash
                new Rectangle
                {
                    //Stroke = Brushes.White,
                    //StrokeDashArray = new DoubleCollection(new double[] { 4, 4 })
                }
            );
            //shadeService = new ShadeTool(canvas, this);
            thumbService = new ThumbTool(canvas, this);
            this.canvas.Children.Add(cropShape.Shape);
            this.canvas.Children.Add(cropShape.DashedShape);
            //this.canvas.Children.Add(shadeService.ShadeOverlay);
            this.canvas.Children.Add(thumbService.TopLeft);
            this.canvas.Children.Add(thumbService.TopRight);
            this.canvas.Children.Add(thumbService.BottomLeft);
            this.canvas.Children.Add(thumbService.BottomRight);
            //this.canvas.Children.Add(textService.TextBlock);
        }
        public void Redraw(double newX, double newY, double newWidth, double newHeight)
        {
            cropShape.Redraw(newX, newY, newWidth, newHeight);
            //shadeService.Redraw();
            thumbService.Redraw();
            this._service.CropAreaChanged();
        }
    }
}
