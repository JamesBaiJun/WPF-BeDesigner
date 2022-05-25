using System;
using System.Collections.Generic;
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
    public class KnobButton : Slider, IExecutable
    {
        static KnobButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KnobButton), new FrameworkPropertyMetadata(typeof(KnobButton)));
        }

        public KnobButton()
        {
            SetCurrentValue(WidthProperty, 150d);
            SetCurrentValue(HeightProperty, 150d);
            SetCurrentValue(MaximumProperty, 100d);
            MouseDown += Path_MouseDown;
            MouseMove += Path_MouseMove;
            MouseWheel += Path_MouseWheel;
            MouseLeftButtonUp += KnobButton_MouseLeftButtonUp;
            Update();
        }

        #region 设计需要
        public string ControlType => "控件";

        private bool isExecuteState;
        public bool IsExecuteState
        {
            get { return isExecuteState; }
            set
            {
                isExecuteState = value;
                if (IsExecuteState)
                    Register();
            }
        }

        /// <summary>
        /// 注册需要处理的事件
        /// </summary>
        public void Register()
        {
            ValueChanged += KnobButton_ValueChanged; ;
        }

        private void KnobButton_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Config.RunJsScipt(ValueChangedExecute);
        }
        #endregion

        #region 绘制
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
                line.Stroke = Brushes.Gray;
                line.StrokeThickness = 2;
                line.HorizontalAlignment = HorizontalAlignment.Center;
                line.RenderTransformOrigin = new Point(0.5, 0.5);
                line.RenderTransform = new RotateTransform() { Angle = -140 + i * 28 };
                bdGrid.Children.Add(line);
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
                    line.Stroke = Brushes.Gray;
                    line.StrokeThickness = 1;
                    line.HorizontalAlignment = HorizontalAlignment.Center;
                    line.RenderTransformOrigin = new Point(0.5, 0.5);
                    line.RenderTransform = new RotateTransform() { Angle = start + j * 2.8 };
                    bdGrid.Children.Add(line);
                }
            }
        }
        #endregion
        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            Update();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);
            Update();
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);
            Update();
        }

        public string ValueChangedExecute
        {
            get { return (string)GetValue(ValueChangedExecuteProperty); }
            set { SetValue(ValueChangedExecuteProperty, value); }
        }
        public static readonly DependencyProperty ValueChangedExecuteProperty =
            DependencyProperty.Register("ValueChangedExecute", typeof(string), typeof(KnobButton), new PropertyMetadata(string.Empty));

        public int Step
        {
            get { return (int)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(int), typeof(KnobButton), new PropertyMetadata(1));

        RotateTransform rotatevalue;
        Grid bdGrid;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rotatevalue = GetTemplateChild("rotatevalue") as RotateTransform;
            bdGrid = GetTemplateChild("bdGrid") as Grid;
            Update();
            InitTick();
        }

        private void Update()
        {
            if (rotatevalue == null) return;
            double perangle = 280 / (Maximum - Minimum);
            double angle = (perangle * (Value-Minimum)) + 40;

            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromMilliseconds(350));
            da.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            da.To = angle;

            rotatevalue.Angle = angle;
            rotatevalue.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        Point lastpoint;
        private void Path_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) return;

            CaptureMouse();
            Point point = e.GetPosition(this);
            double xmove = point.X - lastpoint.X;
            double ymove = point.Y - lastpoint.Y;

            double changeValue = (xmove + ymove) / 10 * Step;
            if ((changeValue + Value) > Maximum)
            {
                if (Value < Maximum)
                {
                    Value = Maximum;
                }
                return;
            }

            if ((changeValue + Value) < Minimum)
            {
                if (Value > Minimum)
                {
                    Value = Minimum;
                }
                return;
            }

            Value = changeValue + Value;
            lastpoint = point;
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released) return;
            lastpoint = e.GetPosition(this);
        }

        private void KnobButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
        }

        private void Path_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double changeValue = (e.Delta / 120) * Step;
            if ((changeValue + Value) > Maximum)
            {
                if (Value < Maximum)
                {
                    Value = Maximum;
                }
                return;
            }

            if ((changeValue + Value) < Minimum)
            {
                if (Value > Minimum)
                {
                    Value = Minimum;
                }
                return;
            }

            Value = Value + changeValue;
            Update();
        }
    }
}
