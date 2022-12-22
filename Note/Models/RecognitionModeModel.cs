using Note.InkCanvasEx.SDK;
using Note.InkCanvasEx.Commons;

namespace Note.Models
{
    public class RecognitionModeModel : BindableBase
    {
        private RecognitionMode recognitionMode;
        /// <summary>
        /// 识别模式
        /// </summary>
        public RecognitionMode RecognitionMode
        {
            get => this.recognitionMode;
            set => this.SetProperty(ref recognitionMode, value);
        }
        /// <summary>
        /// 识别模式名称
        /// </summary>
        public string RecognitionModeName
        {
            get { return "交互式"; }
        }
    }
}
