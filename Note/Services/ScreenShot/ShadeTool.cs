using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Note.Services.ScreenShot
{
    //遮罩层
    public class ShadeTool
    {
        private readonly CropTool cropTool;
        private readonly RectangleGeometry rectangleGeo;
        public Path ShadeOverlay { get; set; }
        public ShadeTool(Canvas canvas, CropTool cropTool)
        {
            this.cropTool = cropTool;
            ShadeOverlay = new Path
            {
                Fill = Brushes.Black,
                Opacity = 0.5
            };
            var geometryGroup = new GeometryGroup();
            RectangleGeometry geometry1 =
                new RectangleGeometry(new Rect(new Size(canvas.Width, canvas.Height)));
            rectangleGeo = new RectangleGeometry(
                new Rect(
                    this.cropTool.TopLeftX,
                    this.cropTool.TopLeftY,
                    this.cropTool.Width,
                    this.cropTool.Height
                )
            );
            geometryGroup.Children.Add(geometry1);
            geometryGroup.Children.Add(rectangleGeo);
            ShadeOverlay.Data = geometryGroup;
        }
        public void Redraw()
        {
            rectangleGeo.Rect = new Rect(
                cropTool.TopLeftX,
                cropTool.TopLeftY,
                cropTool.Width,
                cropTool.Height
            );
        }
    }
}
