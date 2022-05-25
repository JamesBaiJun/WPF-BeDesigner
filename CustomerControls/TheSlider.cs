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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeDesigner.CustomerControls
{
    public class TheSlider : Slider, IExecutable, IDisposable
    {
        static TheSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TheSlider), new FrameworkPropertyMetadata(typeof(TheSlider)));
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
                    Style = null;
                }
            }
        }

        public string ValueChangedExecute
        {
            get { return (string)GetValue(ValueChangedExecuteProperty); }
            set { SetValue(ValueChangedExecuteProperty, value); }
        }
        public static readonly DependencyProperty ValueChangedExecuteProperty =
            DependencyProperty.Register("ValueChangedExecute", typeof(string), typeof(TheSlider), new PropertyMetadata(string.Empty));


        public void Register()
        {
            ValueChanged += TheSlider_ValueChanged;
        }

        private void TheSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Config.RunJsScipt(ValueChangedExecute);
        }

        public void Dispose()
        {
        }
    }
}
