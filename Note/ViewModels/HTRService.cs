using System;
using System.IO;
using System.Linq;
using System.Text;
using Note.Models;
using Note.Services;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Enums;
using Note.InkCanvasEx.SDK;
using Note.InkCanvasEx.SDK.Editor;
using Note.InkCanvasEx.Services;
using Note.InkCanvasEx.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Note.InkCanvasEx.Events;

namespace Note.ViewModels
{
    public class HTRService : BindableBase
    {
        private const double IdleWaitingTime = 500;
        private readonly DispatcherTimer dispatcherTimer;
        private InkCanvasViewModel _inkCanvasEx;
        private InkCanvas _inkCanvas;
        private Canvas _shapeCanvas;
        private AuthorizeService authorizeService;
        private IEventBus _eventBus;
        //SDK对象
        private IParams _engineParams;
        private Editor _editor;
        private Engine _engine;
        /// <summary>
        /// 默认语言
        /// </summary>
        public static readonly string DEFAULT_LAN = "zh";
        /// <summary>
        /// 默认语言模型路径
        /// </summary>
        public static readonly string DEFAULT_LAN_PATH = Path.Combine(Environment.CurrentDirectory, "Assets\\recognize\\conf\\");
        /// <summary>
        /// 默认公式模型路径
        /// </summary>
        public static readonly string DEFAULT_MATH_PATH = Path.Combine(Environment.CurrentDirectory, "Assets\\math\\");
        /// <summary>
        /// 默认形状模型路径
        /// </summary>
        public static readonly string DEFAULT_SHAPE_PATH = Path.Combine(Environment.CurrentDirectory, "Assets\\shape\\");
        /// <summary>
        /// 默认手势模型路径
        /// </summary>
        public static readonly string DEFAULT_GESTURE_PATH = Path.Combine(Environment.CurrentDirectory, "Assets\\gesture\\");
        /// <summary>
        /// 默认化学公式模型路径
        /// </summary>
        public static readonly string DEFAULT_CHEMICAL_PATH = Path.Combine(Environment.CurrentDirectory, "Assets\\chem\\");
        /// <summary>
        /// 默认简谱模型路径
        /// </summary>
        public static readonly string DEFAULT_NOTATION_PATH = Path.Combine(Environment.CurrentDirectory, "Assets\\notation\\");
        public int RoundingMode { get; set; } = 0;
        public string RadianOrDegree { get; set; } = "optraddeg=degree";
        public FractionType FractionType { get; set; } = FractionType.Decimals;

        public StrokeManager _strokeManager;
        public BorderManager _borderManager;
        public ShapeManager _shapeManager;
        public RecognizeManager _recognizeManager;
        public void StartTimer() => dispatcherTimer.Start();
        public void StopTimer() => dispatcherTimer.Stop();

        private string digits = "6";
        public string Digits
        {
            get => this.digits;
            set => this.SetProperty(ref this.digits, value);
        }
        private string engineVersion;
        public string EngineVersion
        {
            get => this.engineVersion;
            set => this.SetProperty(ref this.engineVersion, value);
        }
        private LogLevel _logLevel;
        public LogLevel LogLevel
        {
            get => this._logLevel;
            set => this.SetProperty(ref this._logLevel, value);
        }
        private LanguageModel languageSelected;
        public LanguageModel LanguageSelected
        {
            get => this.languageSelected;
            set => this.SetProperty(ref this.languageSelected, value);
        }
        private RecognitionMode recognizeMode = RecognitionMode.MODE_ICR;
        public RecognitionMode RecognitionMode
        {
            get => this.recognizeMode;
            set => this.SetProperty(ref this.recognizeMode, value);
        }
        private ObservableCollection<LanguageModel> languageItems;
        public ObservableCollection<LanguageModel> LanguageItems
        {
            get => this.languageItems;
            set => this.SetProperty(ref this.languageItems, value);
        }
        private bool smartPenMode;
        public bool SmartPenMode
        {
            get => this.smartPenMode;
            set => this.SetProperty(ref this.smartPenMode, value);
        }
        public HTRService()
        {

        }
        private void OnClear(ClearTextEventArgs clearall)
        {
            this._editor?.ClearContent();
        }
        public void InitRecognizer(InkCanvasViewModel inkCanvasEx)
        {
            this._inkCanvasEx = inkCanvasEx;
            this._eventBus = inkCanvasEx.EventBus;
            this._inkCanvas = inkCanvasEx.InkCanvas;
            this._shapeCanvas = inkCanvasEx.ShapeCanvas;
            this._inkCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            this._inkCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
            _recognizeManager = new RecognizeManager();
            _strokeManager = new StrokeManager();
            _shapeManager = new ShapeManager(this._inkCanvasEx);
            _borderManager = new BorderManager(this._shapeCanvas);
            this.authorizeService = new AuthorizeService();
            this.InitializeEngine();
            this.InitializeEditor(DEFAULT_LAN_PATH);
            this._eventBus = inkCanvasEx.EventBus;
            this._eventBus.Subscribe<ClearTextEventArgs>(OnClear);
            //dispatcherTimer = new DispatcherTimer();
            ////dispatcherTimer.Tick += DispatcherTimer_Tick;
            //dispatcherTimer.Interval = TimeSpan.FromMilliseconds(IdleWaitingTime);
            var reconizeType = new RecognitionTypeModel()
            {
                RecognitionType = RecognitionType.Word,
                Name = "文本",
                Path = HTR.DEFAULT_LAN_PATH
            };
            this.RecognizeTypes.Add(reconizeType);
            //reconizeType = new RecognitionTypeModel()
            //{
            //    RecognitionType = RecognitionType.Math,
            //    Name="Math",
            //    Path = DEFAULT_MATH_PATH
            //};
            //this.RecognizeTypes.Add(reconizeType);
            //reconizeType = new RecognitionTypeModel()
            //{
            //    RecognitionType = RecognitionType.Chemical,
            //    Name="Chemical",
            //    Path = DEFAULT_CHEMICAL_PATH
            //};
            //this.RecognizeTypes.Add(reconizeType);
            reconizeType = new RecognitionTypeModel()
            {
                RecognitionType = RecognitionType.Shape,
                Name = "形状",
                Path = HTR.DEFAULT_SHAPE_PATH
            };
            this.RecognizeTypes.Add(reconizeType);
            //reconizeType = new RecognitionTypeModel()
            //{
            //    RecognitionType = RecognitionType.Gesture,
            //    Name="Gesture",
            //    Path = DEFAULT_GESTURE_PATH
            //};
            //this.RecognizeTypes.Add(reconizeType);
            this.SelectedRecognizeType = this.RecognizeTypes.First();

        }
        private void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            try
            {
                var erasedStrokes = args.Strokes;
                foreach (var stroke in erasedStrokes)
                {
                    var inkStokeId = _strokeManager.RemovePointPath(stroke);
                    _editor.DeleteStrokePoints(inkStokeId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void DispatcherTimer_Tick(object sender, object e)
        {
            StopTimer();
            var strokes = this._inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
            foreach (var stroke in strokes)
            {
                this.AddStroke(stroke);
            }
        }
        private void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            if (!SmartPenMode) return;
            if (SelectedRecognizeType.RecognitionType == RecognitionType.Shape)
            {
                var stroke = _inkCanvas.InkPresenter.StrokeContainer.GetStrokes().Last();
                stroke.Selected = true;
                this.AddStroke(stroke);
                _inkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
            }
            else
            {
                if (args.Strokes.Count > 0)
                {
                    foreach (var stroke in args.Strokes)
                    {
                        this.AddStroke(stroke);
                    }
                }
            }
        }
        private void AddStrokes(List<InkStroke> strokes)
        {
            try
            {
                if (strokes == null || strokes.Count == 0) return;

                if (SelectedRecognizeType.RecognitionType == RecognitionType.Shape
                    || SelectedRecognizeType.RecognitionType == RecognitionType.Gesture)
                {
                    try
                    {
                        _editor.ClearContent();
                        AddStroke(strokes.Last());
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                foreach (var item in strokes)
                {
                    var pointPath = _strokeManager.AddPointPath(item);
                    var strokeId = _editor.AddStrokePoints(pointPath.RecognizePoints);
                    pointPath.StrokeId = strokeId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public void ReRecognizeMoveStrokes(List<InkStroke> strokes)
        {
            if (strokes == null) return;
            foreach (var item in strokes)
            {
                var pointPath = _strokeManager.GetPointPath(item);
                if (pointPath == null) continue;
                _editor.DeleteStrokePoints(pointPath.StrokeId);
                var strokeId = _editor.AddStrokePoints(pointPath.RecognizePoints);
                pointPath.StrokeId = strokeId;
            }
        }
        private void AddStroke(InkStroke stroke)
        {
            if (SelectedRecognizeType.RecognitionType == RecognitionType.Shape || SelectedRecognizeType.RecognitionType == RecognitionType.Gesture)
            {
                try
                {
                    _editor.ClearContent();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            var pointPath = _strokeManager.AddPointPath(stroke);
            var strokeId = _editor.AddStrokePoints(pointPath.RecognizePoints);
            pointPath.StrokeId = strokeId;
        }
        private void RemovePoints(List<InkStroke> strokes)
        {
            if (strokes == null || strokes.Count == 0) return;
            foreach (var item in strokes)
            {
                var strokeId = _strokeManager.RemovePointPath(item);
                ThreadPool.QueueUserWorkItem(RemoveStrokeAsync, strokeId);
            }
        }
        private List<LanguageModel> _customerLanguages = new List<LanguageModel>();
        private void RemoveStrokeAsync(object obj)
        {
            if (obj is int strokeId)
            {
                _editor.DeleteStrokePoints(strokeId);
                Console.WriteLine("Remove stroke points:{0}", strokeId);
            }
        }
        /// <summary>
        /// 初始化识别引擎
        /// </summary>
        public void InitializeEngine()
        {
            var verifyInfo = this.authorizeService.CreateVerifyInfo();
            _engine = Engine.CreateInstance(verifyInfo);
            if (_engine != null)
            {
                _engine.SetOnVerifyCallback(OnEngineVerifyCallback);
                _engine.Start();
                this.EngineVersion = _engine.GetEngineVersion();
                Console.WriteLine($"Engine version :{EngineVersion}");
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                _engine.SetLogDir(path);
                _engine.SetDebugLevel((int)this._logLevel);
                Console.WriteLine($"Log level:{this._logLevel}, dir:{path}");
            }
            else
            {
                throw new InvalidOperationException("Init engine error");
            }
        }
        private void OnEngineVerifyCallback(int code, string msg)
        {
            if ((code & 0x1000) == 0x1000)
            {
                Console.WriteLine("Verify exception:{0}", msg);
            }
            else if ((code & 0x4000) == 0x4000)
            {
                Console.WriteLine("Verify info:{0}", msg);
            }
            if (code < 1000)
            {
                Console.WriteLine("Verify code:{0}, msg:{1}", code, msg);
                if (code != 0)
                {
                    Console.WriteLine("Verify Failed", code);
                }
            }
        }
        /// <summary>
        /// 初始化引擎编辑器
        /// </summary>
        public void InitializeEditor(string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var engineParams = Engine.CreateEditorParams();
            _editor = _engine.CreateEditor(engineParams);
            if (_editor != null)
            {
                InitializeParams(path);
                Console.WriteLine("Registered callback");
                _editor.SetOnLoaded(OnLoaded);
                _editor.SetOnError(OnError);
                _editor.SetOnContentChanged(OnContentChanged);
                var opened = _editor.Open(_engineParams);
                Console.WriteLine($"Editor open:{opened}");
            }
            else
            {
                throw new Exception("Init editor failed");
            }
        }
        private void InitializeParams(string path)
        {
            InitAssets(path);//DEFAULT_LAN_PATH
            _engineParams = _editor.ObtainParams();
            if (_engineParams == null)
            {
                Engine.CreateEditorParams();
            }
            if (_engineParams == null)
            {
                throw new InvalidOperationException("Init engine param error");
            }
            if (LanguageSelected != null)
            {
                _engineParams.SetDataDir(LanguageSelected.Path);
                _engineParams.SetConfigName(LanguageSelected.File);
                Console.WriteLine($"Data dir:{LanguageSelected.Path}, config name:{LanguageSelected.File}");
            }
            _engineParams.SetResultPartitionCoordinate(true);
            _engineParams.SetResultCoordinate(true);
            _engineParams.SetResultSpanProcessed(true);
            _engineParams.SetWordSplitTimeLot(500);
            _engineParams.SetMode(RecognitionMode.MODE_ICR);

            //var viewModel = ServiceLocator.Current.GetInstance<MathSettingViewModel>();
            //viewModel.ParamsChanged = RefreshMathResult;
            //viewModel.SetCalculateParams();
            this.SetCalculateParams(_engineParams);
        }
        private void OnLoaded(Editor editor)
        {

        }
        private void OnError(Editor editor, int code, string msg)
        {

        }
        private void OnContentChanged(Editor editor, string content)
        {
            Console.Write("Rev content:{0}", content);
            Application.Current.Dispatcher.Invoke(() =>
            {
                var recognizeValue = this._recognizeManager.GetRecognizeValue(content, this.RecognitionMode, this.SelectedRecognizeType.RecognitionType,
                    _borderManager, _shapeManager);
                var strokes = this._inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
                try
                {
                    if (recognizeValue.ResultType == RecognizeResultType.Text)
                    {
                        if (strokes != null)
                        {
                            this._inkCanvasEx?.AddText(recognizeValue.Result);
                        }
                    }
                    if (recognizeValue.ResultType == RecognizeResultType.Math)
                    {
                        if (strokes != null)
                        {
                            //var boundingRect = strokes.Last().BoundingRect;
                            //this.transformService.ClearTextAndShapes();
                            //this.transformService.DrawText(recognizeValue.Result, boundingRect);
                        }
                    }
                    else if (recognizeValue.ResultType == RecognizeResultType.Jianpu)
                    {
                        if (strokes != null)
                        {
                            //var boundingRect = strokes.Last().BoundingRect;
                            //this.transformService.ClearTextAndShapes();
                            //this.transformService.DrawText(recognizeValue.Result, boundingRect);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
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
        private void Clear()
        {
            try
            {
                _inkCanvas.InkPresenter.StrokeContainer.Clear();
                _editor?.ClearContent();
                _strokeManager?.Clear();
                _borderManager?.Clear();
                _shapeManager?.Clear();
                _recognizeManager?.Clear();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //_isFullPointOperating = false;
            }
        }
        private ObservableCollection<RecognitionTypeModel> recognizeTypes = new ObservableCollection<RecognitionTypeModel>();
        public ObservableCollection<RecognitionTypeModel> RecognizeTypes
        {
            get => this.recognizeTypes;
            set => this.SetProperty(ref this.recognizeTypes, value);
        }
        private RecognitionTypeModel selectedRecognizeType = null;
        public RecognitionTypeModel SelectedRecognizeType
        {
            get => this.selectedRecognizeType;
            set
            {
                this.SetProperty(ref this.selectedRecognizeType, value);
                Clear();
                if (this.selectedRecognizeType != null)
                {
                    switch (this.selectedRecognizeType.RecognitionType)
                    {
                        case RecognitionType.Word:
                            this.InitializeEngine();
                            this.InitializeEditor(this.selectedRecognizeType.Path);
                            if (languageItems != null) _customerLanguages.ForEach(l => LanguageItems.Add(l));
                            break;
                        case RecognitionType.Math:
                            this.InitializeEngine();
                            this.InitializeEditor(this.selectedRecognizeType.Path);
                            break;
                        case RecognitionType.Chemical:
                            this.InitializeEngine();
                            this.InitializeEditor(this.selectedRecognizeType.Path);
                            break;
                        case RecognitionType.Shape:
                            this.InitializeEngine();
                            this.InitializeEditor(this.selectedRecognizeType.Path);
                            break;
                        case RecognitionType.Gesture:
                            this.InitializeEngine();
                            this.InitializeEditor(this.selectedRecognizeType.Path);
                            break;
                    }
                }
            }
        }
        private void InitAssets(string assetsPath)
        {
            try
            {
                var languagesModels = new ObservableCollection<LanguageModel>();
                LanguageModel defaultModel = null;
                if (Directory.Exists(assetsPath))
                {
                    string[] folders = Directory.GetDirectories(assetsPath);
                    List<string> tempNames = new List<string>();
                    foreach (var item in folders)
                    {
                        string[] names = Directory.GetFiles(item, "*.conf", SearchOption.TopDirectoryOnly);
                        if (names.Length == 0) continue;

                        string langId = Path.GetFileNameWithoutExtension(names[0]);
                        var model = new LanguageModel()
                        {
                            Key = Path.GetFullPath(names[0]),
                            Name = new DirectoryInfo(item).Name,
                            LangId = langId,
                            Path = item,
                            IsDefault = true,
                            File = Path.GetFileName(names[0])
                        };
                        if (model.LangId == DEFAULT_LAN)
                        {
                            defaultModel = model;
                        }
                        languagesModels.Add(model);
                        tempNames.Add(model.LangId);
                    }
                    if (languagesModels.Count == 0)
                    {
                        return;
                    }
                    Console.WriteLine($"Assets list:{string.Join(",", tempNames)}");
                    if (defaultModel == null)
                    {
                        defaultModel = languagesModels.ElementAt(0);
                    }
                }
                LanguageItems = languagesModels;
                LanguageSelected = defaultModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        private bool InitCustomerLanguages(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path)) return false;
                if (!Directory.Exists(path)) return false;
                string[] names = Directory.GetFiles(path, "*.conf", SearchOption.TopDirectoryOnly);
                if (names.Length == 0) return false;
                foreach (var item in names)
                {
                    string key = Path.GetFullPath(item);
                    if (LanguageItems.Where(l => l.Key == key).Count() > 0) continue;

                    var model = new LanguageModel()
                    {
                        Key = key,
                        Name = Path.GetFileNameWithoutExtension(item),
                        Path = path,
                        IsDefault = false,
                        File = Path.GetFileName(item)
                    };
                    LanguageItems.Add(model);
                    _customerLanguages.Add(model);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        public void AddShapes(string shapeStr)
        {
            this._shapeManager.AddShapes(shapeStr);
        }
    }
}
