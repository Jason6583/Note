using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Input.Inking;
namespace Note.InkCanvasEx.Events
{
    public class MoveStrokesEventArgs
    {
        public Point Offset { get; set; }
        public IEnumerable<InkStroke> Strokes { get; set; }
        public MoveStrokesEventArgs(IEnumerable<InkStroke> strokes, Point offset)
        {
            Strokes = strokes;
            Offset = offset;
        }
    }
}
