using Windows.Foundation;
using Note.InkCanvasEx.Events;
using Windows.UI.Input.Inking;
using System.Collections.Generic;

namespace Note.InkCanvasEx.Services.Ink.UndoRedo
{
    public class MoveStrokesUndoRedoOperation : IUndoRedoOperation
    {
        private readonly Point _offset;
        private readonly List<InkStroke> _strokes;
        private readonly InkStrokesService _strokeService;
        public MoveStrokesUndoRedoOperation(IEnumerable<InkStroke> strokes, Point offset, InkStrokesService strokeService)
        {
            _strokes = new List<InkStroke>(strokes);
            _offset = offset;
            _strokeService = strokeService;
            _strokeService.AddStrokeEvent += StrokeService_AddStrokeEvent;
        }
        public void ExecuteRedo()
        {
            _strokeService.SelectStrokes(_strokes);
            _strokeService.MoveSelectedStrokes(_offset);
        }
        public void ExecuteUndo()
        {
            _strokeService.SelectStrokes(_strokes);
            var reverseOffset=new Point(-_offset.X, -_offset.Y);
            _strokeService.MoveSelectedStrokes(reverseOffset);
        }
        private void StrokeService_AddStrokeEvent(object sender, AddStrokeEventArgs e)
        {
            if (e.NewStroke == null)
            {
                return;
            }
            var removedStrokes = _strokes.RemoveAll(s => s.Id == e.OldStroke?.Id);
            if (removedStrokes > 0)
            {
                _strokes.Add(e.NewStroke);
            }
        }
    }
}
