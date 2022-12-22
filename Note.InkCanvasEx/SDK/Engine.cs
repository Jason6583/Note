using System;
using Note.InkCanvasEx.SDK.Accredit;
using Note.InkCanvasEx.SDK.Native;

namespace Note.InkCanvasEx.SDK
{
    /// <summary>识别引擎对象</summary>
    public abstract class Engine
    {
        /// <summary>启动引擎</summary>
        public abstract void Start();
        /// <summary>停止引擎</summary>
        public abstract void Stop();
        /// <summary>创建引擎实例</summary>
        /// <param name="certify">授权证书</param>
        /// <returns>返回一个引擎对象,创建失败时返回null</returns>
        public static Engine CreateInstance(string certify) => (Engine)new EngineNative(certify);
        /// <summary>创建引擎实例</summary>
        /// <param name="verifyInfo">授权信息</param>
        /// <returns>返回一个引擎对象,创建失败时返回null</returns>
        public static Engine CreateInstance(VerifyInfo verifyInfo) => (Engine)new EngineNative(verifyInfo);
        /// <summary>创建编辑器参数设置对象</summary>
        /// <returns></returns>
        public static IParams CreateEditorParams() => (IParams)new Params();
        /// <summary>创建编辑器对象</summary>
        /// <param name="engineParams">编辑器参数</param>
        /// <returns></returns>
        public abstract Note.InkCanvasEx.SDK.Editor.Editor CreateEditor(IParams engineParams);
        /// <summary>设置调试日志输出等级</summary>
        /// <param name="level">0:不打印log;1:只打印错误信息;2:打印错误信息和提示信息;3:打印调试信息、提示信息和错误信息</param>
        public abstract void SetDebugLevel(int level);
        /// <summary>设置log文件路径,将log保存到指定路径</summary>
        /// <param name="dirPath"></param>
        public abstract void SetLogDir(string dirPath);
        /// <summary>获得引擎版本信息</summary>
        /// <returns></returns>
        public abstract string GetEngineVersion();
        /// <summary>设置授权验证回调</summary>
        /// <param name="callback"></param>
        public abstract void SetOnVerifyCallback(Action<int, string> callback);
    }
}
