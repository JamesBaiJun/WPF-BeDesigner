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
    /// TheGroupBox.xaml 的交互逻辑
    /// </summary>
    public partial class TheGroupBox : GroupBox, IExecutable
    {
        public TheGroupBox()
        {
            InitializeComponent();
            Width = 150;
            Height = 150;
            Header = "分组";
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

        /// <summary>
        /// 注册需要处理的事件
        /// </summary>
        public void Register()
        {
        }
    }
}
