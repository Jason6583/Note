using System;

namespace Note.InkCanvasEx.Models
{
    /// <summary>
    /// 音文信息
    /// </summary>
    public class RecordInfo
    {
        public TimeSpan RecordPosition { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
    }
}
