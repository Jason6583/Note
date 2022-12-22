using System.Runtime.Serialization;

namespace Note.Services.YouDao
{
    [DataContract]
    public class OcrCorner
    {
        [DataMember(Name = "X")]
        public int X { get; set; }
        [DataMember(Name = "Y")]
        public int Y { get; set; }
    }
}
