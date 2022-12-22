using Note.InkCanvasEx.Commons;
using Note.InkCanvasEx.Models;
using Note.InkCanvasEx.ViewModels;
using System.Collections.ObjectModel;

namespace Note.ViewModels
{
    internal class MoreColorViewModel : BindableBase
    {
        private ObservableCollection<PenColor> backColors = new ObservableCollection<PenColor>();
        public ObservableCollection<PenColor> BackColors
        {
            get => this.backColors;
            set => this.SetProperty(ref this.backColors, value);
        }
        private PenColor selectedBackColor;
        public PenColor SelectedBackColor
        {
            get => this.selectedBackColor;
            set
            {
                this.SetProperty(ref this.selectedBackColor, value);
                var inkCanvaEx = InkCanvasViewModel.InkCanvasEx;
                if (inkCanvaEx != null)
                {
                    inkCanvaEx.ChangeBackground(this.selectedBackColor.Color);
                }
            }
        }
        public MoreColorViewModel()
        {
            this.LoadBackColors();
        }
        private void LoadBackColors()
        {
            BackColors.Add(new PenColor { Color = "#000000" });
            BackColors.Add(new PenColor { Color = "#707070" });
            BackColors.Add(new PenColor { Color = "#B8B8B8" });
            BackColors.Add(new PenColor { Color = "#DCDCDC" });
            BackColors.Add(new PenColor { Color = "#FFFFFF" });
            BackColors.Add(new PenColor { Color = "#A36ED3" });
            BackColors.Add(new PenColor { Color = "#C943C6" });
            BackColors.Add(new PenColor { Color = "#FFC171" });
            BackColors.Add(new PenColor { Color = "#F4EE22" });
            BackColors.Add(new PenColor { Color = "#89E08A" });
            this.SelectedBackColor = new PenColor { Color = "#FFFFFF" };
        }
    }
}
