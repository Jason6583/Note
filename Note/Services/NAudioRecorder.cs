using System;
using Note.Enums;
using NAudio.Wave;
using NAudio.MediaFoundation;

namespace Note.Services
{
    public class NAudioRecorder
    {
        public RecordState State { get; set; } = RecordState.Stop;
        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;
        private bool RecordFlag = false;
        /// <summary>
        /// 录音结束后保存的文件路径
        /// </summary>
        public string FilePath = "rec.wav";
        /// <summary>
        /// 开始录音
        /// </summary>
        public void Start(string lastRecord = null)
        {
            State = RecordState.Recording;
            RecordFlag = true;
            waveSource = new WaveIn();
            waveSource.WaveFormat = new WaveFormat(16000, 16, 1); // 16bit,16KHz,Mono的录音格式
            waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
            waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
            waveFile = new WaveFileWriter(FilePath, waveSource.WaveFormat);
            if (!string.IsNullOrEmpty(lastRecord))
            {
                WaveFileReader reader = new WaveFileReader(lastRecord);
                WaveStreamWrite(reader, waveFile);
                reader.Close();
            }
            try
            {
                waveSource.StartRecording();
            }
            catch
            {
                waveSource.StartRecording();
            }
        }
        private void WaveStreamWrite(WaveFileReader reader, WaveFileWriter writer)
        {
            var buffer = new byte[1024 * 4];
            Position = reader.TotalTime.TotalMilliseconds;
            int read;
            while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, read);
            }
            writer.Flush();
        }
        public void Pause()
        {
            State = RecordState.Pause;
            RecordFlag = false;
        }
        public double Position { get; set; }
        public void Resume()
        {
            State = RecordState.Recording;
            RecordFlag = true;
        }
        /// <summary>
        /// 停止录音
        /// </summary>
        public void Stop()
        {
            State = RecordState.Stop;
            RecordFlag = false;
            //Close Wave(Not needed under synchronous situation)
            if (waveSource != null)
            {
                waveSource.StopRecording();
                waveSource.Dispose();
                waveSource = null;
            }
            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }
        /// <summary>
        /// 开始录音回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null && RecordFlag)
            {
                Position += waveSource.BufferMilliseconds;
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }
        /// <summary>
        /// 录音结束回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            Stop();
        }
        /// <summary>
        /// 将wav转为MP3
        /// </summary>
        /// <param name="sourceFile">in wav文件</param>
        /// <param name="desFile">out MP3文件</param>
        public static void ConvertWAVtoMP3(string sourceFile, string desFile)
        {
            MediaFoundationApi.Startup();
            using (var reader = new WaveFileReader(sourceFile))
            {
                MediaFoundationEncoder.EncodeToMp3(reader, desFile);
            }
        }
        /// <summary>
        /// 将mp3转为wav
        /// </summary>
        /// <param name="sourceFile">in mp3文件</param>
        /// <param name="desFile">out wav文件</param>
        public static void ConvertMP3toWAV(string sourceFile, string desFile)
        {
            using (var reader = new Mp3FileReader(sourceFile))
            {
                WaveFileWriter.CreateWaveFile(desFile, reader);
            }
        }
    }
}
