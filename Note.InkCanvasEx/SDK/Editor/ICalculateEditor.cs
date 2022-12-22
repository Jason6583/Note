namespace Note.InkCanvasEx.SDK.Editor
{
    /// <summary>计算editor提供公式计算功能</summary>
    public interface ICalculateEditor
    {
        /// <summary>公式计算接口</summary>
        /// <param name="strInputExp">表达式</param>
        /// <returns>计算结果</returns>
        string Calculate(string strInputExp);

        /// <summary>获取公式计算引擎版本号接口</summary>
        /// <returns>版本号</returns>
        string GetMathEngineVersion();

        /// <summary>把传入的latex字符串转换成MathML字符串</summary>
        /// <param name="latex"></param>
        /// <returns></returns>
        string LatexToMathML(string latex);

        /// <summary>把传入的latex字符串转换成Office MathML字符串</summary>
        /// <param name="latex"></param>
        /// <returns></returns>
        string LatexToOfficeMathML(string latex);
    }
}
