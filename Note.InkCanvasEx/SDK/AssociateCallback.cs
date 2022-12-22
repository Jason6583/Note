namespace Note.InkCanvasEx.SDK
{
    /// <summary>联想词回调</summary>
    public abstract class AssociateCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="list"></param>
        public abstract void OnAssociate(Note.InkCanvasEx.SDK.Editor.Editor editor, string list);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorMsg"></param>
        public abstract void OnError(Note.InkCanvasEx.SDK.Editor.Editor editor, int errorCode, string errorMsg);
    }
}
