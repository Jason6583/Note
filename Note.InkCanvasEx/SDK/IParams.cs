namespace Note.InkCanvasEx.SDK
{
    /// <summary>配置引擎参数，包括数据库路径、语言、json数据包含的信息等</summary>
    public interface IParams
    {
        /// <summary>获取当前引擎参数句柄</summary>
        /// <returns></returns>
        long GetHandle();
        /// <summary>获取数据公式参数</summary>
        /// <returns></returns>
        ICalculateParams GetCalculateParams();
        /// <summary>设置配置文件名称</summary>
        /// <param name="configName">配置文件名称</param>
        void SetConfigName(string configName);
        /// <summary>设置识别资源文件夹，包括配置文件、字典信息等</summary>
        /// <param name="path">资源文件夹路径</param>
        void SetDataDir(string path);
        /// <summary>设置引擎识别模式，默认为边写边识别</summary>
        /// <param name="mode"></param>
        void SetMode(RecognitionMode mode);
        /// <summary>获取当前设置的引擎识别模式</summary>
        /// <returns></returns>
        RecognitionMode GetMode();
        /// <summary>设置结果内容是否包含坐标信息，默认false</summary>
        /// <param name="coordinate">true包含，false不包含</param>
        void SetResultCoordinate(bool coordinate);
        /// <summary>
        /// 设置结果内容是否包含联想词，默认false
        /// 该接口为预留接口，功能暂未实现
        /// </summary>
        /// <param name="contained">true包含，false不包含</param>
        void SetResultAssociational(bool contained);
        /// <summary>
        /// 设置结果内容是否包含候选词，默认false
        /// 该接口为预留接口，功能暂未实现
        /// </summary>
        /// <param name="contained">true包含，false不包含</param>
        void SetResultCandidate(bool contained);
        /// <summary>设置结果内容是否包含分割框，默认false</summary>
        /// <param name="coordinate">true包含，false不包含</param>
        void SetResultPartitionCoordinate(bool coordinate);
        /// <summary>设置分词的时间间隔</summary>
        /// <param name="timeLot">分词的时间间隔，默认500ms</param>
        void SetWordSplitTimeLot(long timeLot);
        /// <summary>跨行后处理开关</summary>
        /// <param name="processedEnable">true打开，false关闭</param>
        void SetResultSpanProcessed(bool processedEnable);
    }
}
