using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    internal class ImageButton : Button
    {
        public ImageSource NormalImage
        {
            get { return (ImageSource)GetValue(NormalImageProperty); }
            set { SetValue(NormalImageProperty, value); }
        }
        public static readonly DependencyProperty NormalImageProperty =
            DependencyProperty.Register("NormalImage",
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null));
        public ImageSource CheckedImage
        {
            get { return (ImageSource)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }
        public static readonly DependencyProperty CheckedImageProperty =
            DependencyProperty.Register("CheckedImage",
                typeof(ImageSource),
                typeof(ImageButton),
                new PropertyMetadata(null));
        public ImageButton()
        {

        }
    }
}
