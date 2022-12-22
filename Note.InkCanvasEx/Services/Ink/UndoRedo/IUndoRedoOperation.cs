namespace Note.InkCanvasEx.Services.Ink.UndoRedo
{
    public interface IUndoRedoOperation
    {
        void ExecuteUndo();
        void ExecuteRedo();
    }
}
