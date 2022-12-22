using Note.Models;
using System.Linq;
using Note.Events;
using System.Windows.Input;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.ViewModels;
using System.Collections.ObjectModel;

namespace Note.ViewModels
{
    /// <summary>
    /// 考虑充分利用SelectByPolyLine算法优化选择逻辑
    /// </summary>
    internal class EraserViewModel : BindableBase
    {
        private IEventBus _eventBus;
        private InkCanvasViewModel _inkCanvasEx;
        private ObservableCollection<EraserSize> eraserSizes = new ObservableCollection<EraserSize>();
        public ObservableCollection<EraserSize> EraserSizes
        {
            get => this.eraserSizes;
            set => this.SetProperty(ref this.eraserSizes, value);
        }
        private EraserSize selectedEraserSize;
        public EraserSize SelectedEraserSize
        {
            get => this.selectedEraserSize;
            set
            {
                this.SetProperty(ref this.selectedEraserSize, value);
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ChangeEraserSize(this.selectedEraserSize.Size / 2);
                }
            }
        }
        private ICommand clearCommand;
        public ICommand ClearCommand
        {
            get => this.clearCommand ?? (this.clearCommand = new RelayCommand(() =>
            {
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ClearContent();
                }
                this._eventBus.Publish(new ClearTextEventArgs());
            }));
        }
        public EraserViewModel(InkCanvasViewModel inkCanvasEx)
        {
            this._inkCanvasEx = inkCanvasEx;
            var eraserSize = new EraserSize();
            eraserSize.Size = 30;
            eraserSize.IconSize = 3;
            this.EraserSizes.Add(eraserSize);
            eraserSize = new EraserSize();
            eraserSize.Size = 40;
            eraserSize.IconSize = 6;
            this.EraserSizes.Add(eraserSize);
            eraserSize = new EraserSize();
            eraserSize.IconSize = 9;
            eraserSize.Size = 60;
            this.EraserSizes.Add(eraserSize);
            eraserSize = new EraserSize();
            eraserSize.IconSize = 12;
            eraserSize.Size = 80;
            this.EraserSizes.Add(eraserSize);
            eraserSize = new EraserSize();
            eraserSize.IconSize = 18;
            eraserSize.Size = 100;
            this.EraserSizes.Add(eraserSize);
            this.SelectedEraserSize = this.EraserSizes.FirstOrDefault();
            this._eventBus = this._inkCanvasEx.EventBus;
        }
    }
}
