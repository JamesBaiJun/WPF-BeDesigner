using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// TheToggleButton.xaml 的交互逻辑
    /// </summary>
    public partial class TheToggleButton : ToggleButton, IExecutable
    {
        public TheToggleButton()
        {
            InitializeComponent();
            Content = "开关";
            Width = 80;
            Height = 30;
            Style = FindResource("DesignToggleButton") as Style;
            VerticalContentAlignment = VerticalAlignment.Center;
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
                    Style = FindResource("ExecuteToggleButton") as Style;

                    Register();
                }
            }
        }

        /// <summary>
        /// 不勾选时执行代码
        /// </summary>
        [Category("事件")]
        public string UnCheckedExec
        {
            get { return (string)GetValue(UnCheckedExecProperty); }
            set { SetValue(UnCheckedExecProperty, value); }
        }
        public static readonly DependencyProperty UnCheckedExecProperty =
            DependencyProperty.Register("UnCheckedExec", typeof(string), typeof(TheToggleButton), new PropertyMetadata(string.Empty));

        /// <summary>
        /// 勾选时执行代码
        /// </summary>
        [Category("事件")]
        public string CheckedExec
        {
            get { return (string)GetValue(CheckedExecProperty); }
            set { SetValue(CheckedExecProperty, value); }
        }
        public static readonly DependencyProperty CheckedExecProperty =
            DependencyProperty.Register("CheckedExec", typeof(string), typeof(TheToggleButton), new PropertyMetadata(string.Empty));

        public void Register()
        {
            Checked += TheCheckBox_Checked;
            Unchecked += TheCheckBox_Unchecked;
        }

        private void TheCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.RunJsScipt(UnCheckedExec);
        }

        private void TheCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Config.RunJsScipt(CheckedExec);
        }
    }
}
