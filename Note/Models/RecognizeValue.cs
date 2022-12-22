using Note.InkCanvasEx.Enums;
using System.Collections.Generic;

namespace Note.Models
{
    public struct RecognizeValue
    {
        public string Result { get; set; }
        public RecognizeResultType ResultType { get; set; }
        public List<CandidateItem> CandidateItems { get; set; }
        public RecognizeValue(string result, RecognizeResultType resultType, List<CandidateItem> candidateItems)
        {
            Result = result;
            ResultType = resultType;
            CandidateItems = candidateItems;
        }
    }
}
