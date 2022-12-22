using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Note.InkCanvasEx.Controls
{
    public class UniformGrid : Panel
    {
        private bool _ignorePropertyChange;
        private int _rows;
        private int _columns;
        #region public int Rows
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        public static readonly DependencyProperty RowsProperty =
            DependencyProperty.Register(
                "Rows",
                typeof(int),
                typeof(UniformGrid),
                new PropertyMetadata(0, OnRowsColumnsChanged));
        #endregion
        #region public int Columns
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public static readonly DependencyProperty ColumnsProperty =
            DependencyProperty.Register(
                "Columns",
                typeof(int),
                typeof(UniformGrid),
                new PropertyMetadata(0, OnRowsColumnsChanged));
        #endregion
        #region public int FirstColumn
        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
        }
        public static readonly DependencyProperty FirstColumnProperty =
            DependencyProperty.Register(
                "FirstColumn",
                typeof(int),
                typeof(UniformGrid),
                new PropertyMetadata(0, OnFirstColumnChanged));
        #endregion
        private static void OnRowsColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UniformGrid source = (UniformGrid)d;
            int value = (int)e.NewValue;
            if (source._ignorePropertyChange)
            {
                source._ignorePropertyChange = false;
                return;
            }
            if (value < 0)
            {
                source._ignorePropertyChange = true;
                source.SetValue(e.Property, (int)e.OldValue);
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Properties.Resources.UniformGrid_RowsColumnsChanged_InvalidValue",
                    value);
                throw new ArgumentException(message, "value");
            }
            source.InvalidateMeasure();
        }
        private static void OnFirstColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UniformGrid source = (UniformGrid)d;
            int value = (int)e.NewValue;
            if (source._ignorePropertyChange)
            {
                source._ignorePropertyChange = false;
                return;
            }
            if (value < 0)
            {
                source._ignorePropertyChange = true;
                source.SetValue(e.Property, (int)e.OldValue);
                string message = string.Format(
                    CultureInfo.InvariantCulture,
                    "Properties.Resources.UniformGrid_OnFirstColumnChanged_InvalidValue",
                    value);
                throw new ArgumentException(message, "value");
            }
            source.InvalidateMeasure();
        }
        public UniformGrid()
        {
        }
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "Compat with WPF.")]
        protected override Size MeasureOverride(Size constraint)
        {
            this.UpdateComputedValues();
            Size availableSize = new Size(constraint.Width / (double)this._columns, constraint.Height / (double)this._rows);
            double num = 0.0;
            double num2 = 0.0;
            int i = 0;
            int count = base.Children.Count;
            while (i < count)
            {
                UIElement element = base.Children[i];
                element.Measure(availableSize);
                Size desiredSize = element.DesiredSize;
                if (num < desiredSize.Width)
                {
                    num = desiredSize.Width;
                }
                if (num2 < desiredSize.Height)
                {
                    num2 = desiredSize.Height;
                }
                i++;
            }
            return new Size(num * (double)this._columns, num2 * (double)this._rows);
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect finalRect = new Rect(0.0, 0.0, finalSize.Width / (double)this._columns, finalSize.Height / (double)this._rows);
            double width = finalRect.Width;
            double num = finalSize.Width - 1.0;
            finalRect.X += finalRect.Width * (double)this.FirstColumn;
            foreach (UIElement element in base.Children)
            {
                element.Arrange(finalRect);
                if (element.Visibility != Visibility.Collapsed)
                {
                    finalRect.X += width;
                    if (finalRect.X >= num)
                    {
                        finalRect.Y += finalRect.Height;
                        finalRect.X = 0.0;
                    }
                }
            }
            return finalSize;
        }
        private void UpdateComputedValues()
        {
            this._columns = this.Columns;
            this._rows = this.Rows;
            if (this.FirstColumn >= this._columns)
            {
                this.FirstColumn = 0;
            }
            if (this._rows == 0 || this._columns == 0)
            {
                int num = 0;
                int i = 0;
                int count = base.Children.Count;
                while (i < count)
                {
                    if (base.Children[i].Visibility != Visibility.Collapsed)
                    {
                        num++;
                    }
                    i++;
                }
                if (num == 0)
                {
                    num = 1;
                }
                if (this._rows == 0)
                {
                    if (this._columns > 0)
                    {
                        this._rows = (num + this.FirstColumn + (this._columns - 1)) / this._columns;
                        return;
                    }
                    this._rows = (int)Math.Sqrt((double)num);
                    if (this._rows * this._rows < num)
                    {
                        this._rows++;
                    }
                    this._columns = this._rows;
                    return;
                }
                else if (this._columns == 0)
                {
                    this._columns = (num + (this._rows - 1)) / this._rows;
                }
            }
        }
    }
}
