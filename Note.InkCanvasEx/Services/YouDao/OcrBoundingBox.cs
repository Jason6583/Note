using System.Runtime.Serialization;

namespace Note.InkCanvasEx.YouDao
{
    [DataContract]
    public class OcrBoundingBox
    {
        public OcrCorner TopLeft { get; set; }
        public OcrCorner TopRight { get; set; }
        public OcrCorner BottomLeft { get; set; }
        public OcrCorner BottomRight { get; set; }
    }
}
