using System;
using System.Text;

namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>编辑器</summary>
    public class Params : IParams
    {
        private Encoding utf8 = Encoding.UTF8;
        /// <summary>编辑器句柄</summary>
        private long handle;
        /// <summary>公式编辑器</summary>
        private ICalculateParams calculateEngineParams;
        public Params() => this.handle = LocalParamsNative.createNewParams();
        /// <summary>实例化编辑器，含公式编辑器</summary>
        /// <param name="handle"></param>
        public Params(long handle)
        {
            this.handle = handle != 0L ? handle : throw new ArgumentException("Invalidate handle");
            this.calculateEngineParams = new CalculateParams(this.handle);
        }
        /// <summary>获取编辑器句柄</summary>
        /// <returns></returns>
        public long GetHandle() => this.handle;
        /// <summary>获取公式编辑器对象</summary>
        /// <returns></returns>
        public ICalculateParams GetCalculateParams() => this.calculateEngineParams;
        /// <summary>获取识别模式</summary>
        /// <returns></returns>
        public RecognitionMode GetMode() => (RecognitionMode)LocalParamsNative.getMode(this.handle);
        /// <summary>设置配置文件名称</summary>
        /// <param name="configName"></param>
        public void SetConfigName(string configName) => LocalParamsNative.setConfigName(this.handle, configName);
        /// <summary>设置识别资源文件夹，包括配置文件、字典信息等</summary>
        /// <param name="path"></param>
        public void SetDataDir(string path) => LocalParamsNative.setDataDir(this.handle, this.utf8.GetString(Encoding.Convert(Encoding.Default, this.utf8, Encoding.Default.GetBytes(path))));
        /// <summary>设置引擎识别模式，默认为边写边识别</summary>
        /// <param name="mode"></param>
        public void SetMode(RecognitionMode mode) => LocalParamsNative.setMode(this.handle, (int)mode);
        /// <summary>设置结果内容是否包含联想词，默认false</summary>
        /// <param name="contained"></param>
        public void SetResultAssociational(bool contained) => LocalParamsNative.setResultAssociational(this.handle, contained);
        /// <summary>设置结果内容是否包含候选词，默认false</summary>
        /// <param name="contained"></param>
        public void SetResultCandidate(bool contained) => LocalParamsNative.setResultCandidate(this.handle, contained);
        /// <summary>设置结果内容是否包含坐标信息，默认false</summary>
        /// <param name="coordinate"></param>
        public void SetResultCoordinate(bool coordinate) => LocalParamsNative.setResultCoordinate(this.handle, coordinate);
        /// <summary>设置结果内容是否包含分割框，默认false</summary>
        /// <param name="coordinate"></param>
        public void SetResultPartitionCoordinate(bool coordinate) => LocalParamsNative.setResultPartitionCoordinate(this.handle, coordinate);
        /// <summary>设置分词的时间间隔</summary>
        /// <param name="timeLot"></param>
        public void SetWordSplitTimeLot(long timeLot) => LocalParamsNative.setWordSplitTimeLot(this.handle, timeLot);
        /// <summary>跨行后处理开关</summary>
        /// <param name="processedEnable"></param>
        public void SetResultSpanProcessed(bool processedEnable) => LocalParamsNative.setResultSpanProcessed(this.handle, processedEnable);
    }
}
