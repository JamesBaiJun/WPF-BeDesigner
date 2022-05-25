using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BeDesigner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Assembly assembly = Assembly.GetExecutingAssembly();
            var controls = assembly.GetTypes().Where(t => t.GetInterface("IExecutable") != null);
            CtlList.ItemsSource = controls;

        }


        private void AglinLeftBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.AlignLeft();
        }

        private void AglinBottomBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.AlignBottom();
        }

        private void AglinTopBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.AlignTop();
        }

        private void AglinRightBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.AlignRight();
        }

        private void AglinCenterBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.AlignCenter();
        }

        private void VerticalLayoutBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.VertialLayout();
        }

        private void HorizontalLayoutBtn_Click(object sender, RoutedEventArgs e)
        {
            cav.HorizontalLayout();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "布局文件|*.lay";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                cav.Load(ofd.FileName);
            }

            DoubleAnimation da = new DoubleAnimation(-200, 0, new Duration(TimeSpan.FromMilliseconds(250)));
            da.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            CanvasTranslate.BeginAnimation(TranslateTransform.XProperty, da);

            DoubleAnimation daop = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(250)));
            daop.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            cav.BeginAnimation(OpacityProperty,daop);
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "布局文件|*.lay";

            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string str = cav.Save();
                File.WriteAllText(sfd.FileName, str, Encoding.Unicode);
            }
        }

        private void CtlList_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (CtlList.SelectedItem != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(CtlList, CtlList.SelectedItem, System.Windows.DragDropEffects.Copy);
            }
        }

        private void RunBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
            {
                if (btn.Tag.ToString() == "运行")
                {
                    cav.ClearSelection();
                    runCanvas.Run(cav.Generator());
                }
                else if (btn.Tag.ToString() == "停止")
                {
                    runCanvas.Destory();
                }
            }
            
        }
    }
}
