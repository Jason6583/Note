namespace Note.InkCanvasEx.SDK.Accredit
{
    /// <summary>授权验证对象，设置识别引擎授权相关信息，验证通过后才能进行识别</summary>
    public class VerifyInfo
    {
        /// <summary>应用ID</summary>
        public string AppID;
        /// <summary>公钥</summary>
        public string PublicKey;
        /// <summary>设备唯一id</summary>
        public string DeviceId;
        /// <summary>设备型号</summary>
        public string Model;
        /// <summary>设备制造商</summary>
        public string Manufacturer;
        /// <summary>用户Id</summary>
        public string UserId;
        /// <summary>包名</summary>
        public string BrandName;
        /// <summary>P12证书路径</summary>
        public string CertPath;
        /// <summary>P12证书密码</summary>
        public string CertPwd;
        /// <summary>后台地址</summary>
        public string BaseUrl;
        /// <summary>离线证书</summary>
        public string OfflineCert;
        /// <summary>离线证书</summary>
        public string OfflineIdPath;
        /// <summary>保存的在线证书路径</summary>
        public string OnlineCertPath;
        /// <summary>记录第一次使用时间的文件路径</summary>
        public string RecFisrtUsePath;
        /// <summary>授权版本</summary>
        public string NetworkVer;
    }
}
