using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Note.Services.YouDao
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
