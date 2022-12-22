using System;
using System.IO;
using Note.Models;
using Note.Events;
using System.Text;
using System.Linq;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using Windows.Storage;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Windows.Input;
using System.Collections.Generic;
using System.Drawing.Imaging;
using Note.InkCanvasEx.Controls;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.Commons;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Note.InkCanvasEx.ViewModels
{
    internal class ExportViewModel : BindableBase
    {
        private IEventBus _eventBus;
        private string audioPath;
        private string audioData;
        private bool isImageChecked = true;
        public bool IsImageChecked
        {
            get => this.isImageChecked;
            set => this.SetProperty(ref this.isImageChecked, value);
        }
        private string exportFileName = "";
        private string fileName;
        public string FileName
        {
            get => this.fileName;
            set
            {
                exportFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), value);
                this.SetProperty(ref this.fileName, value);
            }
        }
        private ICommand importCommand;
        public ICommand ImportCommand
        {
            get => this.importCommand ?? (this.importCommand ?? (this.importCommand = new RelayCommand(() =>
            {
                //导入指令:
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
                openFileDialog.Filter = "笔记|*.isf";
                if (openFileDialog.ShowDialog() == true)
                {
                    inkCanvasEx.ClearContent();
                    var fileName = openFileDialog.FileName;
                    this.Load(fileName);
                    inkCanvasEx.ToastNotify("导入成功");
                }
            })));
        }
        private ICommand saveToDeskTopCommand;
        public ICommand SaveToDeskTopCommand
        {
            get => this.saveToDeskTopCommand ?? (this.saveToDeskTopCommand = new RelayCommand(async () =>
            {
                if(string.IsNullOrEmpty(exportFileName))
                {
                    inkCanvasEx.ToastNotify("导出文件未命名");
                    return;
                }
                var imageName = exportFileName + ".png";
                RenderTargetBitmap rtb = new RenderTargetBitmap();
                await rtb.RenderAsync(inkCanvasEx.mainView);
                var pixelBuffer = await rtb.GetPixelsAsync();
                var pixels = pixelBuffer.ToArray();
                var displayInformation = DisplayInformation.GetForCurrentView();
                FileInfo fileInfo = new FileInfo(imageName);
                StorageFolder folder = await KnownFolders.PicturesLibrary.CreateFolderAsync("SmartNote", CreationCollisionOption.OpenIfExists);
                StorageFile file = await folder.CreateFileAsync(Guid.NewGuid().ToString("N") + fileInfo.Extension, CreationCollisionOption.GenerateUniqueName);
                var buffer = await rtb.GetPixelsAsync();
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                         BitmapAlphaMode.Premultiplied,
                                         (uint)rtb.PixelWidth,
                                         (uint)rtb.PixelHeight,
                                         displayInformation.RawDpiX,
                                         displayInformation.RawDpiY,
                                         pixels);
                    await encoder.FlushAsync();
                }
                if (this.IsImageChecked)
                {
                    if (File.Exists(file.Path))
                    {
                        imageName=GetFileName(imageName);
                        File.Move(file.Path, imageName);
                    }
                }
                else
                {
                    var pdfName = exportFileName + ".pdf";
                    PdfDocument pdfDocument = new PdfDocument();
                    pdfDocument.AddPage(new PdfPage());
                    XGraphics g = XGraphics.FromPdfPage(pdfDocument.Pages[0]);
                    if (File.Exists(file.Path))
                    {
                        System.Drawing.Bitmap b = new System.Drawing.Bitmap(file.Path);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            b.Save(ms, ImageFormat.Png);
                            XImage img = XImage.FromStream(ms);
                            g.DrawImage(img, new XPoint(0, 0));
                        }
                        pdfName = GetFileName(pdfName);
                        pdfDocument.Save(pdfName);
                    }
                }
                this.Save(exportFileName + ".isf", this.audioData, this.audioPath);
                inkCanvasEx.ToastNotify("导出成功");
            }));
        }
        private InkCanvas inkCanvas;
        private Canvas canvas;
        private InkCanvasViewModel inkCanvasEx;
        public ExportViewModel(InkCanvasViewModel inkCanvasEx)
        {
            this.inkCanvasEx= inkCanvasEx;
            this.inkCanvas = inkCanvasEx.InkCanvas;
            this.canvas = inkCanvasEx.ShapeCanvas;
            this._eventBus = inkCanvasEx.EventBus;
            this._eventBus.Subscribe<AudioChangedEventArgs>(this.OnAudio);
        }
        private string GetFileName(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            var name=fileInfo.Name;
            var extension = fileInfo.Extension;
            int i = 1;
            while(File.Exists(fileName))
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf('.')) + $"({i++})" + extension;
            }
            return fileName;
        }
        private void OnAudio(AudioChangedEventArgs audioInfo)
        {
            this.audioData = audioInfo.AudioData;
            this.audioPath = audioInfo.AudioPath;
        }
        private async void Save(string filePath, string audioData, string audioPath)
        {
            try
            {
                filePath = GetFileName(filePath);
                using (var stream = new FileStream(filePath, FileMode.CreateNew))
                {
                    stream.WriteByte(1); 
                    var hasAudio = (byte)(inkCanvasEx.HasAudio ? 1 : 0);
                    stream.WriteByte(hasAudio);
                    if (hasAudio == 1)
                    {
                        //音频文件：
                        var audioModel = new AudioModel();
                        audioModel.AudioData = audioData;
                        audioModel.AudioPath = audioPath;
                        var audioJson = JsonConvert.SerializeObject(audioModel);
                        int audioDataLength = 0;
                        byte[] audioBytes = null;
                        if (!string.IsNullOrEmpty(audioJson))
                        {
                            audioBytes = Encoding.Default.GetBytes(audioJson);
                            audioDataLength = audioBytes.Length;
                        }
                        byte[] audioLengthBytes = BitConverter.GetBytes(audioDataLength);
                        stream.Write(audioLengthBytes, 0, audioLengthBytes.Length);
                        if (audioBytes != null)
                        {
                            stream.Write(audioBytes, 0, audioBytes.Length);
                        }
                    }
                    //图形转Json
                    var shapeStrs = new List<string>();
                    var shapeElements = this.canvas.Children.OfType<ShapeContainer>().ToList();
                    var hasShape = (byte)(shapeElements.Count > 0 ? 1 : 0);
                    stream.WriteByte(hasShape);
                    if (hasShape == 1)
                    {
                        foreach (var element in shapeElements)
                        {
                            shapeStrs.Add(element.ShapeStr);
                        }
                        var shapeJson = JsonConvert.SerializeObject(shapeStrs);
                        int shapeDataLength = 0;
                        byte[] shapeBytes = null;
                        if (!string.IsNullOrEmpty(shapeJson))
                        {
                            shapeBytes = Encoding.Default.GetBytes(shapeJson);
                            shapeDataLength = shapeBytes.Length;
                        }
                        byte[] shapeLengthBytes = BitConverter.GetBytes(shapeDataLength);
                        stream.Write(shapeLengthBytes, 0, shapeLengthBytes.Length);
                        if (shapeBytes != null)
                        {
                            stream.Write(shapeBytes, 0, shapeBytes.Length);
                        }
                    }
                    //图片信息转Json 图片：路径和位置
                    var imageJsons = new List<ImageElementJson>();
                    var imageElements = this.canvas.Children.OfType<ImageContainer>().ToList();
                    var hasImage = (byte)(imageElements.Count > 0 ? 1 : 0);
                    stream.WriteByte(hasImage);
                    if (hasImage == 1)
                    {
                        foreach (var element in imageElements)
                        {
                            var imageJson = new ImageElementJson();
                            imageJson.Left = Canvas.GetLeft(element);
                            imageJson.Top = Canvas.GetTop(element);
                            imageJson.FilePath = element.FileName;
                            imageJsons.Add(imageJson);
                        }
                        string imageStr = JsonConvert.SerializeObject(imageJsons);
                        int imageDataLength = 0;
                        byte[] imageDataBytes = null;
                        if (!string.IsNullOrEmpty(imageStr))
                        {
                            imageDataBytes = Encoding.Default.GetBytes(imageStr);
                            imageDataLength = imageDataBytes.Length;
                        }
                        byte[] imageLengthBytes = BitConverter.GetBytes(imageDataLength);
                        stream.Write(imageLengthBytes, 0, imageLengthBytes.Length);
                        if (imageDataBytes != null)
                        {
                            stream.Write(imageDataBytes, 0, imageDataBytes.Length);
                        }
                    }
                    //保存笔迹信息：
                    using (IOutputStream outputStream = stream.AsOutputStream())
                    {
                        await this.inkCanvas.InkPresenter.StrokeContainer.SaveAsync(stream.AsOutputStream());
                        await outputStream?.FlushAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async void Load(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var version = stream.ReadByte();
                    var hasAudio = stream.ReadByte();
                    if (hasAudio == 1)
                    {
                        //音频文件：
                        byte[] audioDataSizeBytes = new byte[sizeof(int)];
                        stream.Read(audioDataSizeBytes, 0, sizeof(int));
                        int length = BitConverter.ToInt32(audioDataSizeBytes, 0);
                        byte[] audioDataBytes = new byte[length];
                        stream.Read(audioDataBytes, 0, length);
                        string audioStr = Encoding.Default.GetString(audioDataBytes);
                        if (!string.IsNullOrEmpty(audioStr))
                        {
                            var audioData = JsonConvert.DeserializeObject<AudioModel>(audioStr);
                            inkCanvasEx.LoadAudioData(audioData.AudioPath, audioData.AudioData);
                        }
                    }
                    var hasShape = stream.ReadByte();
                    if (hasShape == 1)
                    {
                        //Json转图形
                        byte[] shapeDataSizeBytes = new byte[sizeof(int)];
                        stream.Read(shapeDataSizeBytes, 0, sizeof(int));
                        int shapeDataSize = BitConverter.ToInt32(shapeDataSizeBytes, 0);
                        byte[] shapeDataBytes = new byte[shapeDataSize];
                        stream.Read(shapeDataBytes, 0, shapeDataSize);
                        string shapeDataStr = Encoding.Default.GetString(shapeDataBytes);
                        if (!string.IsNullOrEmpty(shapeDataStr))
                        {
                            var shapeDatas = JsonConvert.DeserializeObject<List<string>>(shapeDataStr);
                            //添加图形：
                            foreach (var shapeData in shapeDatas)
                            {
                                //mainContext?.HTRService.AddShapes(shapeData);
                            }
                        }
                    }
                    var hasImage = stream.ReadByte();
                    if (hasImage == 1)
                    {
                        //Json转图片 图片：路径和位置
                        byte[] imgSizeBytes = new byte[sizeof(int)];
                        stream.Read(imgSizeBytes, 0, sizeof(int));
                        int imgDataSize = BitConverter.ToInt32(imgSizeBytes, 0);
                        byte[] imgBytes = new byte[imgDataSize];
                        stream.Read(imgBytes, 0, imgDataSize);
                        string imgStr = Encoding.Default.GetString(imgBytes);
                        if (!string.IsNullOrEmpty(imgStr))
                        {
                            var imageJsons = JsonConvert.DeserializeObject<List<ImageElementJson>>(imgStr);
                            //添加图片：
                            foreach (var imageJson in imageJsons)
                            {
                                inkCanvasEx.InsertImage(new ImageContainer(imageJson.FilePath), imageJson.Left, imageJson.Top);
                            }
                        }
                    }
                    //加载笔记：
                    await this.inkCanvas.InkPresenter.StrokeContainer.LoadAsync(stream.AsInputStream());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //压缩文件
        public void CompressFiles(string scrDir, string destFileName)
        {
            try
            {
                ZipFile.CreateFromDirectory(scrDir, destFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        //解压文件
        public void ExtractFiles(string scrFile, string destDir)
        {
            try
            {
                ZipFile.ExtractToDirectory(scrFile, destDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
