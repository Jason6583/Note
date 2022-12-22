namespace Note.InkCanvasEx.SDK
{
    /// <summary>公式计算参数Params对象</summary>
    public interface ICalculateParams
    {
        /// <summary>设置是否需要公式计算结果</summary>
        /// <param name="calculate">true识别有计算结果，false没有计算结果</param>
        void SetResultCalculate(bool calculate);
        /// <summary>设置计算引擎支持弧度（Radian)、角度(Degree)</summary>
        /// <param name="optStr">optraddeg=radian;或者 optraddeg=degree;</param>
        void SetMathEngineRadianOrDegree(string optStr);
        /// <summary>设置计算结果取值方式</summary>
        /// <param name="roundingMode">0去尾法，1四舍五入</param>
        void SetMathResultRoundingMode(int roundingMode);
        /// <summary>设置保留小数后位数</summary>
        /// <param name="scale"></param>
        void SetMathResultScale(int scale);
    }
}
