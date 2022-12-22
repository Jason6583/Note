using System;
using Windows.UI;
using System.Linq;
using Windows.UI.Xaml;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Note.InkCanvasEx.Services.Ink
{
    public class InkSelectionRectangleService
    {
        private const string SelectionRectName = "selectionRectangle";
        private readonly Canvas _selectionCanvas;
        private readonly InkCanvas _inkCanvas;
        private readonly InkStrokesService _strokeService;
        private Rect selectionStrokesRect = Rect.Empty;
        private Image closeImage = new Image();
        public InkSelectionRectangleService(InkCanvas inkCanvas, Canvas selectionCanvas, InkStrokesService strokeService)
        {
            _inkCanvas = inkCanvas;
            _selectionCanvas = selectionCanvas;
            _strokeService = strokeService;
            _inkCanvas.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            _inkCanvas.ManipulationStarted += InkCanvas_ManipulationStarted;
            _inkCanvas.ManipulationDelta += InkCanvas_ManipulationDelta;
            _inkCanvas.ManipulationCompleted += InkCanvas_ManipulationCompleted;
            CreateCloseImage();
        }
        public void UpdateSelectionRect(Rect rect)
        {
            selectionStrokesRect = rect;
            var selectionRect = GetSelectionRectangle();
            if (!_selectionCanvas.Children.Contains(closeImage))
            {
                _selectionCanvas.Children.Add(closeImage);
            }
            selectionRect.Width = rect.Width;
            selectionRect.Height = rect.Height;
            Canvas.SetLeft(selectionRect, rect.Left);
            Canvas.SetTop(selectionRect, rect.Top);
            var right = rect.Left + selectionRect.Width - closeImage.Width;
            Canvas.SetLeft(closeImage, right);
            Canvas.SetTop(closeImage, rect.Top);
        }
        public void Clear()
        {
            selectionStrokesRect = Rect.Empty;
            _selectionCanvas.Children.Clear();
        }
        public bool ContainsPosition(Point position)
        {
            return !selectionStrokesRect.IsEmpty && RectHelper.Contains(selectionStrokesRect, position);
        }
        private Rectangle GetSelectionRectangle()
        {
            var selectionRectangle = _selectionCanvas.Children
                .FirstOrDefault(f => f is Rectangle r && r.Name == SelectionRectName)
                as Rectangle;
            if (selectionRectangle == null)
            {
                selectionRectangle = CreateNewSelectionRectangle();
                _selectionCanvas.Children.Add(selectionRectangle);
            }
            return selectionRectangle;
        }
        private void CreateCloseImage()
        {
            closeImage = new Image();
            BitmapImage bitmap = new BitmapImage(new Uri("ms-appx:///Assets/ElementClose.png", UriKind.Absolute));
            closeImage.Source = bitmap;
            closeImage.Width = 24;
            closeImage.Height = 24;
            closeImage.HorizontalAlignment = HorizontalAlignment.Center;
            closeImage.VerticalAlignment = VerticalAlignment.Center;   
            closeImage.Tapped += (s, e) =>
            {
                _selectionCanvas.Children.Remove(closeImage);
                var selectionRectangle = _selectionCanvas.Children
                    .FirstOrDefault(f => f is Rectangle r && r.Name == SelectionRectName)
                    as Rectangle;
                if (selectionRectangle != null)
                    _selectionCanvas.Children.Remove(selectionRectangle);
                _inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            };
            closeImage.HorizontalAlignment = HorizontalAlignment.Right;
            closeImage.VerticalAlignment = VerticalAlignment.Top;
            closeImage.PointerEntered += (s, e) => { Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Hand, 3); };
            closeImage.PointerExited += (s, e) => { Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 0); };
        }
        private Rectangle CreateNewSelectionRectangle()
        {
            return new Rectangle()
            {
                Name = SelectionRectName,
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0X5E, 0X75, 0XFD)),
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection() { 2, 2 },
                StrokeDashCap = PenLineCap.Round
            };
        }
        private void InkCanvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }
        private void InkCanvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!selectionStrokesRect.IsEmpty)
            {
                Point offset = new Point();
                offset.X = e.Delta.Translation.X;
                offset.Y = e.Delta.Translation.Y;
                MoveSelection(offset);
            }
        }
        private void InkCanvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }
        private void MoveSelection(Point offset)
        {
            var selectionRect = GetSelectionRectangle();
            if(!_selectionCanvas.Children.Contains(closeImage))
            {
                _selectionCanvas.Children.Add(closeImage);
            }
            var left = Canvas.GetLeft(selectionRect);
            var top = Canvas.GetTop(selectionRect);
            Canvas.SetLeft(selectionRect, left + offset.X);
            Canvas.SetTop(selectionRect, top + offset.Y);
            var right = left + selectionRect.Width - closeImage.Width;
            Canvas.SetLeft(closeImage, right);
            Canvas.SetTop(closeImage, top + offset.Y);
            selectionStrokesRect.X = left + offset.X;
            selectionStrokesRect.Y = top + offset.Y;
            _strokeService.MoveSelectedStrokes(offset);
        }
    }
}
