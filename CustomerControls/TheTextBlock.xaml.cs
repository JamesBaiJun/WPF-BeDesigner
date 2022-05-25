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
    /// <summary>
    /// TheTextBlock.xaml 的交互逻辑
    /// </summary>
    public partial class TheTextBlock : TextBlock, IExecutable
    {
        public TheTextBlock()
        {
            InitializeComponent();
            Text = "文本块";
            Width = 80;
            Height = 30;
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
}
