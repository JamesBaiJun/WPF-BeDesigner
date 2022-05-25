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
    /// <summary>
    /// TheButton.xaml 的交互逻辑
    /// </summary>
    public partial class TheButton : Button, IExecutable
    {
        public TheButton()
        {
            InitializeComponent();
            Content = "按钮";
            Width = 80;
            Height = 30;
            Style = FindResource("DesignButton") as Style;
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
                    Style = null;
                    Register();
                }
            }
        }

        [Category("事件")]
        public string ClickExec
        {
            get { return (string)GetValue(ClickExecProperty); }
            set { SetValue(ClickExecProperty, value); }
        }
        public static readonly DependencyProperty ClickExecProperty =
            DependencyProperty.Register("ClickExec", typeof(string), typeof(TheButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 注册需要处理的事件
        /// </summary>
        public void Register()
        {
            this.Click += MyButton_Click;
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            Config.RunJsScipt(ClickExec);
        }
    }
}
