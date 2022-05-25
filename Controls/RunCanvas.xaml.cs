using BeDesigner.CustomerControls;
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

namespace BeDesigner.Controls
{
    /// <summary>
    /// RunCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class RunCanvas : UserControl
    {
        public RunCanvas()
        {
            InitializeComponent();
            Unloaded += (s,e)=> Destory();
        }

        /// <summary>
        /// Dispose子集
        /// </summary>
        public void Destory()
        {
            foreach (var item in RootCanvas.Children)
            {
                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        public void Run(List<FrameworkElement> canvas)
        {
            RootCanvas.Children.Clear();
            foreach (FrameworkElement element in canvas)
            {
                if (element is IExecutable executable)
                    executable.IsExecuteState = true;

                RootCanvas.Children.Add(element);
                RegisterJsName(element);
            }
        }

        // 注册名称到Js
        static void RegisterJsName(FrameworkElement element)
        {
            Config.SetVariable(element.Name, element);
            if (element is Panel panel)
            {
                foreach (var item in panel.Children)
                {
                    RegisterJsName(item as FrameworkElement);
                }
            }
        }

        #region 拖动与缩放
        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragEnable.IsChecked == false)
            {
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed && isPressed)
            {
                Point point = e.GetPosition(this);
                var movex = (point.X - last.X);
                var movey = (point.Y - last.Y);

                Translate.X += movex;
                Translate.Y += movey;
                last = point;

            }
        }

        bool isPressed = false;
        Point last;//记录上次鼠标坐标位置
        private void RootCanvas_MouseLeftButtoDown(object sender, MouseButtonEventArgs e)
        {
            last = e.GetPosition(this);
            isPressed = true;
        }

        private void RootCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isPressed = false;
        }

        // 缩放
        private void RootCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (ZoomEnable.IsChecked == false)
            {
                return;
            }
            var zoomS = (e.Delta / 960d);
            var zoom = zoomS + Scale.ScaleX;
            if (zoom > 3 || zoom < 0.8)
            {
                return;
            }
            Scale.ScaleX = Scale.ScaleY = zoom;

            Point mouse = e.GetPosition(RootCanvas);
            Point newMouse = new Point(mouse.X * zoomS, mouse.Y * zoomS);

            Translate.X -= newMouse.X;
            Translate.Y -= newMouse.Y;
        }
        #endregion
    }
}
