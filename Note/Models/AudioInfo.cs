using Note.InkCanvasEx.Models;
using System.Collections.Generic;

namespace Note.Models
{
    /// <summary>
    /// 声音文件的序列化与反序列化
    /// </summary>
    public class AudioInfo
    {
        public List<RecordInfo> RecordList { get; set; }
        public string AudioPath { get; set; }
        public double AudioLength { get; set; }
    }
}
