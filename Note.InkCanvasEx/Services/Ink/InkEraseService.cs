using Windows.UI;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;
using Note.InkCanvasEx.Services.Ink.UndoRedo;

namespace Note.InkCanvasEx.Services.Ink
{
    public class InkEraseService 
    {
        private InkCanvas inkCanvas;
        private Canvas shapeCanvas;
        private InkUndoRedoService undoRedoService;
        private InkTransformService transformService;
        public Ellipse Ellipse { get; set; } = new Ellipse()
        {
            Fill = new SolidColorBrush(new Color() { A = 255, B = 242, G = 242, R = 242 }),
            Stroke = new SolidColorBrush(new Color() { A = 255, B = 217, G = 217, R = 217 }),
            Width = 50,
            Height = 50,
            StrokeThickness = 1.5,
            StrokeDashArray = new DoubleCollection() { 2, 2 },
        };
        public InkEraseService(InkCanvas inkCanvas,Canvas shapeCanvas, InkUndoRedoService undoRedoService, InkTransformService transformService)
        {
            this.inkCanvas = inkCanvas;
            this.shapeCanvas = shapeCanvas;
            this.undoRedoService = undoRedoService;
            this.transformService = transformService;
        }
        private double eraserWidth = 10;
        public double EraserWidth 
        { 
            get=>this.eraserWidth;
            set
            {
                this.eraserWidth = value;
                this.Ellipse.Width= value;
                this.Ellipse.Height = value;
            }
        }
        public void StartEraserSelectionConfig()
        {
            inkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.None;
            inkCanvas.InkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.LeaveUnprocessed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerPressed += UnprocessedInput_PointerPressed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerMoved += UnprocessedInput_PointerMoved;
            inkCanvas.InkPresenter.UnprocessedInput.PointerReleased += UnprocessedInput_PointerReleased;
        }
        public void StopEraserSelectionConfig()
        {
            inkCanvas.InkPresenter.InputProcessingConfiguration.Mode = InkInputProcessingMode.Inking;
            inkCanvas.InkPresenter.InputProcessingConfiguration.RightDragAction = InkInputRightDragAction.AllowProcessing;
            inkCanvas.InkPresenter.UnprocessedInput.PointerPressed -= UnprocessedInput_PointerPressed;
            inkCanvas.InkPresenter.UnprocessedInput.PointerMoved -= UnprocessedInput_PointerMoved;
            inkCanvas.InkPresenter.UnprocessedInput.PointerReleased -= UnprocessedInput_PointerReleased;
        }
        private void UnprocessedInput_PointerReleased(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (this.shapeCanvas.Children.Contains(Ellipse))
            {
                this.shapeCanvas.Children.Remove(Ellipse);
            }
        }
        private void UnprocessedInput_PointerPressed(InkUnprocessedInput sender, PointerEventArgs args)
        {
            if (this.shapeCanvas.Children.Contains(Ellipse))
            {
                this.shapeCanvas.Children.Remove(Ellipse);
            }
            this.shapeCanvas.Children.Add(Ellipse);
            var transformX = (float)args.CurrentPoint.RawPosition.X - 0.5 * EraserWidth;
            var transformY = (float)args.CurrentPoint.RawPosition.Y - 0.5 * EraserWidth;
            var translate = new TranslateTransform();
            translate.X = transformX;
            translate.Y = transformY;
            Ellipse.RenderTransform = translate;
        }
        private void UnprocessedInput_PointerMoved(InkUnprocessedInput sender, PointerEventArgs args)
        {
            var transformX = (float)args.CurrentPoint.RawPosition.X - 0.5 * EraserWidth;
            var transformY = (float)args.CurrentPoint.RawPosition.Y - 0.5 * EraserWidth;
            var translate = new TranslateTransform();
            translate.X = transformX;
            translate.Y = transformY;
            Ellipse.RenderTransform = translate;
            Point p1 = new Point()
            {
                X = args.CurrentPoint.RawPosition.X - 0.5 * EraserWidth,
                Y = args.CurrentPoint.RawPosition.Y - 0.5 * EraserWidth,
            };
            Point p2 = new Point()
            {
                X = args.CurrentPoint.RawPosition.X + 0.5 * EraserWidth,
                Y = args.CurrentPoint.RawPosition.Y - 0.5 * EraserWidth,
            };
            Point p3 = new Point()
            {
                X = args.CurrentPoint.RawPosition.X + 0.5 * EraserWidth,
                Y = args.CurrentPoint.RawPosition.Y + 0.5 * EraserWidth,
            };
            Point p4 = new Point()
            {
                X = args.CurrentPoint.RawPosition.X - 0.5 * EraserWidth,
                Y = args.CurrentPoint.RawPosition.Y + 0.5 * EraserWidth,
            };
            inkCanvas.InkPresenter.StrokeContainer.SelectWithLine(p1, p2);
            inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            inkCanvas.InkPresenter.StrokeContainer.SelectWithLine(p2, p3);
            inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            inkCanvas.InkPresenter.StrokeContainer.SelectWithLine(p3, p4);
            inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            inkCanvas.InkPresenter.StrokeContainer.SelectWithLine(p4, p1);
            inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            this.EraseShapeAndText(args);
            EraseSmallPoint(args.CurrentPoint.RawPosition, Ellipse.Width);
        }
        private void EraseSmallPoint(Point pt, double radius)
        {
            //用于删除点：
            var strokes = inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            var selectedStrokes = SelectWithRect(pt, strokes);
            var left = pt.X - radius;
            var top = pt.Y - radius;
            var right = pt.X + radius;
            var bottom = pt.Y + radius;
            var selectedCount = selectedStrokes.Count();
            for (int i = 0; i < selectedCount; i++)
            {
                var selectedStroke = selectedStrokes[i];
                var boundingRect = selectedStroke.BoundingRect;
                if (boundingRect.X >= left &&
                    boundingRect.X <= right &&
                    boundingRect.Y >= top &&
                    boundingRect.Y <= bottom &&
                    boundingRect.Width < Ellipse.Width &&
                    boundingRect.Height < Ellipse.Width)
                {
                    selectedStroke.Selected = true;
                    inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
                }
            }
        }
        public List<InkStroke> SelectWithRect(Point pt, IEnumerable<InkStroke> strokes)
        {
            double radius = Ellipse.Width / 2;
            var left = pt.X - radius;
            var top = pt.Y - radius;
            var rect = new Rect(left, top, Ellipse.Width, Ellipse.Width);
            List<InkStroke> SelectedStrokes = new List<InkStroke>();
            foreach (InkStroke stroke in strokes)
            {
                rect.Intersect(stroke.BoundingRect);
                if (!rect.IsEmpty && rect.Width > 0 && rect.Height > 0)
                {
                    SelectedStrokes.Add(stroke);
                }
                rect = new Rect(left, top, Ellipse.Width, Ellipse.Width);
            }
            return SelectedStrokes;
        }
        private void EraseShapeAndText(PointerEventArgs args)
        {
            var elements = this.shapeCanvas.Children.OfType<FrameworkElement>().ToList();
            var textAndShapes = new List<FrameworkElement>();
            foreach (var element in elements)
            {
                var left = Canvas.GetLeft(element);
                var top = Canvas.GetTop(element);
                var width = element.ActualWidth;
                var height = element.ActualHeight;
                var rect = new Rect(left, top, width, height);
                if (rect.Contains(args.CurrentPoint.Position))
                {
                    textAndShapes.Add(element);
                    shapeCanvas.Children.Remove(element);
                }
            }
            if (textAndShapes.Count > 0)
            {
                undoRedoService.AddOperation(new RemoveTextShapeUndoRedoOperation(textAndShapes, transformService));
            }
        }
    }
}
