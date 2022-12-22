using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Note.Services.YouDao
{
    [DataContract]
    public class OcrRegionData
    {
        [DataMember(Name = "boundingBox")]
        public string Box { get; set; }
        [DataMember(Name = "dir")]
        public string Dir { get; set; }
        [DataMember(Name = "lang")]
        public string Lang { get; set; }
        [DataMember(Name = "lines")]
        public List<OcrLineData> Lines { get; set; }
    }
}
