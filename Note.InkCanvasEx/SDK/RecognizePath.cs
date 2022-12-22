using System.Collections.Generic;
namespace Note.InkCanvasEx.SDK
{
    /// <summary>笔迹数据</summary>
    public class RecognizePath
    {
        /// <summary>识别笔迹点位</summary>
        public List<IRecognizePoint> RecognizePoints { get; set; }
        /// <summary>笔迹id (引擎返回定义)</summary>
        public int StrokeId { get; set; }
        /// <summary>笔迹id</summary>
        public long Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public RecognizePath()
        {
            this.RecognizePoints = new List<IRecognizePoint>();
            this.StrokeId = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="recognizePoints"></param>
        /// <param name="strokeId"></param>
        /// <param name="id"></param>
        public RecognizePath(List<IRecognizePoint> recognizePoints, int strokeId, long id)
        {
            this.RecognizePoints = recognizePoints;
            this.StrokeId = strokeId;
            this.Id = id;
        }
        /// <summary>线条是否为空</summary>
        /// <returns></returns>
        public bool IsEmpty() => this.RecognizePoints == null || this.RecognizePoints.Count == 0;
    }
}
