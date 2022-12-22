using System;
using Note.Models;
using System.Reflection;
using Note.InkCanvasEx.SDK;
using Windows.UI.Xaml.Controls;
using Note.InkCanvasEx.Controls;
using System.Collections.Generic;
using Note.InkCanvasEx.ViewModels;

namespace Note.Services
{
    public class ShapeManager
    {
        private InkCanvasViewModel _inkCanvasEx;
        private Assembly _assembly = Assembly.GetExecutingAssembly();
        private List<ShapeContainer> _shapes = new List<ShapeContainer>();
        private Canvas _shapeCanvas;
        public ShapeManager(InkCanvasViewModel inkCanvasEx)
        {
            this._inkCanvasEx = inkCanvasEx;
            this._shapeCanvas = inkCanvasEx.ShapeCanvas;
        }
        public Tuple<bool, ShapeType> AddShapes(string result)
        {
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    string[] list = result.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (list.Length > 1)
                    {
                        var type = Convert.ToInt32(list[0]);
                        ShapeType shapeType = (ShapeType)type;
                        var shape = GetShapeType(shapeType);
                        if (shape != null)
                        {
                            shape.Shape = shapeType;
                            shape.ShapeDatas = list;
                            //设置位置
                            var path = shape.ShapePath;
                            if (path != null)
                            {
                                try
                                {
                                    var bounds = shape.Bounds;
                                    var shapeContainer = new ShapeContainer(path, result,bounds);
                                    Canvas.SetLeft(shapeContainer, bounds.Left);
                                    Canvas.SetTop(shapeContainer, bounds.Top);
                                    this._shapeCanvas.Children.Add(shapeContainer);
                                    _shapes.Add(shapeContainer);
                                    shapeContainer.CloseHandler = (s, e) =>
                                    {
                                        this._shapeCanvas.Children.Remove(shapeContainer);
                                    };
                                    //撤消恢复功能：
                                    this._inkCanvasEx.AddToUndoList(shapeContainer);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (shape.ShapePath != null)
                                return new Tuple<bool, ShapeType>(true, shapeType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new Tuple<bool, ShapeType>(false, ShapeType.SHAPE_TYPE_UNKNOWN);
        }
        private ShapeStroke GetShapeType(ShapeType shapeType)
        {
            var shapeStr=shapeType.ToString();
            var typeNames = shapeType.ToString().Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            string[] lastNames = new string[typeNames.Length - 2];
            Array.Copy(typeNames, 2, lastNames, 0, lastNames.Length);
            var shapeModel = ($"Note.Models.{string.Join("", lastNames)}Stroke").ToLower();
            var shapeStroke= _assembly.CreateInstance(shapeModel, true) as ShapeStroke;
            return shapeStroke;
        }
        public void Clear()
        {
            this._shapeCanvas.Children.Clear();
            _shapes.Clear();
        }
    }
}
