using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>加载完成的回调</summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void LoadCallback();
}
