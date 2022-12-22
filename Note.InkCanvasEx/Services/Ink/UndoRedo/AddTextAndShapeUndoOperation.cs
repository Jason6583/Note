using Windows.UI.Xaml;
using System.Collections.Generic;

namespace Note.InkCanvasEx.Services.Ink.UndoRedo
{
    internal class AddTextAndShapeUndoOperation : IUndoRedoOperation
    {
        private readonly InkTransformService _transformService;
        private List<UIElement> _textAndShapes;
        public AddTextAndShapeUndoOperation(IEnumerable<UIElement> textAndShapes,
            InkTransformService transformService)
        {
            _textAndShapes = new List<UIElement>(textAndShapes);
            _transformService = transformService;
        }
        public void ExecuteRedo()
        {
            _textAndShapes.ForEach(s => _transformService.AddUIElement(s));
        }
        public void ExecuteUndo()
        {
            _textAndShapes.ForEach(s => _transformService.RemoveUIElement(s));
        }
    }
}
