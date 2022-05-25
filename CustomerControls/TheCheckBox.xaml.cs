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
    /// TheCheckBox.xaml 的交互逻辑
    /// </summary>
    public partial class TheCheckBox : CheckBox, IExecutable
    {
        public TheCheckBox()
        {
            InitializeComponent();
            Content = "勾选框";
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
            DependencyProperty.Register("UnCheckedExec", typeof(string), typeof(TheCheckBox), new PropertyMetadata(string.Empty));

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
            DependencyProperty.Register("CheckedExec", typeof(string), typeof(TheCheckBox), new PropertyMetadata(string.Empty));

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
