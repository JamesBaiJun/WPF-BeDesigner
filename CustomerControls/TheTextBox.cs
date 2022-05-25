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
    public class TheTextBox : TextBox, IExecutable
    {
        static TheTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TheTextBox), new FrameworkPropertyMetadata(typeof(TheTextBox)));
        }

        public TheTextBox()
        {
            Width = 80;
            Height = 30;
            Text = "0.01";
            VerticalContentAlignment = VerticalAlignment.Center;
            Style = FindResource("DesignTheTextBox") as Style;
            Focusable = false;
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
                    IsEnabled = true;
                    Focusable = true;
                    Register();
                    Style = null;
                }
            }
        }

        public void Register()
        {
        }
    }
}
