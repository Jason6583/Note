﻿using System.Runtime.Serialization;

namespace Note.InkCanvasEx.YouDao
{
    [DataContract]
    public class OcrResponceData
    {
        [DataMember(Name = "errorCode")]
        public string ErrorCode { get; set; }
        [DataMember(Name = "Result")]
        public OcrResultData Result { get; set; }
    }
}
