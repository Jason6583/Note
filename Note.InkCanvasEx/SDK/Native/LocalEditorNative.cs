using System;
using System.Runtime.InteropServices;
namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>编辑器的实现类</summary>
    public static class LocalEditorNative
    {
        /// <summary>打开编辑器</summary>
        /// <param name="handle"></param>
        /// <param name="paramsHandle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int editoropen(long handle, long paramsHandle);
        /// <summary>关闭编辑器</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int editorclose(long handle);
        /// <summary>设置加载成功的回调函数</summary>
        /// <param name="handle">编辑器对象的句柄</param>
        /// <param name="loadCallback"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setOnLoadedCallback(long handle, LoadCallback loadCallback);
        /// <summary>设置返回识别结果的回调函数</summary>
        /// <param name="handle">编辑器对象的句柄</param>
        /// <param name="msgCallback">(*onContentChanged)(const char * contentJson)  识别结果contentJson为json字符串</param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setOnContentChanged(long handle, MsgCallback msgCallback);
        /// <summary>
        /// 设置错误信息的回调函数
        /// 暂未实现
        /// </summary>
        /// <param name="handle">编辑器对象的句柄</param>
        /// <param name="msgCallback"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setOnError(long handle, MsgCallback msgCallback);
        /// <summary>
        /// 设置联想词结果的回调函数
        /// 暂未实现
        /// </summary>
        /// <param name="handle">编辑器对象的句柄</param>
        /// <param name="msgCallback"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setOnAssociationalChanged(long handle, MsgCallback msgCallback);
        /// <summary>设置候选词的回调函数</summary>
        /// <param name="handle">编辑器对象的句柄</param>
        /// <param name="msgCallback"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setOnCandidateChanged(long handle, MsgCallback msgCallback);
        /// <summary>更新编辑器参数</summary>
        /// <param name="editorHandle">编辑器对象的句柄</param>
        /// <param name="paramsHandle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int updateParams(long editorHandle, long paramsHandle);
        /// <summary>
        /// 获取编辑器配置参数
        /// 当编辑器已经打开时获取编辑器配置参数，否则生成一个新的配置参数
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern long obtainParams(long handle);
        /// <summary>批量添加设别点信息，每条笔划通过落笔点来分开</summary>
        /// <param name="handle"></param>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="tCoordinate"></param>
        /// <param name="actionCoordinate"></param>
        /// <param name="xlen"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void addPoints(
          long handle,
          double[] xCoordinate,
          double[] yCoordinate,
          double[] tCoordinate,
          int[] actionCoordinate,
          long xlen);
        /// <summary>增加一条笔划的笔点信息</summary>
        /// <param name="handle"></param>
        /// <param name="xCoordinate"></param>
        /// <param name="yCoordinate"></param>
        /// <param name="tCoordinate"></param>
        /// <param name="xlen"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int addStrokePoints(
          long handle,
          double[] xCoordinate,
          double[] yCoordinate,
          double[] tCoordinate,
          long xlen);
        /// <summary>通过笔划的ID,删除一条笔划</summary>
        /// <param name="handle"></param>
        /// <param name="strokeId"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int deleteStrokePoints(long handle, int strokeId);
        /// <summary>
        /// 判断编辑器是否空闲
        /// 预留接口，暂未实现
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern bool isIdle(long handle);
        /// <summary>等待编辑器空闲</summary>
        /// <param name="handle"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void waitForIdle(long handle);
        /// <summary>检查是否已是关闭状态</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern bool isClosed(long handle);
        /// <summary>检查内容是否为空</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern bool isEmpty(long handle);
        /// <summary>获取当前已识别内容</summary>
        /// <param name="handle"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern string getContent(long handle);
        /// <summary>清除编辑器中的内容</summary>
        /// <param name="handle"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void clearContent(long handle);
        /// <summary>
        /// 更新识别结果
        /// 当识别结果中包含words的时候，可以根据识别情况，用备选词代替返回的结果。
        /// 此时可以把选择的备选词通过该方法传递回编辑器，以保证数据的一致性。
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="wordId"></param>
        /// <param name="candidate"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void updateRecognizeWord(long handle, int wordId, string candidate);
        /// <summary>在整篇识别模式RecognitionMode.MODE_ECR下，调用该方法触发识别</summary>
        /// <param name="handle"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void doPageRecognize(long handle);
        /// <summary>进行联想词匹配</summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void doAssociate(long handle, string text);
        /// <summary>公式计算接口</summary>
        /// <param name="handle"></param>
        /// <param name="strInputExp"></param>
        /// <param name="outSize"></param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr calculate(long handle, string strInputExp, out long outSize);
        /// <summary>获取公式计算引擎版本号接口</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern string getMathEngineVersion(long handle);
        /// <summary>把传入的latex字符串转换成MathML字符串</summary>
        /// <param name="handle"></param>
        /// <param name="latex"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern string latexToMathML(long handle, string latex);
        /// <summary>把传入的latex字符串转换成Office MathML字符串</summary>
        /// <param name="handle"></param>
        /// <param name="latex"></param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern string latexToOfficeMathML(long handle, string latex);
        /// <summary>从图片中解析点位并添加到识别引擎</summary>
        /// <param name="handle"></param>
        /// <param name="imgPath">图片路径绝对路径</param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int pictureProcess(long handle, string imgPath);
        /// <summary>设置Character-Set</summary>
        /// <param name="handle"></param>
        /// <param name="characters">Character-Set 输入字符串数组</param>
        /// <param name="charactersSize">数组长度</param>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern void setCharacterSet(
          long handle,
          string[] characters,
          long charactersSize);
        /// <summary>触发增量识别保存</summary>
        /// <param name="handle"></param>
        /// <param name="fullPathFile">保存的全路径文件名</param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int incRecDoSave(long handle, string fullPathFile);
        /// <summary>加载全路径文件的增量识别缓存</summary>
        /// <param name="handle"></param>
        /// <param name="fullPathFile">保存的全路径文件名</param>
        /// <param name="outSize">返回字符串长度</param>
        /// <returns></returns>
        [DllImport("Dll\\HTREngineWin.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr incRecLoadByPath(
          long handle,
          string fullPathFile,
          out long outSize);
    }
}
