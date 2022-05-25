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
using System.Windows.Threading;

namespace BeDesigner.CustomerControls
{
    public class TheTimer : Control, IExecutable, IDisposable
    {
        public TheTimer()
        {
            Width = 40;
            Height = 40;
        }
        static TheTimer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TheTimer), new FrameworkPropertyMetadata(typeof(TheTimer)));
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
                    Style = null;
                }
            }
        }

        DispatcherTimer timer = new DispatcherTimer();
        public void Register()
        {
            timer.Interval = TimeSpan.FromMilliseconds(Interval);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Config.RunJsScipt(TikcExecute);
        }

        public void Start()=> timer.Start();
        public void Stop()=> timer.Stop();
        
        public void Dispose()
        {
            timer.Stop();
        }

        /// <summary>
        /// 时间间隔
        /// </summary>
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(TheTimer), new PropertyMetadata(0));


        /// <summary>
        /// 执行内容
        /// </summary>
        [Category("事件")]
        public string TikcExecute
        {
            get { return (string)GetValue(TikcExecuteProperty); }
            set { SetValue(TikcExecuteProperty, value); }
        }
        public static readonly DependencyProperty TikcExecuteProperty =
            DependencyProperty.Register("TikcExecute", typeof(string), typeof(TheTimer), new PropertyMetadata(string.Empty));
    }
}
