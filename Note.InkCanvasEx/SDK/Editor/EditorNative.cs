using System;
using System.Text;
using Note.InkCanvasEx.SDK.Accredit;
using Note.InkCanvasEx.SDK.Native;
using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Editor
{
    /// <summary>
    /// Description
    /// Author: EInkNote
    /// CreateDate: 2022/3/28 16:16:08
    /// Version:
    /// </summary>
    public class EditorNative : Editor
    {
        private LoadCallback _onLoadedCallback;
        private MsgCallback _onErrorCallback;
        private MsgCallback _onContentChangedCallback;
        private MsgCallback _onCandidateChangedCallback;
        private MsgCallback _onAssociationalChangedCallback;
        private Encoding utf8 = Encoding.UTF8;
        private VerifyModule _verifyModule;

        /// <summary>编辑器构造函数</summary>
        public EditorNative(long handle) => this._handle = handle;
        public EditorNative(long handle, VerifyModule verifyModule)
          : this(handle)
        {
            this._verifyModule = verifyModule;
        }
        public override void SetOnLoaded(Action<Editor> callback)
        {
            this._onLoadedCallback = () =>
            {
                Action<Editor> action = callback;
                if (action == null)
                    return;
                action(this);
            };
            LocalEditorNative.setOnLoadedCallback(this._handle, this._onLoadedCallback);
        }
        public override void SetOnError(Action<Editor, int, string> callback)
        {
            this._onErrorCallback = (intPtr, len) =>
            {
                Action<Editor, int, string> action = callback;
                if (action == null)
                    return;
                action(this, 0, this.ToUtf8String(intPtr, len));
            };
            LocalEditorNative.setOnError(this._handle, this._onErrorCallback);
        }
        public override void SetOnContentChanged(Action<Editor, string> callback)
        {
            this._onContentChangedCallback = (intPtr, len) =>
            {
                Action<Editor, string> action = callback;
                if (action == null)
                    return;
                action(this, this.ToUtf8String(intPtr, len));
            };
            LocalEditorNative.setOnContentChanged(this._handle, this._onContentChangedCallback);
        }
        public override void SetOnCandidateChanged(Action<Editor, string> callback)
        {
            this._onCandidateChangedCallback = (intPtr, len) =>
            {
                Action<Editor, string> action = callback;
                if (action == null)
                    return;
                action(this, this.ToUtf8String(intPtr, len));
            };
            LocalEditorNative.setOnCandidateChanged(this._handle, this._onCandidateChangedCallback);
        }
        public override void SetOnAssociationalChanged(Action<Editor, string> callback)
        {
            this._onAssociationalChangedCallback = (intPtr, len) =>
            {
                Action<Editor, string> action = callback;
                if (action == null)
                    return;
                action(this, this.ToUtf8String(intPtr, len));
            };
            LocalEditorNative.setOnAssociationalChanged(this._handle, this._onAssociationalChangedCallback);
        }
        private string ToUtf8String(IntPtr intPtr, int len)
        {
            byte[] data = new byte[len];
            Marshal.Copy(intPtr, data, 0, len);
            return this.utf8.GetString(data);
        }
    }
}
