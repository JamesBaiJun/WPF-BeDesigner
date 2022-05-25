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
    public class TheRadioButton : RadioButton, IExecutable, IDisposable
    {
        public TheRadioButton()
        {
            SetCurrentValue(ContentProperty, "单选按钮");
        }
        static TheRadioButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TheRadioButton), new FrameworkPropertyMetadata(typeof(TheRadioButton)));
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

        public void Register()
        {
            
        }

        public void Dispose()
        {
        }
    }
}
