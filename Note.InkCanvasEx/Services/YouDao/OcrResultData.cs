using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Note.InkCanvasEx.YouDao
{
    [DataContract]
    public class OcrResultData
    {
        [DataMember(Name = "orientation")]
        public string Orientation { get; set; }
        [DataMember(Name = "regions")]
        public List<OcrRegionData> Regions { get; set; }
    }
}
