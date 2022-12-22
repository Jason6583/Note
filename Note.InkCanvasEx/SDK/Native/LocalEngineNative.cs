using System;
using System.Runtime.InteropServices;
namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>识别引擎实现类</summary>
    public static class LocalEngineNative
    {
        /// <summary>初始化引擎对象</summary>
        /// <param name="certify"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern long createEngine(string certify);
        /// <summary>停止引擎</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern long stopEngine(long handle);
        /// <summary>通过给定的配置，创建一个编辑器</summary>
        /// <param name="engineHandle"></param>
        /// <param name="paramsHandle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern long createEditor(long engineHandle, long paramsHandle);
        /// <summary>设置调试的log等级</summary>
        /// <param name="engineHandle"></param>
        /// <param name="level"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setDebugLevel(long engineHandle, int level);
        /// <summary>设置log文件路径,将log保存到指定路径</summary>
        /// <param name="engineHandle"></param>
        /// <param name="dirPath"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setLogDir(long engineHandle, string dirPath);
        /// <summary>获得引擎版本信息</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr getEngineVersion(long handle);
    }
}
