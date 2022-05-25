using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    [TemplatePart(Name = ELLIPSE, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = TranslateX, Type = typeof(TranslateTransform))]
    public class SwitchButton : ToggleButton, IExecutable
    {
        public SwitchButton()
        {
            SetCurrentValue(WidthProperty, 120d);
            SetCurrentValue(HeightProperty, 40d);
        }

        public const string ELLIPSE = "ELLIPSE";
        public const string TranslateX = "TranslateX";
        static SwitchButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SwitchButton), new FrameworkPropertyMetadata(typeof(SwitchButton)));
        }

        /// <summary>
        /// 不勾选时执行代码
        /// </summary>
        [Category("事件")]
        public string UnCheckedExec
        {
            get { return (string)GetValue(UnCheckedExecProperty); }
            set { SetValue(UnCheckedExecProperty, value); }
        }
        public static readonly DependencyProperty UnCheckedExecProperty =
            DependencyProperty.Register("UnCheckedExec", typeof(string), typeof(SwitchButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 勾选时执行代码
        /// </summary>
        [Category("事件")]
        public string CheckedExec
        {
            get { return (string)GetValue(CheckedExecProperty); }
            set { SetValue(CheckedExecProperty, value); }
        }
        public static readonly DependencyProperty CheckedExecProperty =
            DependencyProperty.Register("CheckedExec", typeof(string), typeof(SwitchButton), new PropertyMetadata(string.Empty));

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            ellipse.Width = Height - 8;
            ellipse.Height = Height - 8;
            Refresh();
        }

        TranslateTransform transX;
        Ellipse ellipse;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ellipse = GetTemplateChild(ELLIPSE) as Ellipse;
            transX = GetTemplateChild(TranslateX) as TranslateTransform;
        }
        protected override void OnChecked(RoutedEventArgs e)
        {
            base.OnChecked(e);
            Refresh();
        }
        protected override void OnUnchecked(RoutedEventArgs e)
        {
            base.OnUnchecked(e);
            Refresh();
        }

        void Refresh()
        {
            if (ellipse == null)
            {
                return;
            }
            DoubleAnimation da = new DoubleAnimation();
            da.Duration = new Duration(TimeSpan.FromMilliseconds(250));
            da.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            if (IsChecked == true)
            {
                da.To = ActualWidth - ellipse.ActualWidth - 5;
                ellipse.SetCurrentValue(Ellipse.FillProperty, Background);
            }
            else
            {
                da.To = 3;
                ellipse.SetCurrentValue(Ellipse.FillProperty, Brushes.Gray);
            }

            transX.BeginAnimation(TranslateTransform.XProperty, da);
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

        public void Register()
        {
            Checked += TheCheckBox_Checked;
            Unchecked += TheCheckBox_Unchecked;
        }
        private void TheCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.RunJsScipt(UnCheckedExec);
        }

        private void TheCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Config.RunJsScipt(CheckedExec);
        }
        public string ControlType => "控件";

    }
}
