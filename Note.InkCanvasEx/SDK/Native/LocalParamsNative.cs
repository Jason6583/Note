using System.Runtime.InteropServices;
namespace Note.InkCanvasEx.SDK.Native
{
    public static class LocalParamsNative
    {
        /// <summary>创建一个配置对象</summary>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern long createNewParams();

        /// <summary>设置配置文件名称</summary>
        /// <param name="handle"></param>
        /// <param name="configName"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setConfigName(long handle, string configName);

        /// <summary>设置识别资源文件夹，包括配置文件、字典等信息</summary>
        /// <param name="handle"></param>
        /// <param name="path"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setDataDir(long handle, string path);
        /// <summary>设置引擎识别模式，默认为边写边识别</summary>
        /// <param name="handle"></param>
        /// <param name="mode"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setMode(long handle, int mode);
        /// <summary>获取当前设置的模式</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int getMode(long handle);
        /// <summary>设置结果内容是否包含坐标信息，默认不包含</summary>
        /// <param name="handle"></param>
        /// <param name="contained"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setResultCoordinate(long handle, bool contained);
        /// <summary>
        /// 设置结果内容是否包含联想词，默认不包含
        /// 该接口为预留接口，功能暂未实现
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="contained"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setResultAssociational(long handle, bool contained);
        /// <summary>
        /// 设置结果内容是否包含候选词，默认不包含
        /// 该接口为预留接口，功能暂未实现
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="contained"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setResultCandidate(long handle, bool contained);
        /// <summary>设置结果内容是否包含分割框，默认不包含</summary>
        /// <param name="handle"></param>
        /// <param name="coordinate"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setResultPartitionCoordinate(long handle, bool coordinate);
        /// <summary>设置分词的时间间隔</summary>
        /// <param name="handle"></param>
        /// <param name="timeLot"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setWordSplitTimeLot(long handle, long timeLot);
        /// <summary>设置是否需要公式计算结果</summary>
        /// <param name="handle"></param>
        /// <param name="calculate"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setResultCalculate(long handle, bool calculate);
        /// <summary>目前支持的参数设置选项： 弧度（Radian)、角度(Degree)</summary>
        /// <param name="handle"></param>
        /// <param name="optStr"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setMathEngineRadianOrDegree(long handle, string optStr);
        /// <summary>设置计算结果取值方式</summary>
        /// <param name="handle"></param>
        /// <param name="roundingMode"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setMathResultRoundingMode(long handle, int roundingMode);
        /// <summary>设置保留小数后位数</summary>
        /// <param name="handle"></param>
        /// <param name="scale"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setMathResultScale(long handle, int scale);
        /// <summary>跨行后处理开关</summary>
        /// <param name="handle"></param>
        /// <param name="processedEnable">true打开，false关闭</param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setResultSpanProcessed(long handle, bool processedEnable);
    }
}
