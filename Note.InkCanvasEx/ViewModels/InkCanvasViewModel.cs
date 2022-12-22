using System;
using Windows.UI;
using System.Linq;
using Windows.UI.Xaml;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Note.InkCanvasEx.Events;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Enums;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.Controls;
using System.Collections.Generic;
using Note.InkCanvasEx.SDK.Accredit;
using Note.InkCanvasEx.Services.Ink;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Note.InkCanvasEx.Services.Ink.UndoRedo;
using ICommand = System.Windows.Input.ICommand;

namespace Note.InkCanvasEx.ViewModels
{
    public class InkCanvasViewModel : BindableBase
    {
        public Grid mainView;
        private ScrollViewer scrollViewer;
        public IEventBus EventBus = new EventBus();
        //
        private InkStrokesService strokeService;
        private InkCopyPasteService copyPasteService;
        private InkLassoSelectionService lassoSelectionService;
        private InkPointerDeviceService pointerDeviceService;
        private InkUndoRedoService undoRedoService;
        private InkTransformService transformService;
        private InkFileService fileService;
        private InkZoomService zoomService;
        private InkEraseService eraserService;
        private InkSelectionRectangleService selectionRectangleService;
        public bool HasAudio { get; set; }
        public string AudioPath { get; set; }
        public PenType CurrentPenType { get; set; }
        public VerifyInfo CurrentVerifyInfo { get; set; }
        public static InkCanvasViewModel InkCanvasEx { get; private set; }
        public Canvas ShapeCanvas { get; private set; }
        public InkCanvas InkCanvas { get; private set; }
        public Canvas SelectionCanvas { get; private set; }
        public CanvasControl CanvasControl { get; private set; }

        private bool inkToText;
        public bool InkToText
        {
            get { return inkToText; }
            set 
            { 
                this.SetProperty(ref inkToText , value); 
                if(this.InkToText)
                {
                    this.InkOnly = false;
                    this.InkToShape = false;
                    this.IsEraser = false;
                    this.IsSelection = false;
                    this.ConfigSmartPen(true);
                    this.EventBus.Publish(new InkToTextEventArgs());
                    this.EventBus.Publish(new SmartPenEventArgs() { IsSmartPen = true });
                }
            }
        }

        private bool inkToShape;
        public bool InkToShape
        {
            get { return inkToShape; }
            set 
            {
                this.SetProperty(ref this.inkToShape, value); 
                if(this.inkToShape)
                {
                    this.InkOnly = false;
                    this.IsEraser = false;
                    this.IsSelection = false;
                    this.InkToText = false;
                    this.ConfigSmartPen(true);
                    this.EventBus.Publish(new InkToShapeEventArgs());
                    this.EventBus.Publish(new SmartPenEventArgs() { IsSmartPen = true });
                }
            }
        }

        private bool inkOnly;
        public bool InkOnly
        {
            get { return inkOnly; }
            set 
            { 
                this.SetProperty(ref this.inkOnly, value); 
                if(this.inkOnly)
                {
                    this.InkToShape = false;
                    this.IsEraser = false;
                    this.IsSelection = false;
                    this.InkToText = false;
                    this.ConfigSmartPen(false);
                    this.EventBus.Publish(new SmartPenEventArgs() { IsSmartPen = false });
                }
            }
        }

        private bool isEraser;
        public bool IsEraser
        {
            get { return this.isEraser; }
            set 
            { 
                this.SetProperty(ref this.isEraser, value);
                if (isEraser)
                {
                    this.InkOnly = false;
                    this.InkToShape = false;
                    this.IsSelection = false;
                    this.InkToText = false;
                    this.ConfigSmartPen(false);
                    this.eraserService.StartEraserSelectionConfig();
                    this.EventBus.Publish(new SmartPenEventArgs() { IsSmartPen = false });
                }
                else
                {
                    this.eraserService.StopEraserSelectionConfig();
                }
            }
        }

        private bool isSelection;
        public bool IsSelection
        {
            get { return this.isSelection; }
            set 
            { 
                this.SetProperty(ref this.isSelection, value);
                if (this.isSelection)
                {
                    this.InkOnly = false;
                    this.InkToShape = false;
                    this.IsEraser = false;
                    this.InkToText = false;
                    this.ConfigSmartPen(false);
                    lassoSelectionService?.StartLassoSelectionConfig();
                    this.EventBus.Publish(new SmartPenEventArgs() { IsSmartPen = false });
                }
                else
                {
                    lassoSelectionService?.EndLassoSelectionConfig();
                }
            }
        }

        private bool enableFinger;
        public bool EnableFinger
        {
            get { return this.enableFinger; }
            set
            {
                this.SetProperty(ref this.enableFinger, value);
                this.ConfigInputMode(this.enableFinger);
            }
        }

        private ICommand inkCommand;
        public ICommand InkCommand
        {
            get { return inkCommand??(this.inkCommand=new RelayCommand(this.OnInk)); }
        }

        private void OnInk()
        {
            this.InkOnly = true;
        }

        private ICommand eraserCommand;
        public ICommand EraserCommand
        {
            get { return this.eraserCommand = new RelayCommand(this.DoErase); }
        }
        private void DoErase()
        {
            this.IsEraser = !this.IsEraser;
        }

        private ICommand screenShotCommand;
        public ICommand ScreenShotCommand
        {
            get { return screenShotCommand??(this.screenShotCommand=new RelayCommand(this.ScreenShot)); }
        }
        private void ScreenShot()
        {
            this.EventBus.Publish(new ScreenShotEventArgs());
        }

        private ICommand insertImageCommand;
        public ICommand InsertImageCommand
        {
            get { return this.insertImageCommand ?? (this.insertImageCommand = new RelayCommand(this.InsertImage)); }
        }

        private void InsertImage()
        {
            this.EventBus.Publish(new InsertImageEventArgs());  
        }

        private ICommand recordCommand;
        public ICommand RecordCommand
        {
            get { return this.recordCommand ?? (this.recordCommand = new RelayCommand(this.Record)); }
        }

        private string audioLength;
        public string AudioLength
        {
            get => this.audioLength;
            set=>this.SetProperty(ref this.audioLength, value);
        }

        private bool isRecording = false;
        public bool IsRecording
        {
            get => this.isRecording;
            set => this.SetProperty(ref this.isRecording, value);
        }
        private void Record()
        {
            this.IsRecording = !this.IsRecording;
            this.EventBus.Publish(new RecordEventArgs() { IsRecording = this.IsRecording });
        }

        private ICommand undoCommand;
        public ICommand UndoCommand
        {
            get { return this.undoCommand??(this.undoCommand=new RelayCommand(this.Undo)); }
        }

        private ICommand stopCommand;
        public ICommand StopCommand
        {
            get => this.stopCommand ?? (this.stopCommand = new RelayCommand(this.StopRecorder));
        }

        private void StopRecorder()
        {
            this.IsRecording = !this.IsRecording;
            this.EventBus.Publish(new RecordEventArgs() { IsRecording = this.IsRecording });
        }

        public InkCanvasViewModel(Grid mainView,ScrollViewer scrollViewer,Canvas shapeCanvas, InkCanvas inkCanvas, Canvas selectionCanvas, CanvasControl canvasControl)
        {
            this.mainView = mainView;
            this.scrollViewer = scrollViewer;
            this.ShapeCanvas = shapeCanvas;
            this.InkCanvas = inkCanvas;
            this.SelectionCanvas = selectionCanvas;
            this.CanvasControl = canvasControl;
            //
            strokeService = new InkStrokesService(inkCanvas.InkPresenter);
            selectionRectangleService = new InkSelectionRectangleService(inkCanvas, selectionCanvas, strokeService);
            lassoSelectionService = new InkLassoSelectionService(inkCanvas, selectionCanvas, strokeService, selectionRectangleService);
            copyPasteService = new InkCopyPasteService(strokeService);
            pointerDeviceService = new InkPointerDeviceService(inkCanvas);
            undoRedoService = new InkUndoRedoService(inkCanvas, strokeService);
            transformService = new InkTransformService(inkCanvas, shapeCanvas, strokeService,undoRedoService, EventBus);
            fileService = new InkFileService(inkCanvas, strokeService);
            zoomService = new InkZoomService(scrollViewer);
            eraserService = new InkEraseService(inkCanvas, shapeCanvas, undoRedoService, transformService);
            strokeService.StrokesCollected += StrokeService_StrokesCollected;
            strokeService.CopyStrokesEvent += (s, e) => RefreshEnabledButtons();
            strokeService.SelectStrokesEvent += (s, e) => RefreshEnabledButtons();
            strokeService.ClearStrokesEvent += (s, e) => RefreshEnabledButtons();
            strokeService.ClearStrokesEvent += (s, e) => RefreshEnabledButtons();
            undoRedoService.UndoEvent += (s, e) => RefreshEnabledButtons();
            undoRedoService.RedoEvent += (s, e) => RefreshEnabledButtons();
            undoRedoService.AddUndoOperationEvent += (s, e) => RefreshEnabledButtons();
            this.ChangeBackground("#FFFFFF");
            //赋值当前内容
            InkCanvasEx = this;
            this.ShapeCanvas.ManipulationStarted += ShapeCanvas_ManipulationStarted;
            this.ShapeCanvas.ManipulationDelta += ShapeCanvas_ManipulationDelta;
            this.ShapeCanvas.ManipulationCompleted += ShapeCanvas_ManipulationCompleted;
        }
        private void ShapeCanvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {

        }
        private void ShapeCanvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {

        }
        private void ShapeCanvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

        }
        private void StrokeService_StrokesCollected(object sender, InkStrokesCollectedEventArgs e)
        {
            if(CurrentPenType== PenType.ParkPen)
            {
                this.CustomParkPenStyle(e);
            }
        }
        private void RefreshEnabledButtons()
        {
            if (InkCanvas.InkPresenter.StrokeContainer.GetStrokes().Any())
            {
                //this.UndoButtonIsEnabled = true;
            }
        }
        public async void ToastNotify(string message)
        {
            MessageContainer border = new MessageContainer(message);
            border.VerticalAlignment = VerticalAlignment.Center;
            border.HorizontalAlignment = HorizontalAlignment.Center;
            this.mainView.Children.Add(border);
            await Task.Delay(1000);
            this.mainView.Children.Remove(border);
        }
        //自定义钢笔笔形：
        private void CustomParkPenStyle(InkStrokesCollectedEventArgs args)
        {
            var stroke = args.Strokes.First();
            var cloneStroke = args.Strokes.First().Clone();
            List<InkPoint> ps = cloneStroke.GetInkPoints().ToList();
            if (ps.Count > 1)
            {
                var first = ps.First();
                ps.Remove(first);
                var second = ps.First();
                InkPoint f = new InkPoint(first.Position, second.Pressure * 9 / 10, ps.Last().TiltX, ps.Last().TiltY, ps.Last().Timestamp + 800);
                ps.Insert(0, f);
            }
            var ls1 = ps.Last();
            var ls2 = ps.Last();
            if (ps.Count > 1)
                ls2 = ps[ps.Count - 2];
            ////两个点的毫秒数
            var t = (ls1.Timestamp - ls2.Timestamp);
            ////速度 像素PER MS
            Vector2 v1 = new Vector2((float)ls1.Position.X, (float)ls1.Position.Y);
            var v2 = new Vector2((float)ls2.Position.X, (float)ls2.Position.Y);
            var l = Vector2.Distance(v1, v2);
            var v = l / t;
            var ax = v * 8 + ls1.Position.X;
            var ay = v * 8 + ls1.Position.Y;
            if (ps.Count > 6)
            {
                var last5 = ps.GetRange(ps.Count - 6, 6);
                ps.RemoveRange(ps.Count - 6, 6);
                var p = last5[0].Pressure;
                for (int i = 0; i < 6; i++)
                {
                    var tp = Math.Max(0.00001f, p - p / 5 * i);
                    InkPoint ttt = new InkPoint(last5[i].Position, tp, last5[i].TiltX, last5[i].TiltY, last5[i].Timestamp);
                    ps.Add(ttt);
                }
            }
            InkPoint ap = new InkPoint(new Point(ax, ay), 0, ps.Last().TiltX, ps.Last().TiltY, ps.Last().Timestamp + 8 * TimeSpan.TicksPerMillisecond);
            ps.Add(ap);
            InkStrokeBuilder builder = new InkStrokeBuilder();
            var da = cloneStroke.DrawingAttributes;
            stroke.Selected = true;
            builder.SetDefaultDrawingAttributes(da);
            InkStroke ink = builder.CreateStrokeFromInkPoints(ps, cloneStroke.PointTransform, cloneStroke.StrokeStartedTime, cloneStroke.StrokeDuration);
            this.InkCanvas.InkPresenter.StrokeContainer.AddStroke(ink);
            this.InkCanvas.InkPresenter.StrokeContainer.DeleteSelected();
        }
        public void ConfigEraserService(bool isEraser)
        {
            if(isEraser)
            {
                this.eraserService.StartEraserSelectionConfig();
            }
            else
            {
                this.eraserService.StopEraserSelectionConfig();
            }
        }
        public void ChangeEraserSize(double eraserWidth)
        {
            this.eraserService.EraserWidth = eraserWidth;
        }
        public void ChangeBackground(Color color)
        {
            this.mainView.Background = new SolidColorBrush(color);
            this.scrollViewer.Background = new SolidColorBrush(color);
        }
        public void ChangeBackground(string color)
        {
            this.mainView.Background = GetSolidColorBrush(color);
            this.scrollViewer.Background = GetSolidColorBrush(color);
        }
        private SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte r = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            return new SolidColorBrush(Color.FromArgb(255, r, g, b));
        }
        public void AddToUndoList(UIElement element)
        {
            var undoList = new List<UIElement>();
            undoList.Add(element);
            this.undoRedoService.AddOperation(new AddTextAndShapeUndoOperation(undoList, transformService));
        }
        public void ClearContent()
        {
            //清空笔迹：
            var strokes = strokeService?.GetStrokes().ToList();
            var textAndShapes = this.ShapeCanvas.Children.ToList();
            this.ShapeCanvas.Children.Clear();
            strokeService.ClearStrokes();
            undoRedoService.AddOperation(new ClearStrokesAndShapesUndoRedoOperation(strokes, textAndShapes, strokeService, transformService));
        }
        public void InsertImage(ImageContainer image,double left,double top)
        {
            //添加图片
            Canvas.SetLeft(image, left);
            Canvas.SetTop(image, top);
            this.ShapeCanvas.Children.Add(image);
            //删除操作
            image.CloseHandler += (s, e) => this.ShapeCanvas.Children.Remove(image);
            //弹出信息：
            MessageContainer border = new MessageContainer("识别中，请稍等");
            image.RecognizeHandler += async (s, e) =>
            {
                await this.ShapeCanvas.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    border.VerticalAlignment = VerticalAlignment.Center;
                    border.HorizontalAlignment = HorizontalAlignment.Center;
                    this.mainView.Children.Add(border);
                });
            };
            image.RecognizeCallback += async (object o, RecogizeEventArgs e) =>
            {
                await this.ShapeCanvas.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.mainView.Children.Remove(border);
                });
            };
            //撤消操作
            var undoList = new List<UIElement>();
            undoList.Add(image);
            undoRedoService.AddOperation(new AddTextAndShapeUndoOperation(undoList, transformService));
        }
        public void Undo()
        {
            ClearSelection();
            undoRedoService?.Undo();
        }
        public void Redo()
        {
            ClearSelection();
            undoRedoService?.Redo();
        }
        private void ClearSelection()
        {
            //nodeSelectionService.ClearSelection();
            lassoSelectionService.ClearSelection();
        }
        public void ConfigLassoSelection(bool enableLasso)
        {
            if (enableLasso)
            {
                lassoSelectionService?.StartLassoSelectionConfig();
            }
            else
            {
                lassoSelectionService?.EndLassoSelectionConfig();
            }
        }
        public void ConfigInputMode(bool enableFinger)
        {
            pointerDeviceService.EnablePen = true;
            if (enableFinger)
            {
                pointerDeviceService.EnableMouse = true;
                pointerDeviceService.EnableTouch = true;
            }
            else
            {
                pointerDeviceService.EnableMouse = false;
                pointerDeviceService.EnableTouch = false;
            }
        }
        public void AddText(string text)
        {
            this.transformService.ClearText();
            this.transformService.DrawText(text);
        }
        public void ConfigSmartPen(bool isSmartPen)
        {
            this.transformService.IsSmartPenMode = isSmartPen;
        }
        public string GetAudioData()
        {
            return null;
        }
        
        public void LoadAudioData(string filePath,string audioData)
        {

        }
    }
}
