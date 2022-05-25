using BeDesigner.Speical;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// TheComboBox.xaml 的交互逻辑
    /// </summary>
    public partial class TheComboBox : ComboBox, IExecutable
    {
        public TheComboBox()
        {
            InitializeComponent();
            VerticalContentAlignment = VerticalAlignment.Center;
            ItemsString = new ItemsList() { "AA", "BB" };
            Style = FindResource("DesignComboBox") as Style;
            Width = 80;
            Height = 30;
            Focusable = false;
        }
        public string ControlType => "控件";

        public ItemsList ItemsString
        {
            get { return (ItemsList)GetValue(ItemsStringProperty); }
            set { SetValue(ItemsStringProperty, value); }
        }

        public static readonly DependencyProperty ItemsStringProperty =
            DependencyProperty.Register("ItemsString", typeof(ItemsList), typeof(TheComboBox), new PropertyMetadata(null));

        private bool isExecuteState;
        public bool IsExecuteState
        {
            get { return isExecuteState; }
            set
            {
                isExecuteState = value;
                if (IsExecuteState)
                {
                    Focusable = true;
                    IsEnabled = true;
                    Register();
                    Style = null;
                }
            }
        }

        public void Register()
        {
            // 运行时进行项目绑定
            Binding binding = new Binding();
            binding.RelativeSource = new RelativeSource() { Mode = RelativeSourceMode.Self };
            binding.Path = new PropertyPath("ItemsString");
            SetBinding(ItemsSourceProperty, binding);
        }
    }
}
