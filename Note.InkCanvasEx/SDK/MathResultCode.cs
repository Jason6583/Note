namespace Note.InkCanvasEx.SDK
{
    /// <summary>公式计算错误码</summary>
    public enum MathResultCode
    {
        /// <summary>计算成功</summary>
        MATH_RESULT_SUCCESSFUL,
        /// <summary>除数为0</summary>
        MATH_RESULT_DENOMINATOR_IS_ZERO,
        /// <summary>方法不支持</summary>
        MATH_RESULT_METHOD_NO_SUPPORTED,
        /// <summary>计算值太小</summary>
        MATH_RESULT_VALUE_TOO_SMALL,
        /// <summary>表达式错误</summary>
        MATH_RESULT_EXPRESSION_ERROR,
        /// <summary>无效参数</summary>
        MATH_RESULT_INVALID_ARGUMENTS,
    }
}
