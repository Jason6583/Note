using System;
using System.Linq;
using System.Windows.Input;
using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Events;
using Note.InkCanvasEx.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Note.InkCanvasEx.ViewModels
{
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
            eraserSize.CheckedIcon = new BitmapImage(new Uri("ms-appx:///Assets/stroke1_hover3.png", UriKind.Absolute));
            eraserSize.Icon = new BitmapImage(new Uri("ms-appx:///Assets/stroke1_3.png", UriKind.Absolute));
            this.EraserSizes.Add(eraserSize);
            
            eraserSize = new EraserSize();
            eraserSize.Size = 40;
            eraserSize.IconSize = 6;
            eraserSize.CheckedIcon = new BitmapImage(new Uri("ms-appx:///Assets/stroke2_3.png", UriKind.Absolute));
            eraserSize.Icon = new BitmapImage(new Uri("ms-appx:///Assets/stroke2_hover3.png", UriKind.Absolute));
            this.EraserSizes.Add(eraserSize);
            
            eraserSize = new EraserSize();
            eraserSize.IconSize = 9;
            eraserSize.Size = 60;
            eraserSize.CheckedIcon = new BitmapImage(new Uri("ms-appx:///Assets/stroke3_3.png", UriKind.Absolute));
            eraserSize.Icon = new BitmapImage(new Uri("ms-appx:///Assets/stroke3_hover3.png", UriKind.Absolute));
            this.EraserSizes.Add(eraserSize);
            
            eraserSize = new EraserSize();
            eraserSize.IconSize = 12;
            eraserSize.Size = 80;
            eraserSize.CheckedIcon = new BitmapImage(new Uri("ms-appx:///Assets/stroke4_3.png", UriKind.Absolute));
            eraserSize.Icon = new BitmapImage(new Uri("ms-appx:///Assets/stroke4_hover3.png", UriKind.Absolute));
            this.EraserSizes.Add(eraserSize);
            
            eraserSize = new EraserSize();
            eraserSize.IconSize = 18;
            eraserSize.Size = 100;
            eraserSize.CheckedIcon = new BitmapImage(new Uri("ms-appx:///Assets/stroke5_3.png", UriKind.Absolute));
            eraserSize.Icon = new BitmapImage(new Uri("ms-appx:///Assets/stroke5_hover3.png", UriKind.Absolute));
            this.EraserSizes.Add(eraserSize);
            this.SelectedEraserSize = this.EraserSizes.FirstOrDefault();
            this._eventBus = this._inkCanvasEx.EventBus;
        }
    }
}
