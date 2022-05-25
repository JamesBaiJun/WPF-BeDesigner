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
    public class DigitalNumber : Control, IExecutable
    {
        public DigitalNumber()
        {
            Width = 80;
            Height = 30;
        }
        public string ControlType => "控件";

        static DigitalNumber()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DigitalNumber), new FrameworkPropertyMetadata(typeof(DigitalNumber)));
        }

        TextBlock text = null;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            text = GetTemplateChild("line") as TextBlock;
        }

        public double NumberValue
        {
            get { return (double)GetValue(NumberValueProperty); }
            set { SetValue(NumberValueProperty, value); }
        }
        public static readonly DependencyProperty NumberValueProperty =
            DependencyProperty.Register("NumberValue", typeof(double), typeof(DigitalNumber), new UIPropertyMetadata(0.00d));

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
}
