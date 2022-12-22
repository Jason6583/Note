using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Note.Services.YouDao;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.ViewModels;
using System.Threading.Tasks;

namespace Note.ViewModels
{
    public class TextRecognizeViewModel : BindableBase
    {
        private InkCanvasViewModel _inkCanvasEx;
        private string text;
        public string Text
        {
            get => this.text;
            set => this.SetProperty(ref this.text, value);
        }
        private string translateResult;
        public string TranslateResult
        {
            get => this.translateResult;
            set => this.SetProperty(ref this.translateResult, value);
        }
        private bool isTranslated;
        public bool IsTranslated
        {
            get => this.isTranslated;
            set => this.SetProperty(ref this.isTranslated, value);
        }
        private ICommand translateCommand;
        public ICommand TranslateCommand
        {
            get => this.translateCommand ?? (this.translateCommand = new RelayCommand(async () =>
            {
                try
                {
                    await Task.Run(async () =>
                    {
                        var result = await YouDaoApiService.GetWordsAsync(this.Text);
                        if (result != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.TranslateResult = result.YouDaoTranslation.FirstTranslation.FirstOrDefault();
                                this.IsTranslated = true;
                            });
                        }
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }));
        }
        private ICommand copyCommand;
        public ICommand CopyCommand
        {
            get => this.copyCommand ?? (this.copyCommand = new RelayCommand(() =>
            {
                try
                {
                    if(this.TranslateResult != null)
                    {
                        Clipboard.SetText(this.TranslateResult);
                        this._inkCanvasEx.ToastNotify("复制成功");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }));
        }
        public TextRecognizeViewModel(string text,InkCanvasViewModel inkCanvasEx,string translate=null)
        {
            Text = text;
            TranslateResult = translate;
            _inkCanvasEx = inkCanvasEx;
        }
    }
}
