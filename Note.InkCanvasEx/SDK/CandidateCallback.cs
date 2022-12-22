namespace Note.InkCanvasEx.SDK
{
    /// <summary>候选词回调</summary>
    public abstract class CandidateCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="jsonData"></param>
        public abstract void OnCandidate(Note.InkCanvasEx.SDK.Editor.Editor editor, string jsonData);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMsg"></param>
        public abstract void OnError(Note.InkCanvasEx.SDK.Editor.Editor editor, int errorCode, string errorMsg);
    }
}
