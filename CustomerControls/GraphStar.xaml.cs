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
    /// GraphStar.xaml 的交互逻辑
    /// </summary>
    public partial class GraphStar : UserControl, IExecutable
    {
        public GraphStar()
        {
            InitializeComponent();
            Width = 100;
            Height = 100;
        }

        public string ControlType => "图形";

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
