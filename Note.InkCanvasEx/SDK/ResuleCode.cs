namespace Note.InkCanvasEx.SDK
{
    /// <summary>错误码</summary>
    public enum ResuleCode
    {
        /// <summary>失败</summary>
        CODE_FAILURE = 0,
        /// <summary>成功</summary>
        CODE_OK = 1,
        /// <summary>音频设备</summary>
        CODE_AUDIO = 100, // 0x00000064
        /// <summary>蓝牙设备</summary>
        CODE_BLUETOOTH = 200, // 0x000000C8
        /// <summary>容错模块</summary>
        CODE_ERROR = 300, // 0x0000012C
        /// <summary>文件管理</summary>
        CODE_FILE = 400, // 0x00000190
        /// <summary>HTTPS网络</summary>
        CODE_HTTPS = 500, // 0x000001F4
        /// <summary>IO设备</summary>
        CODE_IO = 600, // 0x00000258
        /// <summary>JSON工具</summary>
        CODE_JSON = 700, // 0x000002BC
        /// <summary>日志模块</summary>
        CODE_LOG = 800, // 0x00000320
        /// <summary>运行时监视</summary>
        CODE_MONITOR = 900, // 0x00000384
        /// <summary>SOCKET网络</summary>
        CODE_SOCKET = 1000, // 0x000003E8
        /// <summary>数据库管理</summary>
        CODE_DB = 1100, // 0x0000044C
        /// <summary>引擎鉴权</summary>
        CODE_VERIFY = 1200, // 0x000004B0
        /// <summary>语音设备</summary>
        CODE_VOICE = 1300, // 0x00000514
        /// <summary>XML工具</summary>
        CODE_XML = 1400, // 0x00000578
        /// <summary>ZIP工具</summary>
        CODE_ZIP = 1500, // 0x000005DC
        /// <summary>算法抽象模块</summary>
        CODE_ALGORITHM = 2000, // 0x000007D0
        /// <summary>数学计算模块</summary>
        CODE_ALGORITHM_MATH = 2100, // 0x00000834
        /// <summary>笔迹算法模块</summary>
        CODE_ALGORITHM_STROKE = 2200, // 0x00000898
        /// <summary>图形算法模块</summary>
        CODE_ALGORITHM_GRAPH = 2300, // 0x000008FC
        /// <summary>格式抽象模块</summary>
        CODE_FORMAT = 4000, // 0x00000FA0
        /// <summary>文档格式模块</summary>
        CODE_FORMAT_DOC = 4200, // 0x00001068
        /// <summary>视频算法模块</summary>
        CODE_FORMAT_VIDEO = 4300, // 0x000010CC
        /// <summary>识别抽象模块</summary>
        CODE_RECOGNIZE = 6000, // 0x00001770
        /// <summary>文字识别模块</summary>
        CODE_RECOGNIZE_CHARS = 6100, // 0x000017D4
        /// <summary>公式识别模块</summary>
        CODE_RECOGNIZE_FORMULA = 6200, // 0x00001838
        /// <summary>形状识别模块</summary>
        CODE_RECOGNIZE_SHAPE = 6300, // 0x0000189C
        /// <summary>绘图抽象模块</summary>
        CODE_DRAW = 8000, // 0x00001F40
        /// <summary>画布管理模块</summary>
        CODE_DRAW_CANVAS = 8100, // 0x00001FA4
        /// <summary>编辑操作模块</summary>
        CODE_DRAW_EDIT = 8200, // 0x00002008
        /// <summary>图片处理模块</summary>
        CODE_DRAW_IMAGE = 8300, // 0x0000206C
        /// <summary>笔型功能模块</summary>
        CODE_DRAW_PEN = 8300, // 0x0000206C
        /// <summary>表格设计模块</summary>
        CODE_DRAW_TABLE = 8300, // 0x0000206C
        /// <summary>辅助工具模块</summary>
        CODE_DRAW_TOOL = 8300, // 0x0000206C
        /// <summary>控件设计模块</summary>
        CODE_DRAW_UI = 8300, // 0x0000206C
    }
}
