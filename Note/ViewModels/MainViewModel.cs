using Note.Views;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Input;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Controls;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.Services;
using Note.InkCanvasEx.ViewModels;
using Note.InkCanvasEx.Enums;

namespace Note.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private IEventBus _eventBus;
        private InkCanvasViewModel inkCanvasEx;
        public InkCanvasViewModel InkCanvasEx
        {
            get { return inkCanvasEx; }
            set { this.SetProperty(ref inkCanvasEx, value); }
        }
        private InkStrokeAudioService inkStrokeAudioService;
        public InkStrokeAudioService InkStrokeAudioService
        {
            get { return inkStrokeAudioService; }
            set { this.SetProperty(ref this.inkStrokeAudioService, value); }
        }
        private HTRService htrService;
        public HTRService HTRService
        {
            get => this.htrService;
            set => this.SetProperty(ref this.htrService, value);
        }
        private string message;
        public string Message
        {
            get => this.message;
            set => this.SetProperty(ref this.message, value);
        }
        private bool isMorePopupOpen;
        public bool IsMorePopupOpen
        {
            get => this.isMorePopupOpen;
            set => this.SetProperty(ref this.isMorePopupOpen, value);
        }
        private bool isMoreColorOpen;
        public bool IsMoreColorOpen
        {
            get => this.isMoreColorOpen;
            set => this.SetProperty(ref this.isMoreColorOpen, value);
        }
        private bool isEraserPopupOpen;
        public bool IsEraserPopupOpen
        {
            get => this.isEraserPopupOpen;
            set => this.SetProperty(ref this.isEraserPopupOpen, value);
        }
        //橡皮擦开启：
        private bool eraserChecked;
        public bool EraserChecked
        {
            get => this.eraserChecked;
            set
            {
                this.SetProperty(ref this.eraserChecked, value);
                IsEraserPopupOpen = value;
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ConfigEraserService(this.eraserChecked);
                }
            }
        }
        private bool lassoSelectionButtonIsChecked;
        public bool LassoSelectionButtonIsChecked
        {
            get => lassoSelectionButtonIsChecked;
            set
            {
                this.SetProperty(ref lassoSelectionButtonIsChecked, value);               
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ConfigLassoSelection(value);
                }
            }
        }
        private bool enableHandWriting = true;
        public bool EnableHandWriting
        {
            get => this.enableHandWriting;
            set
            {
                this.SetProperty(ref this.enableHandWriting, value);
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ConfigInputMode(this.enableHandWriting);
                }
            }
        }
        private ICommand changeBackColorCommand;
        public ICommand ChangeBackColorCommand
        {
            get => this.changeBackColorCommand ?? (changeBackColorCommand = new RelayCommand(() =>
            {
                this.IsMorePopupOpen = false;
                this.IsMoreColorOpen = true;
            }));
        }
        private ICommand insertImageCommand;
        public ICommand InsertImageCommand
        {
            get => this.insertImageCommand ?? (this.insertImageCommand = new RelayCommand(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    var fileName = openFileDialog.FileName;
                    this.InsertImage(fileName, 200, 200);
                }
            }));
        }
        private ICommand undoCommand;
        public ICommand UndoCommand
        {
            get => this.undoCommand ?? (this.undoCommand = new RelayCommand(() =>
            {
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.Undo();
                }
            }));
        }
        private ICommand redoCommand;
        public ICommand RedoCommand
        {
            get => this.redoCommand ?? (this.redoCommand = new RelayCommand(() =>
            {
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.Redo();
                }
            }));
        }
        private ICommand screenShotCommand;
        public ICommand ScreenShotCommand
        {
            get => this.screenShotCommand ?? (this.screenShotCommand = new RelayCommand(() =>
            {
                Application.Current.MainWindow.WindowState = WindowState.Minimized;
                ScreenShotView shotView = new ScreenShotView(this.inkCanvasEx);
                shotView.ShowDialog();
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }));
        }
        private bool smartPenMode;
        public bool SmartPenMode
        {
            get => this.smartPenMode;
            set
            {
                this.SetProperty(ref this.smartPenMode, value);
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ConfigSmartPen(this.smartPenMode);
                }
                if(this.smartPenMode)
                {
                    this.HTRService.InitializeEngine();
                    this.HTRService.InitializeEditor(HTR.DEFAULT_LAN_PATH);
                }
            }
        }

        public MainViewModel()
        {
            this.HTRService = new HTRService();
            this.InkStrokeAudioService = new InkStrokeAudioService();
        }
        public void LoadInkCanvasEx(InkCanvasViewModel inkCanvasEx)
        {
            this.inkCanvasEx = inkCanvasEx;
            this.HTRService.InitRecognizer(inkCanvasEx);
            this.InkStrokeAudioService.InitAudioService(inkCanvasEx);
            this._eventBus = inkCanvasEx.EventBus;
            this._eventBus.Subscribe<InkToTextEventArgs>(this.OnInkToText);
            this._eventBus.Subscribe<InkToShapeEventArgs>(this.OnInkToShape);
            this._eventBus.Subscribe<ScreenShotEventArgs>(this.OnScreenShot);
            this._eventBus.Subscribe<InsertImageEventArgs>(this.OnInsertImage);
            this._eventBus.Subscribe<RecordEventArgs>(this.OnRecord);
            this._eventBus.Subscribe<SmartPenEventArgs>(this.OnSmartPen);
        }

        private void OnSmartPen(SmartPenEventArgs obj)
        {
            this.HTRService.SmartPenMode = obj.IsSmartPen;
        }

        private void OnRecord(RecordEventArgs e)
        {
            InkStrokeAudioService.IsRecording = e.IsRecording;
            InkStrokeAudioService.OnAudio();
        }
        private void OnInkToText(InkToTextEventArgs obj)
        {
            this.HTRService.SmartPenMode = true;
            this.HTRService.InitializeEngine();
            this.HTRService.InitializeEditor(HTR.DEFAULT_LAN_PATH);
            this.HTRService.SelectedRecognizeType.RecognitionType = RecognitionType.Word;
        }
        private void OnInkToShape(InkToShapeEventArgs e)
        {
            this.HTRService.SmartPenMode = true;
            this.HTRService.InitializeEngine();
            this.HTRService.InitializeEditor(HTR.DEFAULT_SHAPE_PATH);
            this.HTRService.SelectedRecognizeType.RecognitionType = RecognitionType.Shape;
        }
        private void OnScreenShot(ScreenShotEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
            ScreenShotView shotView = new ScreenShotView(this.inkCanvasEx);
            shotView.ShowDialog();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }
        private void OnInsertImage(InsertImageEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var fileName = openFileDialog.FileName;
                this.InsertImage(fileName, 200, 200);
            }
        }

        public void InsertImage(string filePath, double left, double top)
        {
            ImageContainer image = new ImageContainer(filePath);
            var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
            if (inkCanvaEx != null)
            {
                inkCanvaEx.InsertImage(image, left, top);
            }
            image.RecognizeCallback += (object o, RecogizeEventArgs e) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    var ocrText = e.Result;
                    var textview = new TextRecognizeView();
                    var textviewModel = new TextRecognizeViewModel(ocrText,this.inkCanvasEx);
                    textview.DataContext = textviewModel;
                    textview.ShowDialog();
                });
            };
            this._eventBus = inkCanvasEx.EventBus;
        }
    }
}
