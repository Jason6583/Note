namespace Note.InkCanvasEx.SDK
{
    /// <summary>识别点信息</summary>
    public class RecognizePoint : IRecognizePoint
    {
        /// <summary>x坐标</summary>
        public float X { get; set; }
        /// <summary>y坐标</summary>
        public float Y { get; set; }
        /// <summary>事件产生的时间戳</summary>
        public long T { get; set; }
        /// <summary>操作事件类型 0.落笔事件 1.移动事件 2.抬笔事件</summary>
        public int Action { get; set; }
        /// <summary>点所在线的id</summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="t"></param>
        /// <param name="action"></param>
        public RecognizePoint(float x, float y, long t, int action)
        {
            this.X = x;
            this.Y = y;
            this.T = t;
            this.Action = action;
        }
    }
}
