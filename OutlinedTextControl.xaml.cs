using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Space_Shooters
{
    public partial class OutlinedTextControl : UserControl
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(OutlinedTextControl), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(OutlinedTextControl), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(object), typeof(OutlinedTextControl), new FrameworkPropertyMetadata(Brushes.White, OnStrokePropertyChanged));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(OutlinedTextControl), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsRender));

        public OutlinedTextControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public object? Stroke
        {
            get { return GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        private static void OnStrokePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (OutlinedTextControl)d;
            if (e.NewValue is SolidColorBrush || e.NewValue is GradientBrush)
            {
                // Accept SolidColorBrush or GradientBrush directly
                control.Stroke = e.NewValue;
            }
            else if (e.NewValue is string stringValue)
            {
                // Convert hex color string to SolidColorBrush
                control.Stroke = (SolidColorBrush)new BrushConverter().ConvertFrom(stringValue);
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (Text == null || Text.Length == 0)
                return;

            // Create formatted text for the text content
            FormattedText formattedText = new(
                Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily, FontStyle, FontWeight, FontStretches.Normal),
                FontSize,
                Fill,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);

            // Create geometry for the outline based on the formatted text
            Geometry textGeometry = formattedText.BuildGeometry(new Point(0, 0));

            // Create a pen for the outline
            if (Stroke is SolidColorBrush solidColorBrush)
            {
                Pen pen = new(solidColorBrush, StrokeThickness);
                drawingContext.DrawGeometry(null, pen, textGeometry);
            }
            else if (Stroke is GradientBrush gradientBrush)
            {
                drawingContext.DrawGeometry(gradientBrush, new Pen(), textGeometry);
            }

            // Draw the filled text over the outline
            drawingContext.DrawText(formattedText, new Point(0, 0));
        }
    }
}
