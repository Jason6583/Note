using System;
using Note.Models;
using Windows.UI.Xaml;
using Windows.Foundation;
using Note.InkCanvasEx.Enums;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Note.Services
{
    public class BorderManager
    {
        private bool _isSplitDisplay = true;
        private bool _isResultDisplay = true;
        private bool _isRowDisplay = true;

        private List<StrokeRect> _rectList = new List<StrokeRect>();

        private Canvas _borderCanvas;
        public BorderManager(Canvas borderCanvas)
        {
            _borderCanvas = borderCanvas;
        }
        public void AddRect(dynamic val)
        {
            Clear();
            AddResultRect(val);
            AddSplitRect(val);
            AddRowRect(val);
        }
        private void AddResultRect(dynamic val)
        {
            if (val.words != null)
            {
                foreach (var c in val.words)
                {
                    if (c.bounding_box == null) continue;

                    var strokeRect = new StrokeRect()
                    {
                        DrawRect = GetRectValue(c),
                        RectType = RectType.Result,
                        WordId = Convert.ToInt32(c.id),
                        Word = c.label.ToString()
                    };
                    if (_isResultDisplay)
                    {
                        _borderCanvas.Children.Add(strokeRect.RectPath);
                    }
                    _rectList.Add(strokeRect);
                }
            }
        }
        private void AddSplitRect(dynamic val)
        {
            if (val.chars != null)
            {
                foreach (var c in val.chars)
                {
                    if (c.bounding_box == null) continue;

                    var strokeRect = new StrokeRect()
                    {
                        DrawRect = GetRectValue(c),
                        RectType = RectType.Split
                    };
                    if (_isSplitDisplay)
                    {
                        _borderCanvas.Children.Add(strokeRect.RectPath);
                    }
                    _rectList.Add(strokeRect);
                }
            }
        }
        private Rect GetRectValue(dynamic c)
        {
            double x = Convert.ToDouble(c.bounding_box.x);
            double y = Convert.ToDouble(c.bounding_box.y);
            double w = Convert.ToDouble(c.bounding_box.width);
            double h = Convert.ToDouble(c.bounding_box.height);
            if (w < 0) w = 0;
            if (h < 0) h = 0;
            return new Rect(x, y, w, h);
        }

        private void AddRowRect(dynamic val)
        {
            if (val.line_box != null)
            {
                foreach (var c in val.line_box)
                {
                    var strokeRect = new StrokeRect()
                    {
                        DrawRect = GetRectValueV2(c),
                        RectType = RectType.Row,
                    };
                    if (_isRowDisplay)
                    {
                        _borderCanvas.Children.Add(strokeRect.RectPath);
                    }
                    _rectList.Add(strokeRect);
                }
            }
        }

        private Rect GetRectValueV2(dynamic c)
        {
            double x = Convert.ToDouble(c.x);
            double y = Convert.ToDouble(c.y);
            double w = Convert.ToDouble(c.width);
            double h = Convert.ToDouble(c.height);
            if (w < 0) w = 0;
            if (h < 0) h = 0;
            return new Rect(x, y, w, h);
        }

        public bool ContainsRect()
        {
            return _rectList.Count > 0;
        }
        public bool Find(string word, RecognizeManager recognizeManager)
        {
            ClearMultiWordRect();

            bool exist = false;
            string label = recognizeManager.Label;
            int index = label.IndexOf(word);
            if (index >= 0)
            {
                if (word.Length == 1)
                {
                    foreach (var item in _rectList)
                    {
                        if (item.RectType == RectType.Result)
                        {
                            if (item.Word == word)
                            {
                                exist = true;
                                item.HighlightFindedPath();
                                if (!_borderCanvas.Children.Contains(item.RectPath))
                                    _borderCanvas.Children.Add(item.RectPath);
                            }
                            else
                            {
                                item.ResetPath();
                                if (!_isResultDisplay)
                                {
                                    _borderCanvas.Children.Remove(item.RectPath);
                                }
                            }
                        }
                    }
                }
                else
                {
                    List<StrokeRect> findList = new List<StrokeRect>();
                    for (int i = 0; i < word.Length; i++)
                    {
                        foreach (var item in _rectList)
                        {
                            if (item.RectType == RectType.Result)
                            {
                                if (item.WordId == index + i)
                                {
                                    exist = true;
                                    findList.Add(item);
                                }
                            }

                            item.ResetPath();
                            if (!_isResultDisplay)
                            {
                                _borderCanvas.Children.Remove(item.RectPath);
                            }
                        }
                    }

                    if (findList.Count > 0)
                    {
                        Rect containRect = findList[0].ActualRect;
                        for (int i = 1; i < findList.Count; i++)
                        {
                            containRect.Union(findList[i].ActualRect);
                        }

                        var strokeRect = new StrokeRect()
                        {
                            DrawRect = containRect,
                            RectType = RectType.Multi,
                            WordId = index,
                            Word = word
                        };
                        _borderCanvas.Children.Add(strokeRect.RectPath);
                        _rectList.Add(strokeRect);
                    }
                }
            }
            return exist;
        }
        private void ClearMultiWordRect()
        {
            List<StrokeRect> deleteList = new List<StrokeRect>();
            foreach (var item in _rectList)
            {
                if (item.RectType == RectType.Multi)
                {
                    _borderCanvas.Children.Remove(item.RectPath);
                    deleteList.Add(item);
                }
            }
            deleteList.ForEach(s => _rectList.Remove(s));
        }
        public void HiddenPath()
        {
            _borderCanvas.Visibility = Visibility.Collapsed;
        }

        public void DisplayPath()
        {
            _borderCanvas.Visibility = Visibility.Visible;
        }
        //public void DisplayPath(int wordId)
        //{
        //    StrokeRect strokeRect = _rectList.Where(r => r.WordId == wordId).FirstOrDefault();
        //    if (strokeRect == null) return;
        //    List<StrokeRect> strokeRects = new List<StrokeRect>();
        //    foreach (var item in _rectList)
        //    {
        //        if (item.DrawRect.Contains(strokeRect.DrawRect))
        //        {
        //            var count = _rectList.Where(r => r != strokeRect && item.DrawRect.Contains(r.DrawRect)).Count();
        //            if (count == 0) 
        //                strokeRects.Add(item);
        //        }
        //    }
        //    foreach (var item in strokeRects)
        //    {
        //        if (!_borderCanvas.Children.Contains(item.RectPath))
        //            _borderCanvas.Children.Add(item.RectPath);
        //    }
        //}
        //public void HiddenPath(int wordId)
        //{
        //    StrokeRect strokeRect = _rectList.Where(r => r.WordId == wordId).FirstOrDefault();
        //    if (strokeRect == null) return;
        //    List<StrokeRect> strokeRects = new List<StrokeRect>();
        //    foreach (var item in _rectList)
        //    {
        //        if (item.DrawRect.Contains(strokeRect.DrawRect))
        //        {
        //            var count = _rectList.Where(r => r != strokeRect && item.DrawRect.Contains(r.DrawRect)).Count();
        //            if (count == 0) strokeRects.Add(item);
        //        }
        //    }
        //    foreach (var item in strokeRects)
        //    {
        //        _borderCanvas.Children.Remove(item.RectPath);
        //    }
        //}
        public void DisplayPath(bool isDisplay, RectType resultRect)
        {
            switch (resultRect)
            {
                case RectType.Split:
                    _isSplitDisplay = isDisplay;
                    break;
                case RectType.Result:
                    _isResultDisplay = isDisplay;
                    break;
                case RectType.Row:
                    _isRowDisplay = isDisplay;
                    break;
                default:
                    break;
            }
            foreach (var item in _rectList)
            {
                if (item.RectType == resultRect)
                {
                    if (isDisplay)
                    {
                        if (!_borderCanvas.Children.Contains(item.RectPath))
                            _borderCanvas.Children.Add(item.RectPath);
                    }
                    else
                    {
                        if (!item.IsFinded)
                        {
                            _borderCanvas.Children.Remove(item.RectPath);
                        }
                    }
                }
            }
        }
        public void Clear()
        {
            _borderCanvas.Children.Clear();
            _rectList.Clear();
        }
    }
}
