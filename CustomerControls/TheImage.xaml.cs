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
    /// TheImage.xaml 的交互逻辑
    /// </summary>
    public partial class TheImage : Image, IExecutable
    {
        public TheImage()
        {
            InitializeComponent();
            Stretch = Stretch.UniformToFill;
            SetCurrentValue(SourceProperty, new BitmapImage(new Uri("pack://application:,,,/Images/win.png", UriKind.Absolute)));
            Width = 120;
            Height = 40;
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
