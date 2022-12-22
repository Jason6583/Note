using System;

namespace Note.InkCanvasEx.SDK.Native
{
    /// <summary>公式编辑器</summary>
    public class CalculateParams : ICalculateParams
    {
        /// <summary>公式编辑器对象句柄</summary>
        public long Handle { get; private set; }

        public CalculateParams(long handle) => this.Handle = handle != 0L ? handle : throw new ArgumentException("Invalidate handle");

        /// <summary>设置计算引擎支持弧度（Radian)、角度(Degree)</summary>
        /// <param name="optStr"></param>
        public void SetMathEngineRadianOrDegree(string optStr) => LocalParamsNative.setMathEngineRadianOrDegree(this.Handle, optStr);

        /// <summary>设置计算结果取值方式</summary>
        /// <param name="roundingMode"></param>
        public void SetMathResultRoundingMode(int roundingMode) => LocalParamsNative.setMathResultRoundingMode(this.Handle, roundingMode);

        /// <summary>设置保留小数后位数</summary>
        /// <param name="scale"></param>
        public void SetMathResultScale(int scale) => LocalParamsNative.setMathResultScale(this.Handle, scale);

        /// <summary>设置是否需要公式计算结果</summary>
        /// <param name="calculate"></param>
        public void SetResultCalculate(bool calculate) => LocalParamsNative.setResultCalculate(this.Handle, calculate);
    }
}
