using System;
using System.Numerics;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    public sealed partial class SelectionRect : UserControl
    {
        private bool _isResizing = false;
        public EventHandler CloseHandler;
        public SelectionRect()
        {
            this.InitializeComponent();
        }
        private void CloseImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.CloseHandler?.Invoke(sender, new EventArgs());
        }
        private void Manipulator_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.Width = this.ActualWidth;
            this.Height = this.ActualHeight;
            if (e.Position.X > Width - resizer.Width && e.Position.Y > Height - resizer.Height)
                _isResizing = true;
            else
                _isResizing = false;
        }
        public static Matrix4x4 ToMatrix4x4(Matrix3x2 matrix)
        {
            return new Matrix4x4(
               matrix.M11, matrix.M12, 0, 0,
               matrix.M21, matrix.M22, 0, 0,
               0, 0, 1, 0,
               matrix.M31, matrix.M32, 0, 1);
        }
        private void Manipulator_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (_isResizing)
            {
                Width += e.Delta.Translation.X;
                Height += e.Delta.Translation.Y;
            }
            else
            {
                var scale = Matrix3x2.CreateScale(e.Delta.Scale);
                Matrix3x2 transformX;
                transformX = Matrix3x2.CreateTranslation((float)-e.Position.X, (float)-e.Position.Y) *
                             scale *
                             Matrix3x2.CreateTranslation((float)e.Position.X, (float)e.Position.Y) *
                             Matrix3x2.CreateTranslation((float)(e.Delta.Translation.X), (float)(e.Delta.Translation.Y));
                this.TransformMatrix *= ToMatrix4x4(transformX);
            }
        }
        private void ContainerGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {

        }
        private void ContainerGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {

        }
    }
}
