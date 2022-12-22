using Note.InkCanvasEx.Enums;
using Note.InkCanvasEx.Commons;

namespace Note.Models
{
    public class RecognitionTypeModel : BindableBase
    {
        private RecognitionType recognitionType;
        /// <summary>
        /// 识别类型 文字识别或公式识别
        /// </summary>
        public RecognitionType RecognitionType
        {
            get => this.recognitionType;
            set => this.SetProperty(ref this.recognitionType, value);
        }
        private string file;
        /// <summary>
        /// 获取文件名
        /// </summary>
        public string File
        {
            get => file;
            set => this.SetProperty(ref this.file, value);
        }

        private string path;
        /// <summary>
        /// 配置文件路径
        /// </summary>
        public string Path
        {
            get => this.path;
            set => this.SetProperty(ref this.path, value);
        }

        private string name;
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
    }
}
