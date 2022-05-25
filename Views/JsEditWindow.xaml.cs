using BeDesigner.Common;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace BeDesigner.Views
{
    /// <summary>
    /// Interaction logic for JsEditWindow.xaml
    /// </summary>
    public partial class JsEditWindow : ThemedWindow
    {
        public JsEditWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            Closing += (s, e) => { e.Cancel = true; Hide(); };

        }

        public static JsEditWindow Instance { get; } = new JsEditWindow();

        public static string ShowEdit(string script, List<FrameworkElement> selectItems)
        {
            // 获取所有控件的可用属性
            var pros = PropertyHelper.GetCustomerControlProperty(selectItems);
            Instance.Tree.ItemsSource = pros;
            Instance.txtBox.Text = script;
            Instance.ShowDialog();

            if (Instance.IsOk)
            {
                return Instance.txtBox.Text;
            }
            else
            {
                return script;
            }
        }

        public bool IsOk { get; set; }
        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsOk = false;
            Close();
        }

        private void ConfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            IsOk = true;
            Close();
        }

        private void Tree_MouseMove(object sender, MouseEventArgs e)
        {
            if (Tree.SelectedItem != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(Tree, Tree.SelectedItem, System.Windows.DragDropEffects.Copy);
            }
        }

        private void txtBox_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            var data = ((ControlName)e.Data.GetData("BeDesigner.Common.ControlName"));
            var str = $"{(data.Parent == null ? string.Empty : (data.Parent.Name + "."))}{data.Name}";

            var position = txtBox.GetPositionFromPoint(e.GetPosition(txtBox));
            if (position == null)
            {
                txtBox.Text += str;
                return;
            }

            var offset = txtBox.Document.GetOffset(position.Value.Location);
            txtBox.SelectionStart = offset;
            txtBox.Document.Insert(offset, str);
        }

        private void txtBox_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
        }
     
    }
}
