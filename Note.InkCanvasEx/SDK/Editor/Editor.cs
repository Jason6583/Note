using System;
using System.Text;
using Note.InkCanvasEx.SDK.Native;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Note.InkCanvasEx.SDK.Editor
{
    /// <summary>
    /// 实现不同的editor具有不同的功能
    /// 如：只需要文字识别，实现CalculateEditor和BaseEditor
    /// </summary>
    public abstract class Editor : IBaseEditor, IRecognizeEditor, ICalculateEditor
    {
        /// <summary>编辑器对象句柄</summary>
        protected long _handle;

        /// <summary>是否已经初始化</summary>
        private bool _isInitialized => this._handle != 0L;

        /// <summary>打开编辑器</summary>
        /// <param name="engineParams"></param>
        /// <returns></returns>
        public bool Open(IParams engineParams)
        {
            var res = LocalEditorNative.editoropen(this._handle, engineParams.GetHandle());
            return this._isInitialized &&  res == 1;
        }
        /// <summary>关闭编辑器</summary>
        public void Close()
        {
            if (!this._isInitialized)
                return;
            LocalEditorNative.editorclose(this._handle);
        }

        /// <summary>更新编辑器参数</summary>
        /// <param name="engineParams"></param>
        /// <returns></returns>
        public bool UpdateParams(IParams engineParams) => this._isInitialized && LocalEditorNative.updateParams(this._handle, engineParams.GetHandle()) == 1;

        /// <summary>
        /// 获取编辑器配置参数
        /// 当编辑器已经打开，获取编辑器配置参数
        /// 否则生成一个新的配置参数
        /// </summary>
        /// <returns></returns>
        public IParams ObtainParams()
        {
            long handle = 0;
            if (this._isInitialized)
                handle = LocalEditorNative.obtainParams(this._handle);
            return handle != 0L ? new Params(handle) : (IParams)null;
        }

        /// <summary>判断编辑器是否空闲</summary>
        /// <returns></returns>
        public bool IsIdle() => this._isInitialized && LocalEditorNative.isIdle(this._handle);

        /// <summary>等待编辑器空闲</summary>
        public void WaitForIdle()
        {
            if (!this._isInitialized)
                return;
            LocalEditorNative.waitForIdle(this._handle);
        }

        /// <summary>检查编辑器是否已是关闭状态</summary>
        /// <returns></returns>
        public bool IsClosed() => this._isInitialized && LocalEditorNative.isClosed(this._handle);

        /// <summary>检查编辑器内容是否为空</summary>
        /// <returns></returns>
        public bool IsEmpty() => this._isInitialized && LocalEditorNative.isEmpty(this._handle);

        /// <summary>获取当前已识别内容</summary>
        /// <returns></returns>
        public string GetContent() => this._isInitialized ? LocalEditorNative.getContent(this._handle) : "";

        /// <summary>清除编辑器中的内容</summary>
        public void ClearContent()
        {
            if (!this._isInitialized)
                return;
            LocalEditorNative.clearContent(this._handle);
        }

        /// <summary>生成一个识别点</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="t"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public IRecognizePoint ObtainPoint(float x, float y, long t, int action) => (IRecognizePoint)new RecognizePoint(x, y, t, action);

        /// <summary>批量添加设别点信息，每条笔划通过落笔点来分开</summary>
        /// <param name="recognizePoints"></param>
        public void AddPoints(List<IRecognizePoint> recognizePoints)
        {
            if (recognizePoints == null || recognizePoints.Count == 0 || !this._isInitialized)
                return;
            int count = recognizePoints.Count;
            double[] xCoordinate = new double[count];
            double[] yCoordinate = new double[count];
            double[] tCoordinate = new double[count];
            int[] actionCoordinate = new int[count];
            for (int index = 0; index < count; ++index)
            {
                xCoordinate[index] = (double)recognizePoints[index].X;
                yCoordinate[index] = (double)recognizePoints[index].Y;
                tCoordinate[index] = (double)recognizePoints[index].T;
                actionCoordinate[index] = recognizePoints[index].Action;
            }
            LocalEditorNative.addPoints(this._handle, xCoordinate, yCoordinate, tCoordinate, actionCoordinate, (long)recognizePoints.Count);
        }

        /// <summary>增加一条笔划的笔点信息</summary>
        /// <param name="recognizePoints"></param>
        /// <returns></returns>
        public int AddStrokePoints(List<IRecognizePoint> recognizePoints)
        {
            if (recognizePoints == null || recognizePoints.Count == 0 || !this._isInitialized)
                return 0;
            int count = recognizePoints.Count;
            double[] xCoordinate = new double[count];
            double[] yCoordinate = new double[count];
            double[] tCoordinate = new double[count];
            int[] numArray = new int[count];
            for (int index = 0; index < count; ++index)
            {
                xCoordinate[index] = (double)recognizePoints[index].X;
                yCoordinate[index] = (double)recognizePoints[index].Y;
                tCoordinate[index] = (double)recognizePoints[index].T;
                numArray[index] = recognizePoints[index].Action;
            }
            return LocalEditorNative.addStrokePoints(this._handle, xCoordinate, yCoordinate, tCoordinate, (long)recognizePoints.Count);
        }

        /// <summary>通过笔划的ID,删除一条笔划</summary>
        /// <param name="strokeId"></param>
        /// <returns></returns>
        public bool DeleteStrokePoints(int strokeId) => this._isInitialized && LocalEditorNative.deleteStrokePoints(this._handle, strokeId) == 1;

        /// <summary>
        /// 更新识别结果
        /// 当识别结果中包含words的时候，可以根据识别情况，用备选词代替返回的结果
        /// 此时可以把选择的备选词通过该方法传递回编辑器，以保证数据的一致性
        /// </summary>
        /// <param name="wordId"></param>
        /// <param name="candidate"></param>
        public void UpdateRecognizeWord(int wordId, string candidate)
        {
            if (!this._isInitialized)
                return;
            LocalEditorNative.updateRecognizeWord(this._handle, wordId, candidate);
        }

        /// <summary>在整篇识别模式{RecognitionMode.MODE_ECR}下，调用该方法触发识别</summary>
        public void DoPageRecognize()
        {
            if (!this._isInitialized)
                return;
            LocalEditorNative.doPageRecognize(this._handle);
        }

        /// <summary>进行联想词匹配</summary>
        /// <param name="text"></param>
        public void DoAssociate(string text)
        {
            if (!this._isInitialized)
                return;
            LocalEditorNative.doAssociate(this._handle, text);
        }

        /// <summary>公式计算接口</summary>
        /// <param name="strInputExp"></param>
        /// <returns></returns>
        public string Calculate(string strInputExp)
        {
            long outSize;
            IntPtr source = LocalEditorNative.calculate(this._handle, strInputExp, out outSize);
            byte[] numArray = new byte[outSize];
            Marshal.Copy(source, numArray, 0, (int)outSize);
            return Encoding.UTF8.GetString(numArray);
        }

        /// <summary>获取公式计算引擎版本号接口</summary>
        /// <returns></returns>
        public string GetMathEngineVersion() => LocalEditorNative.getMathEngineVersion(this._handle);

        /// <summary>把传入的latex字符串转换成MathML字符串</summary>
        /// <param name="latex"></param>
        /// <returns></returns>
        public string LatexToMathML(string latex) => LocalEditorNative.latexToMathML(this._handle, latex);

        /// <summary>把传入的latex字符串转换成Office MathML字符串</summary>
        /// <param name="latex"></param>
        /// <returns></returns>
        public string LatexToOfficeMathML(string latex) => LocalEditorNative.latexToOfficeMathML(this._handle, latex);

        /// <summary>从图片中解析点位并添加到识别引擎</summary>
        /// <param name="imgPath">图片路径绝对路径</param>
        /// <returns></returns>
        public int PictureProcess(string imgPath) => LocalEditorNative.pictureProcess(this._handle, imgPath);

        /// <summary>设置Character-Set</summary>
        /// <param name="characterArray">输入字符串数组</param>
        public void SetCharacterSet(string[] characterArray) => LocalEditorNative.setCharacterSet(this._handle, characterArray, (long)characterArray.Length);

        /// <summary>触发增量识别保存</summary>
        /// <param name="fullPathFile">保存的全路径文件名</param>
        /// <returns></returns>
        public int IncRecDoSave(string fullPathFile) => LocalEditorNative.incRecDoSave(this._handle, fullPathFile);

        /// <summary>加载全路径文件的增量识别缓存</summary>
        /// <param name="fullPathFile">保存的全路径文件名</param>
        /// <returns></returns>
        public string IncRecLoadByPath(string fullPathFile)
        {
            long outSize;
            IntPtr source = LocalEditorNative.incRecLoadByPath(this._handle, fullPathFile, out outSize);
            byte[] numArray = new byte[outSize];
            Marshal.Copy(source, numArray, 0, (int)outSize);
            return Encoding.UTF8.GetString(numArray);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public abstract void SetOnLoaded(Action<Note.InkCanvasEx.SDK.Editor.Editor> callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public abstract void SetOnError(Action<Note.InkCanvasEx.SDK.Editor.Editor, int, string> callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public abstract void SetOnContentChanged(Action<Note.InkCanvasEx.SDK.Editor.Editor, string> callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public abstract void SetOnAssociationalChanged(Action<Note.InkCanvasEx.SDK.Editor.Editor, string> callback);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        public abstract void SetOnCandidateChanged(Action<Note.InkCanvasEx.SDK.Editor.Editor, string> callback);
    }
}
