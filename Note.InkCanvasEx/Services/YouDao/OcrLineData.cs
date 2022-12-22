using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Note.InkCanvasEx.YouDao
{
    [DataContract]
    public class OcrLineData
    {
        [DataMember(Name = "boundingBox")]
        public string Box { get; set; }
        [DataMember(Name = "words")]
        public List<OcrWordData> Words { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}
