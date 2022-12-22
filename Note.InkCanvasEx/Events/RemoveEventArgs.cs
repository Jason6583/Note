using Windows.UI.Input.Inking;
namespace Note.InkCanvasEx.Events
{
    public class RemoveStrokeEventArgs
    {
        public InkStroke RemovedStroke { get; set; }
        public RemoveStrokeEventArgs(InkStroke removedStroke)
        {
            RemovedStroke = removedStroke;
        }
    }
}
