using System;
using Newtonsoft.Json;
using System.Diagnostics;
using Note.InkCanvasEx.Commons;

namespace Note.Models
{
    public class LanguageModel : BindableBase
    {
        private string key;
        /// <summary>
        /// 语言id
        /// </summary>
        public string Key
        {
            get => this.key;
            set => this.SetProperty(ref this.key, value);
        }
        private string langId;
        public string LangId
        {
            get => this.langId;
            set => this.SetProperty(ref this.langId, value);
        }
        private string name;
        /// <summary>
        /// 语言名称
        /// </summary>
        public string Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        public string OrignName
        {
            get => this.name;
        }
        private string file;
        /// <summary>
        /// 获取文件名
        /// </summary>
        public string File
        {
            get => this.file;
            set => this.SetProperty<string>(ref this.file, value);
        }
        private string path;
        /// <summary>
        /// 语言配置文件路径
        /// </summary>
        public string Path
        {
            get => this.path;
            set => this.SetProperty(ref this.path, value);
        }
        private bool isDefault;
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault
        {
            get => this.isDefault;
            set => this.SetProperty(ref this.isDefault, value);
        }
        private string _configName;
        public string ConfigName
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(_configName))
                    {
                        var content = System.IO.File.ReadAllText(Key);
                        var data = JsonConvert.DeserializeObject<dynamic>(content);
                        _configName = data["Conf-Name"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return _configName;
            }
        }
    }
}
