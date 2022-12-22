using System;

namespace Note.InkCanvasEx.Events
{
    public class RecogizeEventArgs : EventArgs
    {
        public string Result { get; set; }
        public RecogizeEventArgs(string result)
        {
            Result = result;
        }
    }
}
