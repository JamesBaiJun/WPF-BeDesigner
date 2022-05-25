using BeDesigner.Speical;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeDesigner.CustomerControls
{
    public class ArcGauge : Control, IExecutable
    {
        public ArcGauge()
        {
            Width = 300;
            Height = 300;
            SetCurrentValue(ValueProperty, 0d);
            SetCurrentValue(MinValueProperty, 0d);
            SetCurrentValue(MaxValueProperty, 100d);
        }

        private void InitTick()
        {
            // 画大刻度
            for (int i = 0; i < 11; i++)
            {
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = 0;
                line.X2 = 0;
                line.Y2 = 12;
                line.Stroke = Brushes.White;
                line.StrokeThickness = 2;
                line.HorizontalAlignment = HorizontalAlignment.Center;
                line.RenderTransformOrigin = new Point(0.5, 0.5);
                line.RenderTransform = new RotateTransform() { Angle = -140 + i * 28 };
                bdGrid.Children.Add(line);
                DrawText();
            }

            // 画小刻度
            for (int i = 0; i < 10; i++)
            {
                var start = -140 + 28 * i + 2.8;
                for (int j = 0; j < 9; j++)
                {
                    Line line = new Line();
                    line.X1 = 0;
                    line.Y1 = 0;
                    line.X2 = 0;
                    line.Y2 = 6;
                    line.Stroke = Brushes.White;
                    line.StrokeThickness = 1;
                    line.HorizontalAlignment = HorizontalAlignment.Center;
                    line.RenderTransformOrigin = new Point(0.5, 0.5);
                    line.RenderTransform = new RotateTransform() { Angle = start + j * 2.8 };
                    bdGrid.Children.Add(line);
                }
            }
        }

        List<TextBlock> textLabels = new List<TextBlock>();
        private void DrawText()
        {
            foreach (var item in textLabels)
            {
                bdGrid.Children.Remove(item);
            }
            textLabels.Clear();

            var per = MaxValue / 10;
            for (int i = 0; i < 11 ; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = $"{MinValue + (per * i)}";
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.RenderTransformOrigin = new Point(0.5, 0.5);
                textBlock.RenderTransform = new RotateTransform() { Angle = -140 + i * 28 };
                textBlock.Margin = new Thickness(12);
                textBlock.Foreground = Brushes.White;
                bdGrid.Children.Add(textBlock);
                textLabels.Add(textBlock);
            }
        }

        static ArcGauge()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ArcGauge), new FrameworkPropertyMetadata(typeof(ArcGauge)));
        }

        RotateTransform rotateTransform;
        Grid bdGrid;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rotateTransform = GetTemplateChild("PointRotate") as RotateTransform;
            bdGrid = GetTemplateChild("bdGrid") as Grid;
            Refresh();
            InitTick();
        }

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

        [Category("值设定")]
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(ArcGauge), new PropertyMetadata(0d, OnValueChanged));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as ArcGauge)?.Refresh();

        [Category("值设定")]
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(ArcGauge), new PropertyMetadata(0d, OnValueChanged));

        [Category("值设定")]
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public string ControlType => "控件";

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(ArcGauge), new PropertyMetadata(0d, OnValueChanged));


        private void Refresh()
        {
            if (rotateTransform == null)
                return;
            DrawText();
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromMilliseconds(350));
            da.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

            if (Value > MaxValue)
            {
                rotateTransform.Angle = 140;
                da.To = 140;
            }
            else if (Value < MinValue)
            {
                rotateTransform.Angle = -140;
                da.To = -140;
            }
            else
            {
                var range = MaxValue - MinValue;
                var process = Value / range;
                var tAngle = process * 280 - 140;
                rotateTransform.Angle = tAngle;
                da.To = tAngle;
            }

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        public void Register()
        {
            Refresh();
        }
    }
}
