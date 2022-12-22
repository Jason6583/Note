using System;

namespace Note.InkCanvasEx.Services
{
    public class HTR
    {
        /// <summary>
        /// 默认语言
        /// </summary>
        public static readonly string DEFAULT_LAN = "zh";
        /// <summary>
        /// 默认语言模型路径
        /// </summary>
        public static readonly string DEFAULT_LAN_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets\\recognize\\conf\\");
        /// <summary>
        /// 默认公式模型路径
        /// </summary>
        public static readonly string DEFAULT_MATH_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets\\math\\");
        /// <summary>
        /// 默认形状模型路径
        /// </summary>
        public static readonly string DEFAULT_SHAPE_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets\\shape\\");
        /// <summary>
        /// 默认手势模型路径
        /// </summary>
        public static readonly string DEFAULT_GESTURE_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets\\gesture\\");
        /// <summary>
        /// 默认化学公式模型路径
        /// </summary>
        public static readonly string DEFAULT_CHEMICAL_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets\\chem\\");
        /// <summary>
        /// 默认简谱模型路径
        /// </summary>
        public static readonly string DEFAULT_NOTATION_PATH = System.IO.Path.Combine(Environment.CurrentDirectory, "Assets\\notation\\");
    }
}
