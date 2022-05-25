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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeDesigner.CustomerControls
{
    public class StatusLight : Control, IExecutable
    {
        public StatusLight()
        {
            Width = 80;
            Height = 80;
        }
        public string ControlType => "控件";

        static StatusLight()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StatusLight), new FrameworkPropertyMetadata(typeof(StatusLight)));
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

        /// <summary>
        /// 状态值
        /// </summary>
        [Category("值设定")]
        public int StatusValue
        {
            get { return (int)GetValue(StatusValueProperty); }
            set { SetValue(StatusValueProperty, value); }
        }
        public static readonly DependencyProperty StatusValueProperty =
            DependencyProperty.Register("StatusValue", typeof(int), typeof(StatusLight), new UIPropertyMetadata(0, OnStatusChanged));

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as StatusLight).Refresh();

        private void Refresh()
        {
            if (image != null)
            {
                switch (StatusValue)
                {
                    case 0:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/State0.png", UriKind.Absolute));
                        break;
                    case -1:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/State11.png", UriKind.Absolute));
                        break;
                    case 1:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/State1.png", UriKind.Absolute));
                        break;
                    case 2:
                        image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/State2.png", UriKind.Absolute));
                        break;
                    default:
                        break;
                }
            }
        }

        Image image;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            image = GetTemplateChild("ima") as Image;
            Refresh();
        }

        public void Register()
        {
        }
    }
}
