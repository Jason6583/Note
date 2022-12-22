namespace Note.InkCanvasEx.SDK
{
    /// <summary>识别点信息</summary>
    public interface IRecognizePoint
    {
        /// <summary>x坐标</summary>
        float X { get; set; }
        /// <summary>y坐标</summary>
        float Y { get; set; }
        /// <summary>事件产生的时间戳</summary>
        long T { get; set; }
        /// <summary>操作事件类型 0.落笔事件 1.移动事件 2.抬笔事件</summary>
        int Action { get; set; }
        /// <summary>点所在线的id</summary>
        long Id { get; set; }
    }
}
