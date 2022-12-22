using System;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Enums;
using Note.InkCanvasEx.SDK;

namespace Note.ViewModels
{
    public class MathSettingViewModel : BindableBase
    {
        private readonly int MAX_DIGITS = 16;
        private readonly int MIN_DIGITS = 0;
        public int RoundingMode { get; set; } = 0;
        public string RadianOrDegree { get; set; } = "optraddeg=degree";
        public FractionType FractionType { get; set; } = FractionType.Decimals;
        public Action ParamsChanged;

        private string digits = "6";
        public string Digits
        {
            set => this.SetProperty(ref this.digits, value);
            get => this.digits;
        }
        public void SetCalculateParams(IParams iparams)
        {
            var calcParams = iparams?.GetCalculateParams();
            if (calcParams != null)
            {
                calcParams.SetMathResultScale(Convert.ToInt32(Digits));
                calcParams.SetMathResultRoundingMode(RoundingMode);
                calcParams.SetMathEngineRadianOrDegree(RadianOrDegree);
            }
        }
    }
}
