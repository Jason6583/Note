using System;
using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>消息通知的回调</summary>
    /// <param name="intPtr"></param>
    /// <param name="strLen"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MsgCallback(IntPtr intPtr, int strLen);
}
