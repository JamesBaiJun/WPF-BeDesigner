using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace BeDesigner.CustomerControls
{
    /// <summary>
    /// 波浪进度条
    /// </summary>
    [TemplatePart(Name = ElementWave, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ElementClip, Type = typeof(FrameworkElement))]
    public class WaveProgressBar : RangeBase, IExecutable
    {
        private const string ElementWave = "PART_Wave";

        private const string ElementClip = "PART_Clip";

        private FrameworkElement _waveElement;

        private const double TranslateTransformMinY = -20;

        private double _translateTransformYRange;

        private TranslateTransform _translateTransform;

        public WaveProgressBar()
        {
            Loaded += (s, e) => UpdateWave(Value);
            SetCurrentValue(WidthProperty, 200d);
            SetCurrentValue(HeightProperty, 200d);
            SetCurrentValue(ValueProperty, 50d);
        }

        static WaveProgressBar()
        {
            FocusableProperty.OverrideMetadata(typeof(WaveProgressBar),
                new FrameworkPropertyMetadata(ValueBoxes.FalseBox));
            MaximumProperty.OverrideMetadata(typeof(WaveProgressBar),
                new FrameworkPropertyMetadata(ValueBoxes.Double100Box));
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            UpdateWave(newValue);
        }

        private void UpdateWave(double value)
        {
            if (_translateTransform == null || IsVerySmall(Maximum)) return;
            var scale = 1 - value / Maximum;
            var y = _translateTransformYRange * scale + TranslateTransformMinY;
            _translateTransform.Y = y;
        }

        public static bool IsVerySmall(double value) => Math.Abs(value) < 1E-06;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _waveElement = GetTemplateChild(ElementWave) as FrameworkElement;
            var clipElement = GetTemplateChild(ElementClip) as FrameworkElement;

            if (_waveElement != null && clipElement != null)
            {
                _translateTransform = new TranslateTransform
                {
                    Y = clipElement.Height
                };
                _translateTransformYRange = clipElement.Height - TranslateTransformMinY;
                _waveElement.RenderTransform = new TransformGroup
                {
                    Children = { _translateTransform }
                };
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(WaveProgressBar), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText", typeof(bool), typeof(WaveProgressBar), new PropertyMetadata(ValueBoxes.TrueBox));

        public bool ShowText
        {
            get => (bool)GetValue(ShowTextProperty);
            set => SetValue(ShowTextProperty, ValueBoxes.BooleanBox(value));
        }

        public static readonly DependencyProperty WaveFillProperty = DependencyProperty.Register(
            "WaveFill", typeof(Brush), typeof(WaveProgressBar), new PropertyMetadata(default(Brush)));

        public Brush WaveFill
        {
            get => (Brush)GetValue(WaveFillProperty);
            set => SetValue(WaveFillProperty, value);
        }

        public static readonly DependencyProperty WaveThicknessProperty = DependencyProperty.Register(
            "WaveThickness", typeof(double), typeof(WaveProgressBar), new PropertyMetadata(ValueBoxes.Double0Box));

        public double WaveThickness
        {
            get => (double)GetValue(WaveThicknessProperty);
            set => SetValue(WaveThicknessProperty, value);
        }

        public static readonly DependencyProperty WaveStrokeProperty = DependencyProperty.Register(
            "WaveStroke", typeof(Brush), typeof(WaveProgressBar), new PropertyMetadata(default(Brush)));

        public Brush WaveStroke
        {
            get => (Brush)GetValue(WaveStrokeProperty);
            set => SetValue(WaveStrokeProperty, value);
        }

        public string ControlType => "控件";

        private bool isExecuteState;
        public bool IsExecuteState
        {
            get { return isExecuteState; }
            set
            {
                isExecuteState = value;
                if (IsExecuteState)
                {
                    Register();
                }
            }
        }

        public void Register()
        {
        }
    }

    internal static class ValueBoxes
    {
        internal static object TrueBox = true;

        internal static object FalseBox = false;

        internal static object Double0Box = .0;

        internal static object Double01Box = .1;

        internal static object Double1Box = 1.0;

        internal static object Double10Box = 10.0;

        internal static object Double20Box = 20.0;

        internal static object Double100Box = 100.0;

        internal static object Double200Box = 200.0;

        internal static object Double300Box = 300.0;

        internal static object DoubleNeg1Box = -1.0;

        internal static object Int0Box = 0;

        internal static object Int1Box = 1;

        internal static object Int2Box = 2;

        internal static object Int5Box = 5;

        internal static object Int99Box = 99;

        internal static object BooleanBox(bool value) => value ? TrueBox : FalseBox;
    }
}
