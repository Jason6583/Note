using System;
using System.IO;
using NAudio.Wave;
using System.Windows.Threading;
using Note.InkCanvasEx.Commons;

namespace Note.Services
{
    public class NAudioPlayer : BindableBase
    {
        private readonly string inputFolder;
        private string currentTime;
        public string CurrentTime
        {
            get { return currentTime; }
            set { this.SetProperty(ref currentTime, value); }
        }
        private string totalTime;
        public string TotalTime
        {
            get { return totalTime; }
            set { this.SetProperty(ref totalTime, value); }
        }
        private bool isPlaying;
        public bool IsPlaying
        {
            get { return isPlaying; }
            set { this.SetProperty(ref isPlaying, value); }
        }
        private WaveOut waveOut;
        private string fileName;
        private DispatcherTimer timer=new DispatcherTimer();
        private AudioFileReader file;
        public NAudioPlayer()
        {
            inputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(inputFolder))
            {
                Directory.CreateDirectory(inputFolder);
            }
        }
        public void Play(string filename)
        {
            this.fileName =Path.Combine(inputFolder, filename);
            waveOut = new WaveOut();
            waveOut.DeviceNumber = 0;
            //waveOut.DesiredLatency = 100;
            file =new AudioFileReader(fileName);
            waveOut.Init(file);
            waveOut.Play();
            this.timer.Interval = TimeSpan.FromMilliseconds(200);
            this.timer.Tick += new EventHandler(timer_Tick);
        }
        public void Pause()
        {
            waveOut.Pause();
            this.CurrentTime = FormatTimeSpan(file.CurrentTime);
            this.TotalTime = FormatTimeSpan(file.TotalTime);
            this.timer.Tick-= new EventHandler(timer_Tick);
        }
        public void Resume()
        {
            waveOut.Resume();
            this.timer.Tick += new EventHandler(timer_Tick);
        }
        private static string FormatTimeSpan(TimeSpan ts)
        {
            return string.Format("{0:D2}:{1:D2}", (int)ts.TotalMinutes, ts.Seconds);
        }
        //更新时间以为判断是否置灰
        void timer_Tick(object sender, EventArgs e)
        {
            if (file != null)
            {
                this.CurrentTime = FormatTimeSpan(file.CurrentTime);
                this.TotalTime = FormatTimeSpan(file.TotalTime);
            }
        }
        private void CleanUp()
        {
            if (this.file != null)
            {
                this.file.Dispose();
                this.file = null;
            }
            if (this.waveOut != null)
            {
                this.waveOut.Dispose();
                this.waveOut = null;
            }
        }
    }
}
