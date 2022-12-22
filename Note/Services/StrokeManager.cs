using System;
using System.Linq;
using Newtonsoft.Json;
using Note.InkCanvasEx.SDK;
using Windows.UI.Input.Inking;
using System.Collections.Generic;

namespace Note.Services
{
    public class StrokeManager
    {
        private List<RecognizePath> _recognizePaths;
        public StrokeManager()
        {
            _recognizePaths = new List<RecognizePath>();
        }
        public RecognizePath AddPointPath(InkStroke stroke)
        {
            RecognizePath recognizePath = new RecognizePath();
            List<IRecognizePoint> recognizePoints = new List<IRecognizePoint>();
            if (stroke.GetInkPoints() == null || stroke.GetInkPoints().Count == 0)
                return recognizePath;
            recognizePath.Id = stroke.GetHashCode();
            foreach (var point in stroke.GetInkPoints())
            {
                recognizePoints.Add(new RecognizePoint((float)point.Position.X, (float)point.Position.Y, 0, (int)ActionEvent.MOVE));
            }
            recognizePoints.Last().Action = (int)ActionEvent.UP;
            recognizePoints.First().Action = (int)ActionEvent.DOWN;
            recognizePath.RecognizePoints = recognizePoints;
            _recognizePaths.Add(recognizePath);
            return recognizePath;
        }
        public RecognizePath GetPointPath(InkStroke stroke)
        {
            var query = _recognizePaths.Where(p => p.Id == stroke.GetHashCode());
            if (query.Count() > 0)
            {
                RecognizePath recognizePath = query.First();
                List<IRecognizePoint> recognizePoints = new List<IRecognizePoint>();
                if (stroke.GetInkPoints() == null || stroke.GetInkPoints().Count == 0) return recognizePath;
                recognizePath.Id = stroke.GetHashCode();
                foreach (var item in stroke.GetInkPoints())
                {
                    recognizePoints.Add(new RecognizePoint((float)item.Position.X, (float)item.Position.Y, 0, (int)ActionEvent.MOVE));
                }
                recognizePoints.Last().Action = (int)ActionEvent.UP;
                recognizePoints.First().Action = (int)ActionEvent.DOWN;
                recognizePath.RecognizePoints = recognizePoints;
                return recognizePath;
            }
            return null;
        }
        public int RemovePointPath(InkStroke stroke)
        {
            int strokeId = 0;
            var query = _recognizePaths.Where(p => p.Id == stroke.GetHashCode());
            if (query.Count() > 0)
            {
                strokeId = query.First().StrokeId;
                _recognizePaths.Remove(query.First());
            }
            return strokeId;
        }
        public string ExportJsonText()
        {
            if (_recognizePaths.Count > 0)
            {
                List<object> paths = new List<object>();
                foreach (var path in _recognizePaths)
                {
                    List<object> points = new List<object>();
                    foreach (var pt in path.RecognizePoints)
                    {
                        points.Add(new
                        {
                            eventType = ((ActionEvent)pt.Action).ToString(),
                            t = pt.T,
                            x = pt.X,
                            y = pt.Y
                        });
                    }
                    paths.Add(new
                    {
                        mId = path.StrokeId,
                        mPoints = points
                    });
                }
                return JsonConvert.SerializeObject(paths);
            }
            return null;
        }
        public List<InkStroke> GetStrokes(string content)
        {
            List<InkStroke> strokes = new List<InkStroke>();
            try
            {
                var strokeBuilder = new InkStrokeBuilder();
                var pointList = JsonConvert.DeserializeObject<List<dynamic>>(content);
                if (pointList != null)
                {
                    foreach (var item in pointList)
                    {
                        var inkPoints = new List<InkPoint>();
                        foreach (var p in item.mPoints)
                        {
                            var inkPoint = new InkPoint(new Windows.Foundation.Point(Convert.ToDouble(p.x), Convert.ToDouble(p.y)), 0.5f);
                            inkPoints.Add(inkPoint);
                        }
                        var inkstroke= strokeBuilder.CreateStrokeFromInkPoints(inkPoints, System.Numerics.Matrix3x2.Identity);
                        strokes.Add(inkstroke);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return strokes;
        }
        public void Clear()
        {
            _recognizePaths.Clear();
        }
    }
}
