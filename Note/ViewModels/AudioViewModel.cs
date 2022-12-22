using System;
using System.IO;
using Note.Enums;
using System.Linq;
using Note.Services;
using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using Windows.Foundation;
using System.Windows.Input;
using System.Windows.Media;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Threading;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;
using Windows.UI.Input.Inking.Core;
using Note.InkCanvasEx.ViewModels;
using Note.Models;

namespace Note.ViewModels
{
    public class AudioViewModel : BindableBase
    {
        private CoreInkIndependentInputSource coreInkIndependentInputSource;
        private DispatcherTimer timerRec = new DispatcherTimer();
        private DispatcherTimer timer = new DispatcherTimer();
        private InkCanvas _inkCanvas;
        private InkCanvasViewModel _inkCanvasViewModel;
        private bool isAudioMoreOpen;
        public bool IsAudioMoreOpen
        {
            get => this.isAudioMoreOpen;
            set => this.SetProperty(ref this.isAudioMoreOpen, value);
        }
        private string audioData;
        public string AudioData
        {
            get => this.audioData;
            set => this.SetProperty(ref this.audioData, value);
        }
        //存取音文信息：
        private AudioInfo audioInfo;
        private DateTimeOffset? lastOffset;
        private bool lastIsGray = false;
        private MediaPlayer player = new MediaPlayer();
        private List<InkStroke> orignalStrokes = new List<InkStroke>();
        private double duration;
        private bool initFlag = false;
        //录音位置：
        private string audioDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private bool addFlag = false;
        private bool isCanRecord = true;
        private bool hasAudio = false;
        public bool HasAudio
        {
            get => this.hasAudio;
            set
            {
                this.SetProperty(ref this.hasAudio, value);
                if (!this.HasAudio)
                {
                    IsAudioPanelVisible = false;
                }
            }
        }
        private bool isGray;
        private bool isGrayChecked = false;
        public bool IsGrayChecked
        {
            get => this.isGrayChecked;
            set
            {
                this.SetProperty(ref this.isGrayChecked, value);
                if (this.isGrayChecked)
                {
                    this.AddPointerReleasingForRecoding();
                    SetGray();
                }
                else
                {
                    this.RemovePointerReleasingForRecoding();
                    SetUnGray();
                }
            }
        }

        private bool isAudioPanelVisible;
        public bool IsAudioPanelVisible
        {
            get => this.isAudioPanelVisible;
            set => this.SetProperty(ref this.isAudioPanelVisible, value);
        }
        private double playerPosition;
        public double PlayerPosition
        {
            get => this.playerPosition;
            set => this.SetProperty(ref playerPosition, value);
        }
        private string playerTimeTip = "00:00/00:00";
        public string PlayerTimeTip
        {
            get => this.playerTimeTip;
            set
            {
                this.SetProperty(ref playerTimeTip, value);
            }
        }
        private bool isRecording;
        public bool IsRecording
        {
            get => this.isRecording;
            set
            {
                this.SetProperty(ref isRecording, value);
                if (!this.isRecording && this.HasAudio)
                {
                    IsAudioPanelVisible = true;
                    this.PlayerTimeTip = $"00:00/{FormatTime(TimeSpan.FromMilliseconds(recorder.Position))}";
                }
            }
        }
        private ICommand playCommand;
        public ICommand PlayCommand
        {
            get => this.playCommand ?? (this.playCommand = new RelayCommand(() =>
            {
                this.PlayAudio();
            }));
        }
        private ICommand stopCommand;
        public ICommand StopCommand
        {
            get => this.stopCommand ?? (this.stopCommand = new RelayCommand(() =>
            {
                PauseAudio();
            }));
        }
        private ICommand recordCommand;
        public ICommand RecordCommand
        {
            get => this.recordCommand ?? (this.recordCommand = new RelayCommand(() =>
            {
                this.OnAudio();
            }));
        }
        private double minimum = 1;
        public double Minimum
        {
            get => this.minimum;
            set => this.SetProperty(ref minimum, value);
        }
        private double maxinum = 100;
        public double Maximum
        {
            get => this.maxinum;
            set => this.SetProperty(ref maxinum, value);
        }
        private string audioLength;
        public string AudioLength
        {
            get => this.audioLength;
            set => this.SetProperty(ref audioLength, value);
        }
        private string lastAudioPath = "last_rec";
        public string LastAudioPath
        {
            get => this.lastAudioPath;
            set => this.SetProperty(ref this.lastAudioPath, lastAudioPath + ".wav");
        }

        private string audioPath = "rec";
        public string AudioPath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", audioPath + ".wav"); }
            set { audioPath = value; }
        }
        #region MoreCommand
        private ICommand exportCommand;
        public ICommand ExportCommand
        {
            get
            {
                return this.exportCommand ?? (this.exportCommand = new RelayCommand(async () =>
                {
                    this.IsAudioMoreOpen = false;
                    if (File.Exists(audioPath))
                    {
                        var desc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".mp3");
                        File.Copy(audioPath, desc, true);
                        await Task.Delay(1000);
                        var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                        if (inkCanvaEx != null)
                        {
                            inkCanvaEx.ToastNotify("导出成功");
                        }
                    }
                    else
                    {
                        var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                        if (inkCanvaEx != null)
                        {
                            inkCanvaEx.ToastNotify("导出失败");
                        }
                    }
                }));
            }
        }
        private ICommand deleteCommand;
        public ICommand DeleteCommand
        {
            get => this.deleteCommand ?? (this.deleteCommand = new RelayCommand(() =>
            {
                this.IsAudioMoreOpen = false;
                this.RecordInfos.Clear();
                this.CurrentRecoderInfo = new RecordInfo();
                this.duration = 0;
                hasAudio = false;
                this.isCanRecord = false;
                this.IsAudioPanelVisible = false;
                if (isGray)
                    SetUnGray();
                if (player != null)
                {
                    player.Stop();
                    player = null;
                    timer.Stop();
                }
                if (recorder != null)
                {
                    recorder.Stop();
                    recorder.Position = 0;
                }
                this.PlayerPosition = 0;
                this.AudioLength = "00:00";
                if (File.Exists(AudioPath))
                {
                    File.Delete(AudioPath);
                    var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                    if (inkCanvaEx != null)
                    {
                        inkCanvaEx.ToastNotify("删除成功");
                    }
                }
            }));
        }
        #endregion
        private NAudioRecorder recorder = new NAudioRecorder();
        public NAudioRecorder Recorder
        {
            get => this.recorder;
            set => this.SetProperty(ref this.recorder, value);
        }
        private RecordInfo currentRecoderInfo = new RecordInfo();
        public RecordInfo CurrentRecoderInfo
        {
            get => this.currentRecoderInfo;
            set => this.SetProperty(ref this.currentRecoderInfo, value);
        }
        private List<RecordInfo> recordInfos = new List<RecordInfo>();
        public List<RecordInfo> RecordInfos
        {
            get => this.recordInfos;
            set => this.SetProperty(ref this.recordInfos, value);
        }
        private bool isPlaying;
        public bool IsPlaying
        {
            get => this.isPlaying;
            set
            {
                this.SetProperty(ref this.isPlaying, value);
            }
        }
        private void OnAudio()
        {
            this.PlayerPosition = 0;
            switch (Recorder.State)
            {
                case RecordState.Stop:
                    if (player != null)
                    {
                        player.Close();
                        player = null;
                    }
                    timer.Stop();
                    if (isGray)
                    {
                        SetUnGray();
                    }
                    currentRecoderInfo = new RecordInfo();
                    currentRecoderInfo.StartTime = DateTimeOffset.Now;
                    string startFile = "";
                    if (File.Exists(LastAudioPath))
                    {
                        File.Delete(LastAudioPath);
                    }
                    var file = new FileInfo(AudioPath);
                    if (file.Exists)
                    {
                        if (file.Length > 0)
                        {
                            file.MoveTo(LastAudioPath);
                            startFile = LastAudioPath;
                        }
                        else
                        {
                            file.Delete();
                        }
                    }
                    Recorder.FilePath = AudioPath;
                    if (!Directory.Exists(audioDir))
                    {
                        Directory.CreateDirectory(audioDir);
                    }
                    Recorder.Start(startFile);
                    this.IsRecording = true;
                    this.HasAudio = true;
                    timerRec.Start();
                    hasAudio = true;
                    break;
                case RecordState.Recording:
                    duration = recorder.Position;
                    Recorder.Stop();
                    currentRecoderInfo.EndTime = DateTimeOffset.UtcNow;
                    currentRecoderInfo.RecordPosition = TimeSpan.FromMilliseconds(recorder.Position);
                    RecordInfos.Add(currentRecoderInfo);
                    this.IsRecording = false;
                    timerRec.Stop();
                    break;
                case RecordState.Pause:
                    if (isGray)
                    {
                        SetUnGray();
                    }
                    currentRecoderInfo = new RecordInfo();
                    currentRecoderInfo.StartTime = DateTimeOffset.UtcNow;
                    Recorder.Resume();
                    this.IsRecording = false;
                    timerRec.Stop();
                    break;
            }
        }
        //初始化音频设备
        public AudioViewModel(InkCanvasViewModel inkCanvasEx)
        {
            this._inkCanvasViewModel = inkCanvasEx;
            this._inkCanvas = this._inkCanvasViewModel.InkCanvas;
            if (!Directory.Exists(audioPath))
            {
                Directory.CreateDirectory(audioPath);
            }
            coreInkIndependentInputSource = CoreInkIndependentInputSource.Create(this._inkCanvas.InkPresenter);
            timerRec.Interval = TimeSpan.FromSeconds(1);
            timerRec.Tick += TimerRec_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;
            if (File.Exists(AudioPath))
            {
                File.Delete(AudioPath);
            }
            //是否打开进度条：
            this.IsAudioPanelVisible = !IsRecording && hasAudio;
            SystemEvents.PowerModeChanged += OnPowerModeChanged;
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }
        private void PlayAudio()
        {
            if (!File.Exists(AudioPath))
            {
                return;
            }
            if (WaveOut.DeviceCount < 1)
            {
                return;
            }
            //录音时长
            AudioLength = FormatTime(TimeSpan.FromMilliseconds(duration));
            recorder.Stop();
            if (player == null)
            {
                InitPlayer();
            }
            if (this.PlayerPosition == this.Maximum)
            {
                this.PlayerPosition = 0;
            }
            player.Play();
            timer.Start();
            IsPlaying = true;
            IsRecording = false;
        }
        private void PauseAudio()
        {
            this.IsPlaying = false;
            player.Pause();
            timer.Stop();
        }
        private void RecordAudio()
        {
            if (!initFlag)
            {
                initFlag = true;
                IsRecording = !IsRecording;
            }
            else
            {
                IsRecording = !IsRecording;
                this.IsAudioPanelVisible = !IsRecording;
            }
            this.IsAudioPanelVisible = !IsRecording && hasAudio;
            IsPlaying = false;
            PlayerPosition = 0;
            this.OnAudio();
            if (IsRecording)
            {
                this.AudioLength = FormatTime(TimeSpan.FromMilliseconds(recorder.Position));
                timerRec.Start();
                this.isCanRecord = true;
            }
            else
            {
                timer.Stop();
                this.AudioLength = FormatTime(TimeSpan.FromMilliseconds(duration));
                this.PlayerTimeTip = $"00:00/{FormatTime(TimeSpan.FromMilliseconds(duration))}";
            }
        }
        private void InitPlayer(bool autoPlay = false)
        {
            if (player == null)
            {
                player = new MediaPlayer();
                player.MediaEnded -= Player_MediaEnded;
                player.MediaEnded += Player_MediaEnded;
                player.Open(new Uri(AudioPath, UriKind.Absolute));
            }
        }
        private void Player_MediaEnded(object sender, EventArgs e)
        {
            timer.Stop();
            player.Stop();
            IsPlaying = false;
            player.Position = TimeSpan.FromSeconds(0);
            PlayerPosition = this.Maximum;
            PlayerTimeTip = $"{player.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}/{player.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
        }
        #region 置灰
        public void SetGray(DateTimeOffset? relativeOffset = null)
        {
            isGray = true;
            lastIsGray = true;
            if (!addFlag)
            {
                orignalStrokes.Clear();
                var strokes = this._inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
                foreach (var stroke in strokes)
                {
                    orignalStrokes.Add(stroke.Clone());
                }
                int count = orignalStrokes.Count;
                foreach (var stroke in strokes)
                {
                    var da = stroke.DrawingAttributes;
                    if (relativeOffset.HasValue)
                    {
                        if (relativeOffset.Value < stroke.StrokeStartedTime &&
                            RecordInfos.Where(x => x.StartTime < stroke.StrokeStartedTime &&
                                                x.EndTime > stroke.StrokeStartedTime).Count() > 0)
                        {
                            da.Color = GetAlphaColor(stroke.DrawingAttributes.Color, Windows.UI.Colors.White, 150);
                            stroke.DrawingAttributes = da;
                        }
                    }
                    else
                    {
                        if (RecordInfos.Where(x => x.StartTime < stroke.StrokeStartedTime &&
                                                x.EndTime > stroke.StrokeStartedTime).Count() > 0)
                        {
                            da.Color = GetAlphaColor(stroke.DrawingAttributes.Color, Windows.UI.Colors.White, 150);
                            stroke.DrawingAttributes = da;
                        }
                    }
                }
                addFlag = true;
            }
            else
            {
                if (relativeOffset.HasValue)
                {
                    if (!lastOffset.HasValue || lastOffset.Value < relativeOffset.Value) //播放前进中
                    {
                        var strokes = this._inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
                        for (int i = 0; i < strokes.Count; i++)
                        {
                            var attr = orignalStrokes[i].DrawingAttributes;
                            if (relativeOffset.Value > strokes[i].StrokeStartedTime && RecordInfos.Where(x => x.StartTime < strokes[i].StrokeStartedTime && x.EndTime > strokes[i].StrokeStartedTime).Count() > 0)
                            {
                                //attr.Size = new Size(attr.Size.Width + 2, attr.Size.Height + 2);
                                strokes[i].DrawingAttributes = attr;
                            }
                        }
                    }
                    if (lastOffset.HasValue && lastOffset.Value > relativeOffset.Value)
                    {
                        var strokes = this._inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
                        for (int i = 0; i < strokes.Count; i++)
                        {
                            var attr = orignalStrokes[i].DrawingAttributes;
                            if (relativeOffset.HasValue)
                            {
                                if (relativeOffset.Value < strokes[i].StrokeStartedTime && RecordInfos.Where(x => x.StartTime < strokes[i].StrokeStartedTime && x.EndTime > strokes[i].StrokeStartedTime).Count() > 0)
                                {
                                    attr.Color = GetAlphaColor(orignalStrokes[i].DrawingAttributes.Color, Windows.UI.Colors.White, 150);
                                    strokes[i].DrawingAttributes = attr;
                                }
                            }
                            else
                            {
                                if (RecordInfos.Where(x => x.StartTime < strokes[i].StrokeStartedTime && x.EndTime > strokes[i].StrokeStartedTime).Count() > 0)
                                {
                                    attr.Color = GetAlphaColor(orignalStrokes[i].DrawingAttributes.Color, Windows.UI.Colors.White, 150);
                                    strokes[i].DrawingAttributes = attr;
                                }
                            }
                        }
                    }
                }
            }
            lastOffset = relativeOffset;
        }
        public void SetUnGray()
        {
            addFlag = false;
            isGray = false;
            lastIsGray = false;
            this._inkCanvas.InkPresenter.StrokeContainer.Clear();
            this._inkCanvas.InkPresenter.StrokeContainer.AddStrokes(orignalStrokes);
            //进度条开启的条件：
            this.isCanRecord = recorder.Position < 7200000 && duration < 7200000;
        }
        private void AddPointerReleasingForRecoding()
        {
            coreInkIndependentInputSource.PointerReleasing -= Core_PointerReleasing_ForRecord;
            coreInkIndependentInputSource.PointerReleasing += Core_PointerReleasing_ForRecord;
        }
        private void RemovePointerReleasingForRecoding()
        {
            coreInkIndependentInputSource.PointerReleasing -= Core_PointerReleasing_ForRecord;
        }
        private void Core_PointerReleasing_ForRecord(CoreInkIndependentInputSource sender, Windows.UI.Core.PointerEventArgs args)
        {
            var pos = args.CurrentPoint.Position;
            var start = GetSelectedStrokeStartTime(pos);
            if (start != null && start != DateTimeOffset.MinValue)
            {
                var recordInfo = this.RecordInfos.Where(x => x.StartTime < start && x.EndTime > start).FirstOrDefault();
                if (recordInfo != null)
                {
                    if (player == null)
                    {
                        PlayAudio();
                    }
                    if (player != null)
                    {
                        player.Position = recordInfo.RecordPosition - (recordInfo.EndTime - start);
                        player.Play();
                        timer.Start();
                        IsPlaying = true;
                    }
                }
            }
        }
        #endregion
        #region 系统事件
        private void OnPowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume://系统挂起到重新唤醒
                    break;
                case PowerModes.Suspend://系统挂起
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (IsRecording)
                        {
                            RecordAudio();
                        }
                    });
                    break;
                case PowerModes.StatusChange://电源插拔
                    break;
            }
        }
        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                //锁屏
                case SessionSwitchReason.SessionLock:
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (IsRecording)
                        {
                            RecordAudio();
                        }
                    });
                    break;
                //解锁
                case SessionSwitchReason.SessionUnlock:
                    break;
                //注销
                case SessionSwitchReason.SessionLogoff:
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (IsRecording)
                        {
                            RecordAudio();
                        }
                    });
                    break;
            }
        }
        #endregion
        #region 定时器
        //播放置灰定时器：
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hasAudio)
            {
                if (player.NaturalDuration.HasTimeSpan)
                {
                    this.PlayerPosition = this.Maximum * (player.Position.TotalMilliseconds) / player.NaturalDuration.TimeSpan.TotalMilliseconds;
                }
                if (player.NaturalDuration.HasTimeSpan)
                {
                    this.PlayerTimeTip = $"{player.Position.ToString(@"mm\:ss")}/{player.NaturalDuration.TimeSpan.ToString(@"mm\:ss")}";
                }
            }
            else
            {
                this.PlayerPosition = 0;
                this.PlayerTimeTip = "00:00/00:00";
            }
            if (this.RecordInfos.Count > 0)
            {
                var r = this.RecordInfos.Where(x => x.RecordPosition > player.Position).FirstOrDefault();
                if (r != null)
                {
                    var relativePos = r.EndTime - (r.RecordPosition - player.Position);
                    if (isGray)
                    {
                        SetGray(relativePos);
                    }
                }
            }
        }
        //更新录音进度条：
        private void TimerRec_Tick(object sender, EventArgs e)
        {
            if (IsRecording)
            {
                this.AudioLength = FormatTime(TimeSpan.FromMilliseconds(recorder.Position));
                if (recorder.Position > 7200000)//停止录音
                {
                    duration = recorder.Position;
                    Recorder.Stop();
                    currentRecoderInfo.EndTime = DateTimeOffset.UtcNow;
                    currentRecoderInfo.RecordPosition = TimeSpan.FromMilliseconds(recorder.Position);
                    RecordInfos.Add(currentRecoderInfo);
                    this.IsRecording = false;
                    timerRec.Stop();
                    this.isCanRecord = false;
                }
            }
        }
        #endregion
        #region 逻辑功能
        private string FormatTime(TimeSpan timeSpan)
        {
            return ((int)timeSpan.TotalMinutes).ToString("00") + ":" + (((int)timeSpan.TotalSeconds) % 60).ToString("00");
        }
        public Windows.UI.Color GetAlphaColor(Windows.UI.Color c1, Windows.UI.Color c2, int alpha)
        {
            int r = (c1.R * (255 - alpha) + c2.R * alpha) / 255;
            int g = (c1.G * (255 - alpha) + c2.G * alpha) / 255;
            int b = (c1.B * (255 - alpha) + c2.B * alpha) / 255;
            return Windows.UI.Color.FromArgb(c1.A, (byte)r, (byte)g, (byte)b);
        }
        public DateTimeOffset GetSelectedStrokeStartTime(Point p, int radius = 20)
        {
            DateTimeOffset res = DateTimeOffset.MinValue;
            if (this._inkCanvas.InkPresenter.InputProcessingConfiguration.Mode == InkInputProcessingMode.None)
            {
                var strokes = this._inkCanvas.InkPresenter.StrokeContainer.GetStrokes();
                for (int i = strokes.Count - 1; i > -1; i--)
                {
                    Size size = strokes[i].DrawingAttributes.Size;
                    if (strokes[i].GetInkPoints().Where(
                        pt => Math.Abs(pt.Position.X - p.X) < (size.Width * 0.5 + radius) &&
                        Math.Abs(pt.Position.Y - p.Y) < (size.Height * 0.5 + radius)).Count() > 0)
                    {
                        res = strokes[i].StrokeStartedTime.GetValueOrDefault();
                        break;
                    }
                }
            }
            return res;
        }
        #endregion
        #region 序列化
        //获取音文信息文件：
        public string GetAudioData()
        {
            var recordingInfos = new List<RecordInfo>();
            if (this.RecordInfos.Count == 0)
            {
                if (this.CurrentRecoderInfo.StartTime > DateTimeOffset.MinValue)
                {
                    recordingInfos.Add(new RecordInfo()
                    {
                        StartTime = this.CurrentRecoderInfo.StartTime,
                        EndTime = DateTimeOffset.Now
                    });
                }
                else
                {
                    recordingInfos = this.RecordInfos;
                }
            }
            else
            {
                if (CurrentRecoderInfo.StartTime > DateTimeOffset.MinValue)
                {
                    recordingInfos.AddRange(this.RecordInfos);
                    recordingInfos.Add(new RecordInfo()
                    {
                        StartTime = CurrentRecoderInfo.StartTime,
                        EndTime = DateTimeOffset.Now
                    });
                }
            }
            var audioInfo = new AudioInfo() { AudioPath = this.AudioPath, RecordList = recordingInfos };
            return JsonConvert.SerializeObject(audioInfo);
        }
        //加载音文文件
        public void LoadAudioData(string filePath, string data)
        {
            if (audioPath == filePath)
            {
                return;
            }
            this.audioPath = filePath;
            lastIsGray = false;
            lastOffset = null;
            addFlag = false;
            this.RecordInfos.Clear();
            currentRecoderInfo = new RecordInfo();
            this.orignalStrokes.Clear();
            if (!string.IsNullOrEmpty(data))
            {
                AudioInfo audioInfo = JsonConvert.DeserializeObject<AudioInfo>(data);
                this.RecordInfos = audioInfo.RecordList;
            }
            isPlaying = false;
            if (player != null)
            {
                player.Stop();
                player = null;
            }
            if (recorder != null)
            {
                recorder.Stop();
                recorder.Position = 0;
            }
            //进度条信息
            this.PlayerPosition = 0;
            this.PlayerTimeTip = "00:00/00:00";
            if (this.isGray)
            {
                this.isGray = false;
            }
            this.IsRecording = false;
            timer.Stop();
            this.AudioLength = "00:00";
            this.duration = 0;
            hasAudio = false;
            if (File.Exists(AudioPath))
            {
                FileInfo file = new FileInfo(AudioPath);
                this.AudioLength = FormatTime(TimeSpan.FromSeconds(file.Length / 32000));
                duration = file.Length / 32;
                hasAudio = file.Length > 0;
            }
            this.PlayerTimeTip = $"00:00/{this.AudioLength}";
            this.isCanRecord = duration < 7200000;
            this.IsAudioPanelVisible = !IsRecording && hasAudio;
        }
        #endregion
    }
}
