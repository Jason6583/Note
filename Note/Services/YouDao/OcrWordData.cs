using System.Runtime.Serialization;

namespace Note.Services.YouDao
{
    [DataContract]
    public class OcrWordData
    {
        [DataMember(Name = "boundingBox")]
        public string Box { get; set; }
        [DataMember(Name = "word")]
        public string Word { get; set; }
    }
}
