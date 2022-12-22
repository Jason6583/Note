using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.Controls;
using System.Collections.Generic;
using Windows.UI.Input.Inking.Analysis;
using Note.InkCanvasEx.Services.Ink.UndoRedo;

namespace Note.InkCanvasEx.Services.Ink
{
    //识别算法
    public class InkTransformService : BindableBase
    {
        public bool IsSmartPenMode { get; set; } = false;
        private readonly Canvas _shapeCanvas;
        private readonly InkCanvas _inkCanvas;
        private readonly InkStrokesService _strokeService;
        private InkUndoRedoService _undoRedoService;
        private IEventBus _eventBus;
        public InkTransformService(InkCanvas inkCanvas, Canvas shapeCanvas, InkStrokesService strokeService, InkUndoRedoService undoRedoService,IEventBus eventBus)
        {
            _inkCanvas = inkCanvas;
            _shapeCanvas = shapeCanvas;
            _strokeService = strokeService;
            _undoRedoService = undoRedoService;
            _eventBus = eventBus;
        }
        public IEnumerable<UIElement> GetTextAndShapes() => _shapeCanvas.Children;
        public void AddUIElement(UIElement element) => _shapeCanvas.Children.Add(element);
        public void RemoveUIElement(UIElement element)
        {
            if (_shapeCanvas.Children.Contains(element))
            {
                _shapeCanvas.Children.Remove(element);
            }
        }
        public bool HasTextAndShapes() => _shapeCanvas.Children.Any();
        public void ClearTextAndShapes()
        {
            _shapeCanvas.Children.Clear();
        }
        //添加文字
        public UIElement DrawText(string recognizedText)
        {
            TextContainer textContainer = new TextContainer();
            textContainer.UpdateText(recognizedText, 36);
            Canvas.SetTop(textContainer, 50);
            Canvas.SetLeft(textContainer, 50);
            _shapeCanvas.Children.Add(textContainer);
            textContainer.CloseHandler = (s, e) => 
            { 
                _shapeCanvas.Children.Remove(textContainer);
                _eventBus.Publish(new ClearTextEventArgs());               
            };
            var undoList = new List<UIElement>();
            undoList.Add(textContainer);
            _undoRedoService.AddOperation(new AddTextAndShapeUndoOperation(undoList, this));
            return textContainer;
        }
        public void ClearText()
        {
            var texts = this._shapeCanvas.Children.OfType<TextContainer>();
            foreach(var text in texts)
            {
                this._shapeCanvas.Children.Remove(text);
            }
        }
        //添加图形
        public UIElement DrawShape(Path shape,string shapeStr,Rect rect)
        {
            var shapeContainer=new ShapeContainer(shape,shapeStr,rect);
            Canvas.SetTop(shapeContainer, rect.Top);
            Canvas.SetLeft(shapeContainer, rect.Left);
            _shapeCanvas.Children.Add(shapeContainer);
            shapeContainer.CloseHandler += (s, e) => { _shapeCanvas.Children.Remove(shapeContainer); };
            var undoList = new List<UIElement>();
            undoList.Add(shapeContainer);
            _undoRedoService.AddOperation(new AddTextAndShapeUndoOperation(undoList, this));
            return shapeContainer;
        }
        //作图椭圆
        private UIElement DrawEllipse(InkAnalysisInkDrawing shape)
        {
            var points = shape.Points;
            Ellipse ellipse = new Ellipse();
            ellipse.Width = Math.Sqrt(((points[0].X - points[2].X) * (points[0].X - points[2].X)) +
                 ((points[0].Y - points[2].Y) * (points[0].Y - points[2].Y)));
            ellipse.Height = Math.Sqrt(((points[1].X - points[3].X) * (points[1].X - points[3].X)) +
                 ((points[1].Y - points[3].Y) * (points[1].Y - points[3].Y)));
            var rotAngle = Math.Atan2(points[2].Y - points[0].Y, points[2].X - points[0].X);
            RotateTransform rotateTransform = new RotateTransform();
            rotateTransform.Angle = rotAngle * 180 / Math.PI;
            rotateTransform.CenterX = ellipse.Width / 2.0;
            rotateTransform.CenterY = ellipse.Height / 2.0;
            TranslateTransform translateTransform = new TranslateTransform();
            translateTransform.X = shape.Center.X - (ellipse.Width / 2.0);
            translateTransform.Y = shape.Center.Y - (ellipse.Height / 2.0);
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(rotateTransform);
            transformGroup.Children.Add(translateTransform);
            ellipse.RenderTransform = transformGroup;
            var brush = new SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 255));
            ellipse.Stroke = brush;
            ellipse.StrokeThickness = 2;
            _shapeCanvas.Children.Add(ellipse);
            return ellipse;
        }
        //作图多边形
        private UIElement DrawPolygon(InkAnalysisInkDrawing shape)
        {
            var points = shape.Points;
            Polygon polygon = new Polygon();
            foreach (var point in points)
            {
                polygon.Points.Add(point);
            }
            var brush = new SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 255));
            polygon.Stroke = brush;
            polygon.StrokeThickness = 2;
            _shapeCanvas.Children.Add(polygon);
            return polygon;
        }
    }
}
