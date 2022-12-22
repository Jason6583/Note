using System;
using Note.InkCanvasEx.SDK.Accredit;
using Note.InkCanvasEx.SDK.Editor;
using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>识别引擎实现</summary>
    public class EngineNative : Engine
    {
        /// <summary>授权证书</summary>
        private string _certify;
        /// <summary>授权信息</summary>
        private VerifyInfo _verifyInfo;
        /// <summary>授权模块</summary>
        private VerifyModule _verifyModule;
        /// <summary>识别引擎对象句柄</summary>
        public long Handle { get; private set; }
        public bool IsValid { get; private set; } = false;
        /// <summary>创建引擎实例</summary>
        /// <param name="certify">授权证书</param>
        public EngineNative(string certify) => this._certify = certify;
        /// <summary>创建引擎实例</summary>
        /// <param name="verifyInfo">授权信息</param>
        public EngineNative(VerifyInfo verifyInfo)
        {
            this._verifyInfo = verifyInfo;
            this._verifyModule = new VerifyModule(this._verifyInfo);
        }
        /// <summary>创建并初始化引擎</summary>
        private void InitializeEngine()
        {
            this.Handle = LocalEngineNative.createEngine(this._certify);
            if (this.Handle == 0L)
            {
                this.IsValid = false;
            }
            else
            {
                this.IsValid = true;
                this._verifyModule?.VerifyAsync();
            }
        }
        /// <summary>创建编辑器实例</summary>
        /// <param name="engineParams">编辑器参数</param>
        /// <returns></returns>
        public override Note.InkCanvasEx.SDK.Editor.Editor CreateEditor(IParams engineParams)
        {
            long editor = LocalEngineNative.createEditor(this.Handle, engineParams.GetHandle());
            return editor != 0L ? new EditorNative(editor, this._verifyModule) : (Note.InkCanvasEx.SDK.Editor.Editor)null;
        }
        /// <summary>获取识别引擎版本</summary>
        /// <returns></returns>
        public override string GetEngineVersion() => this.IsValid ? Marshal.PtrToStringAnsi(LocalEngineNative.getEngineVersion(this.Handle)) : "";
        /// <summary>设置调试日志输出等级</summary>
        /// <param name="level">0:不打印log;1:只打印错误信息;2:打印错误信息和提示信息;3:打印调试信息、提示信息和错误信息</param>
        public override void SetDebugLevel(int level)
        {
            if (!this.IsValid)
                return;
            LocalEngineNative.setDebugLevel(this.Handle, level);
        }
        /// <summary>设置log文件路径,将log保存到指定路径</summary>
        /// <param name="dirPath"></param>
        public override void SetLogDir(string dirPath)
        {
            if (!this.IsValid)
                return;
            LocalEngineNative.setLogDir(this.Handle, dirPath);
        }
        /// <summary>设置授权验证回调</summary>
        /// <param name="callback"></param>
        public override void SetOnVerifyCallback(Action<int, string> callback)
        {
            if (this._verifyModule == null)
                return;
            this._verifyModule.VerifyCallback = callback;
        }
        /// <summary>启动引擎</summary>
        public override void Start()
        {
            if (this.IsValid)
                this.Stop();
            this.InitializeEngine();
        }
        /// <summary>停止引擎</summary>
        public override void Stop()
        {
            if (!this.IsValid)
                return;
            LocalEngineNative.stopEngine(this.Handle);
            this.Handle = 0L;
        }
    }
}
