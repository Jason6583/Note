using System;
using Newtonsoft.Json;
using Note.InkCanvasEx.SDK;
using Note.InkCanvasEx.Enums;
using System.Collections.Generic;
using Note.InkCanvasEx.ViewModels;
using Note.Models;

namespace Note.Services
{
    public class RecognizeManager
    {
        private string _wordJson = "";
        private string _mathJson = "";
        private string _label = "";
        private bool _isInputMath = false;

        private MathSettingViewModel _mathSetting;
        public MathSettingViewModel MathSetting
        {
            get
            {
                if (_mathSetting == null)
                {
                    _mathSetting = new MathSettingViewModel();
                }
                return _mathSetting;
            }
        }
        public string WordJson
        {
            get =>this._wordJson; 
        }
        public string MathJson
        {
            get => this._mathJson; 
        }
        public string Label
        {
            get => this._label; 
        }
        public bool IsInputMath
        {
            get =>_isInputMath;
        }
        public void Clear()
        {
            _wordJson = "";
            _mathJson = "";
            _label = "";
        }
        public string GetMathValue()
        {
            string result = string.Empty;
            try
            {
                dynamic val = JsonConvert.DeserializeObject<dynamic>(_mathJson);
                if (string.Compare(val.type.ToString(), "math") == 0)
                {
                    if (MathSetting.FractionType == FractionType.Decimals)
                    {
                        result = val.label.ToString();
                    }
                    if (MathSetting.FractionType == FractionType.ProperFraction)
                    {
                        result = val.properFraction.ToString();
                    }
                    if (MathSetting.FractionType == FractionType.ImproperFraction)
                    {
                        result = val.improperFraction.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }
        public FormulaValue GetCalcMathValue(string val)
        {
            string formula = "";
            int mathResultCode;
            List<string> resultList = new List<string>();
            try
            {
                _isInputMath = true;
                _wordJson = "";
                _mathJson = val;
                dynamic data = JsonConvert.DeserializeObject<dynamic>(val);
                if (MathSetting.FractionType == FractionType.Decimals)
                {
                    foreach (var item in data.@decimal)
                    {
                        resultList.Add(item.ToString());
                    }
                }
                if (MathSetting.FractionType == FractionType.ProperFraction)
                {
                    foreach (var item in data.properFraction)
                    {
                        resultList.Add(item.ToString());
                    }
                }
                if (MathSetting.FractionType == FractionType.ImproperFraction)
                {
                    foreach (var item in data.improperFraction)
                    {
                        resultList.Add(item.ToString());
                    }
                }
                foreach (var item in data.mathLabels)
                {
                    if (string.Compare(item.label.ToString(), "\n") == 0) continue;
                    formula += item.label.ToString();
                }
                mathResultCode = Convert.ToInt32(data.resultCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                mathResultCode = (int)MathResultCode.MATH_RESULT_EXPRESSION_ERROR;
            }
            return new FormulaValue(resultList, formula, mathResultCode);
        }
        public RecognizeValue GetRecognizeValue(string content,
            RecognitionMode recognitionMode,
            RecognitionType recognitionType,
            BorderManager borderManager,
            ShapeManager shapeManager)
        {
            string result = "";
            List<CandidateItem> wordItems = new List<CandidateItem>();
            RecognizeResultType recognizeResultType = RecognizeResultType.Text;
            try
            {
                _isInputMath = false;
                if (recognitionType == RecognitionType.Math)
                {
                    this._wordJson = "";
                    this._mathJson = content;
                }
                else
                {
                    this._wordJson = content;
                    this._mathJson = "";
                }
                dynamic val = JsonConvert.DeserializeObject<dynamic>(content);

                if (string.Compare(val.type.ToString(), "math", true) == 0
                    || string.Compare(val.type.ToString(), "math_sunia", true) == 0)
                {
                    if (MathSetting.FractionType == FractionType.Decimals)
                    {
                        result = val.label.ToString();
                    }
                    if (MathSetting.FractionType == FractionType.ProperFraction)
                    {
                        result = val.properFraction.ToString();
                    }
                    if (MathSetting.FractionType == FractionType.ImproperFraction)
                    {
                        result = val.improperFraction.ToString();
                    }
                    recognizeResultType = RecognizeResultType.Math;
                }
                else if (string.Compare(val.type.ToString(), "chem", true) == 0)
                {
                    result = val.label.ToString();
                    recognizeResultType = RecognizeResultType.Math;
                }
                else if (string.Compare(val.type.ToString(), "shape", true) == 0)
                {
                    result = val.label.ToString();
                    recognizeResultType = RecognizeResultType.Shape;
                    var res = shapeManager.AddShapes(result);
                    if(res != null)
                    {
                        result = $"{res.Item2}；{result}";
                    }
                }
                else if (string.Compare(val.type.ToString(), "gesture", true) == 0)
                {
                    result = val.label.ToString();
                    recognizeResultType = RecognizeResultType.Shape;
                    int.TryParse(result, out int value);
                    result = $"{result}-{ value }";
                }
                else
                {
                    result = val.label.ToString();
                    if (recognitionMode == RecognitionMode.MODE_ICR)
                    {
                        if (val.words != null)
                        {
                            foreach (var word in val.words)
                            {
                                string w = word.label.ToString();
                                if (w == "\n" || w == "\r") continue;
                                List<string> candidates = new List<string>();
                                foreach (var candidate in word.candidates)
                                {
                                    candidates.Add(candidate.ToString());
                                }
                                wordItems.Add(new CandidateItem()
                                {
                                    Word = w,
                                    WordId = Convert.ToInt32(word.id),
                                    Candidates = candidates
                                });
                            }
                        }
                    }
                    if (string.Compare(val.type.ToString(), "jianpu", true) == 0)
                    {
                        recognizeResultType = RecognizeResultType.Jianpu;
                    }
                }
                if (recognitionMode == RecognitionMode.MODE_ECR && !string.IsNullOrEmpty(result))
                {
                    borderManager.AddRect(val);
                }
                _label = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new RecognizeValue(result, recognizeResultType, wordItems);
        }
    }
}
