using System.ComponentModel;

namespace Note.InkCanvasEx.Enums
{
    /// <summary>
    /// 识别结果类型
    /// </summary>
    public enum RecognizeResultType
    {
        /// <summary>
        /// 文字
        /// </summary>
        [Description("Text")]
        Text,
        /// <summary>
        /// 数学公式
        /// </summary>
        [Description("math")]
        Math,
        /// <summary>
        /// 形状
        /// </summary>
        [Description("Shape")]
        Shape,
        /// <summary>
        /// 手势
        /// </summary>
        [Description("Gesture")]
        Gesture,
        /// <summary>
        /// 化学公式
        /// </summary>
        [Description("Chemical")]
        Chemical,
        /// <summary>
        /// 简谱
        /// </summary>
        [Description("Jianpu")]
        Jianpu
    }
}
