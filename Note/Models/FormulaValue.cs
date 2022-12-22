using System.Collections.Generic;

namespace Note.Models
{
    public struct FormulaValue
    {
        public List<string> Results { get; set; }
        public string Formula { get; set; }
        public int ErrorCode { get; set; }
        public FormulaValue(List<string> results, string formula, int errorCode)
        {
            Results = results;
            Formula = formula;
            ErrorCode = errorCode;
        }
    }
}
