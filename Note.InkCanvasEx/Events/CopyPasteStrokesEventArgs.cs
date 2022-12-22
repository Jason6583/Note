using System.Collections.Generic;
using Windows.UI.Input.Inking;

namespace Note.InkCanvasEx.Events
{
    public class CopyPasteStrokesEventArgs
    {
        public IEnumerable<InkStroke> Strokes { get; set; }
        public CopyPasteStrokesEventArgs(IEnumerable<InkStroke> strokes)
        {
            Strokes = strokes;
        }
    }
}
