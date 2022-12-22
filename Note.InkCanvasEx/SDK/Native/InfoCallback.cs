using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>消息通知的回调</summary>
    /// <param name="data"></param>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void InfoCallback(string data);
}
