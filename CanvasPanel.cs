using BeDesigner.Adorners;
using BeDesigner.CustomerControls;
using BeDesigner.Speical;
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace BeDesigner
{
    public class CanvasPanel : Canvas
    {
        public CanvasPanel()
        {
            UseLayoutRounding = true;
            Drop += CanvasPanel_Drop;
            CopySelectItemsCommand = new DelegateCommand(CopySelectItems);
            PasteSelectItemsCommand = new DelegateCommand(PasteSelectItems);
            DeleteSelectItemsCommand = new DelegateCommand(DeleteSelectItems);
            SetTopLayerCommand = new DelegateCommand(SetTopLayer);
            SetBottomLayerCommand = new DelegateCommand(SetBottomLayer);
            SelectedItems = new ObservableCollection<FrameworkElement>();
            ContextMenu = FindResource("CanvasRightMenu") as ContextMenu;

            KeyDown += CanvasPanel_KeyDown;
        }

        #region 添加控件

        private void CanvasPanel_Drop(object sender, DragEventArgs e)
        {
            Type type = e.Data.GetData("PersistentObject") as Type;
            var t = type.GetCustomAttributes(typeof(ControlTypeAttribute), false);
            if (t.Length > 0)
            {
                Console.WriteLine((t as ControlTypeAttribute[])[0].Group);
            }

            try
            {
                var control = Activator.CreateInstance(type) as FrameworkElement;
                control.Name = GetControlName(type);
                Children.Add(control);

                var xPos = e.GetPosition(this).X;
                var yPos = e.GetPosition(this).Y;
                if (xPos % GridPxiel != 0)
                    xPos = (GridPxiel - xPos % GridPxiel) + xPos;
                if (yPos % GridPxiel != 0)
                    yPos = (GridPxiel - yPos % GridPxiel) + yPos;

                SetLeft(control, xPos);
                SetTop(control, yPos);

                SelectedItems = new ObservableCollection<FrameworkElement>() { control };
            }
            catch (Exception)
            {
            }

        }



        string GetControlName(Type ctrlType)
        {
            var children = Children.GetEnumerator();
            children.Reset();
            List<string> names = new List<string>();
            while (children.MoveNext())
            {
                if (children.Current.GetType().Name == ctrlType.Name)
                {
                    names.Add((children.Current as FrameworkElement).Name);
                }
            }

            var nameIndex = names.Count;
            while (names.Contains($"{ctrlType.Name.ToLower().Replace("the", string.Empty)}{nameIndex}"))
            {
                nameIndex++;
            }

            return $"{ctrlType.Name.ToLower().Replace("the", string.Empty)}{nameIndex}";
        }
        #endregion

        #region 初始化
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (visualAdded is Border || visualRemoved is Border)
            {
                return;
            }

            if (visualAdded is FrameworkElement ctrl)
            {
                ctrl.PreviewMouseLeftButtonDown += Ctrl_MouseLeftButtonDown;
            }
            if (visualRemoved is FrameworkElement ctr)
            {
                ctr.PreviewMouseLeftButtonDown -= Ctrl_MouseLeftButtonDown;
            }

            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
        #endregion

        #region 单击选中项处理
        /// <summary>
        /// 单击了控件时：不调用框选的刷新方式
        /// </summary>
        bool isClickedControl = false;
        private void Ctrl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement ctl)
            {
                var cp = GetParentObject<CanvasPanel>(ctl);
                if (Keyboard.Modifiers == ModifierKeys.Control)
                {
                    cp.SelectedItems.Add(ctl);
                }
                else
                {
                    cp.SelectedItems = new ObservableCollection<FrameworkElement>() { ctl };
                }
                isClickedControl = true;

                RefreshSelection();
            }
        }

        public static T GetParentObject<T>(DependencyObject obj) where T : FrameworkElement
        {
            DependencyObject parent = VisualTreeHelper.GetParent(obj);
            while (parent != null)
            {
                if (parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }
        #endregion

        #region 右键菜单
        #endregion

        #region 绘制选择框
        Border selectionBorder = new Border()
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#557F7F7F")),
            BorderThickness = new Thickness(1),
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF303030")),
        };

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            selectionStart = e.GetPosition(this);
            if (!this.Children.Contains(selectionBorder))
            {
                this.Children.Add(selectionBorder);
                this.CaptureMouse();
            }
            
        }

        Point selectionStart = default;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (isClickedControl)
            {
                return;
            }
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var nowPoint = e.GetPosition(this);

                var offsetX = nowPoint.X - selectionStart.X;
                var offsetY = nowPoint.Y - selectionStart.Y;
                Clear();

                selectionBorder.Width = Math.Abs(offsetX);
                selectionBorder.Height = Math.Abs(offsetY);
                // 分四种情况绘制
                if (offsetX >= 0 && offsetY >= 0)// 右下
                {
                    SetLeft(selectionBorder, selectionStart.X);
                    SetTop(selectionBorder, selectionStart.Y);
                }
                else if (offsetX > 0 && offsetY < 0)// 右上
                {
                    SetLeft(selectionBorder, selectionStart.X);
                    SetBottom(selectionBorder, ActualHeight - selectionStart.Y);

                }
                else if (offsetX < 0 && offsetY > 0)// 左下
                {
                    SetRight(selectionBorder, ActualWidth - selectionStart.X);
                    SetTop(selectionBorder, selectionStart.Y);
                }
                else if (offsetX < 0 && offsetY < 0)// 左上
                {
                    SetRight(selectionBorder, ActualWidth - selectionStart.X);
                    SetBottom(selectionBorder, ActualHeight - selectionStart.Y);
                }


            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (double.IsNaN(GetLeft(selectionBorder)))
            {
                SetLeft(selectionBorder, ActualWidth - GetRight(selectionBorder) - selectionBorder.ActualWidth);
            }
            if (double.IsNaN(GetTop(selectionBorder)))
            {
                SetTop(selectionBorder, ActualHeight - GetBottom(selectionBorder) - selectionBorder.ActualHeight);
            }

            FrameSelection(GetLeft(selectionBorder), GetTop(selectionBorder), selectionBorder.Width, selectionBorder.Height);
            selectionBorder.Width = 0;
            selectionBorder.Height = 0;
            this.Children.Remove(selectionBorder);
            this.ReleaseMouseCapture();
        }

        private void Clear()
        {
            SetLeft(selectionBorder, double.NaN);
            SetRight(selectionBorder, double.NaN);
            SetTop(selectionBorder, double.NaN);
            SetBottom(selectionBorder, double.NaN);
        }


        #endregion

        #region 框选
        public ObservableCollection<FrameworkElement> SelectedItems
        {
            get { return (ObservableCollection<FrameworkElement>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<FrameworkElement>), typeof(CanvasPanel), new PropertyMetadata(null, OnSelectedItemsChanged));

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as CanvasPanel)?.RefreshSelection();

        public void RefreshSelection()
        {
            foreach (var item in Children)
            {
                if (!(item is IExecutable))
                    continue;

                var ele = item as FrameworkElement;
                if (ele == null) continue;

                var layer = AdornerLayer.GetAdornerLayer(ele);
                var arr = layer.GetAdorners(ele);//获取该控件上所有装饰器，返回一个数组
                if (arr != null)
                {
                    for (int i = arr.Length - 1; i >= 0; i--)
                    {
                        layer.Remove(arr[i]);
                    }
                }
            }

            if (SelectedItems != null)
            {
                foreach (var item in SelectedItems)
                {
                    var layer = AdornerLayer.GetAdornerLayer(item);
                    layer.Add(new SelectionAdorner(item));
                }
            }
        }

        /// <summary>
        /// 移除所有选择装饰器
        /// </summary>
        public void ClearSelection()
        {
            foreach (var item in Children)
            {
                if (!(item is IExecutable))
                    continue;

                var ele = item as FrameworkElement;
                if (ele == null) continue;

                var layer = AdornerLayer.GetAdornerLayer(ele);
                var arr = layer.GetAdorners(ele);//获取该控件上所有装饰器，返回一个数组
                if (arr != null)
                {
                    for (int i = arr.Length - 1; i >= 0; i--)
                    {
                        layer.Remove(arr[i]);
                    }
                }
            }
        }

        /// <summary>
        /// 计算框选项
        /// </summary>
        private void FrameSelection(double x, double y, double width, double height)
        {
            if (width > 0 || height > 0)
            {
                isClickedControl = false;
            }
            if (isClickedControl)
            {
                isClickedControl = false;
                SelectedItems = new ObservableCollection<FrameworkElement>();
                return;
            }
            SelectedItems = new ObservableCollection<FrameworkElement>();
            foreach (var item in Children)
            {
                if (item is FrameworkElement ctrl && !(ctrl is Border))
                {
                    // 框左上角
                    var left = GetLeft(ctrl);
                    var top = GetTop(ctrl);
                    if (left >= x && left <= x + width && top >= y && top <= y + height)
                    {
                        if (!SelectedItems.Contains(ctrl))
                            SelectedItems.Add(ctrl);
                    }

                    // 框右下角
                    var right = left + ctrl.ActualWidth;
                    var bottom = top + ctrl.ActualHeight;
                    if (right >= x && right <= x + width && bottom >= y && bottom <= y + height)
                    {
                        if (!SelectedItems.Contains(ctrl))
                            SelectedItems.Add(ctrl);
                    }

                    // 框右上角
                    if (right >= x && right <= x + width && top >= y && top <= y + height)
                    {
                        if (!SelectedItems.Contains(ctrl))
                            SelectedItems.Add(ctrl);
                    }

                    // 框左下角
                    if (left >= x && left <= x + width && bottom >= y && bottom <= y + height)
                    {
                        if (!SelectedItems.Contains(ctrl))
                            SelectedItems.Add(ctrl);
                    }
                }
            }

            RefreshSelection();
        }
        #endregion

        #region 外部调用
        public List<FrameworkElement> GetAllExecChildren()
        {
            List<FrameworkElement> result = new List<FrameworkElement>();
            foreach (var ctrl in Children)
            {
                if (ctrl is IExecutable exec)
                {
                    result.Add(exec as FrameworkElement);
                }
            }

            return result;
        }

        /// <summary>
        /// 对齐像素单位
        /// </summary>
        public int GridPxiel
        {
            get { return (int)GetValue(GridPxielProperty); }
            set { SetValue(GridPxielProperty, value); }
        }
        public static readonly DependencyProperty GridPxielProperty =
            DependencyProperty.Register("GridPxiel", typeof(int), typeof(CanvasPanel), new PropertyMetadata(4));


        public void MoveControls(double offsetX, double offsetY)
        {
            ClearAlignLine();

            // 获取可对齐的点
            List<Point> points = new List<Point>();
            foreach (FrameworkElement ctrl in Children)
            {
                if (!SelectedItems.Contains(ctrl))
                {
                    // 左上的点
                    Point item = new Point(GetLeft(ctrl), GetTop(ctrl));
                    points.Add(item);

                    // 左下的点
                    Point itemlb = new Point(GetLeft(ctrl), GetTop(ctrl) + ctrl.ActualHeight);
                    points.Add(itemlb);

                    // 右下的点
                    Point itemrb = new Point(GetLeft(ctrl) + ctrl.ActualWidth, GetTop(ctrl) + ctrl.ActualHeight);
                    points.Add(itemrb);

                    // 右上的点
                    Point itemrt = new Point(GetLeft(ctrl) + ctrl.ActualWidth, GetTop(ctrl));
                    points.Add(itemrt);
                }
            }

            // 控件移动
            foreach (var item in SelectedItems)
            {
                var moveX = GetLeft(item) + offsetX;
                moveX = (moveX < 0) ? 0 : moveX;

                var moveY = GetTop(item) + offsetY;
                moveY = moveY < 0 ? 0 : moveY;

                if (moveX % GridPxiel != 0)
                    moveX = (GridPxiel - moveX % GridPxiel) + moveX;
                if (moveY % GridPxiel != 0)
                    moveY = (GridPxiel - moveY % GridPxiel) + moveY;

                SetLeft(item, moveX);
                SetTop(item, moveY);
            }

            // 计算是否显示对齐线
            var targetItemTop = SelectedItems.Min(x => GetTop(x));
            var targetItem = SelectedItems.FirstOrDefault(x => GetTop(x) == targetItemTop);
            var lefAlign = points.FirstOrDefault(x => Math.Abs(x.X - GetLeft(targetItem)) == 0);
            if (lefAlign != default)
            {
                //SetLeft(targetItem, lefAlign.X);
                var layer = AdornerLayer.GetAdornerLayer(this);
                layer.Add(new SelectionAlignLine(this, lefAlign, new Point(GetLeft(targetItem), GetTop(targetItem))));
            }

            var topAlign = points.FirstOrDefault(x => Math.Abs(x.Y - GetTop(targetItem)) == 0);
            if (topAlign != default)
            {
                //SetTop(targetItem, topAlign.Y);
                var layer = AdornerLayer.GetAdornerLayer(this);
                layer.Add(new SelectionAlignLine(this, topAlign, new Point(GetLeft(targetItem), GetTop(targetItem))));
            }

            int px = 20;
            // 网格对齐
            if (UseAutoAlignment)
            {
                foreach (var item in SelectedItems)
                {
                    var left = GetLeft(item);
                    if (left % px <= 1)
                    {
                        SetLeft(item, (int)left / px * px);
                    }

                    var top = GetTop(item);
                    if (top % px <= 1)
                    {
                        SetTop(item, (int)top / px * px);
                    }
                }
            }
        }

        /// <summary>
        /// 清除绘制的对齐线
        /// </summary>
        public void ClearAlignLine()
        {
            var arr = AdornerLayer.GetAdornerLayer(this).GetAdorners(this);
            if (arr != null)
            {
                for (int i = arr.Length - 1; i >= 0; i--)
                {
                    AdornerLayer.GetAdornerLayer(this).Remove(arr[i]);
                }
            }
        }

        public void ZoomControls(int offsetX, int offsetY)
        {
            foreach (var item in SelectedItems)
            {
                if (item.ActualHeight + offsetY > 10)
                {
                    item.Height += offsetY;
                }
                if (item.ActualWidth + offsetX > 10)
                {
                    item.Width += offsetX;
                }
            }
        }
        #endregion

        #region 对齐操作
        /// <summary>
        /// 是否使用网格对齐 10px
        /// </summary>
        public bool UseAutoAlignment
        {
            get { return (bool)GetValue(UseAutoAlignmentProperty); }
            set { SetValue(UseAutoAlignmentProperty, value); }
        }
        public static readonly DependencyProperty UseAutoAlignmentProperty =
            DependencyProperty.Register("UseAutoAlignment", typeof(bool), typeof(CanvasPanel), new PropertyMetadata(false));


        public void AlignLeft()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var leftMin = SelectedItems.Min(x => Canvas.GetLeft(x));
            foreach (var item in SelectedItems)
            {
                SetLeft(item, leftMin);
            }
        }

        public void AlignRight()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var rightMax = SelectedItems.Max(x => GetLeft(x) + x.ActualWidth);
            foreach (var item in SelectedItems)
            {
                var targetLeft = rightMax - item.ActualWidth;
                SetLeft(item, targetLeft);
            }
        }

        public void AlignCenter()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var leftmin = SelectedItems.Min(x => GetLeft(x));
            var rightmax = SelectedItems.Max(x => GetLeft(x) + x.ActualWidth);
            var center = (rightmax - leftmin) / 2 + leftmin;

            foreach (var item in SelectedItems)
            {
                var target = center - (item.ActualWidth / 2);
                SetLeft(item, target);
            }
        }

        public void AlignTop()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var topMin = SelectedItems.Min(x => GetTop(x));
            foreach (var item in SelectedItems)
            {
                SetTop(item, topMin);
            }
        }

        public void AlignBottom()
        {
            if (SelectedItems == null || SelectedItems.Count == 0)
                return;

            var botMax = SelectedItems.Max(x => GetTop(x) + x.ActualHeight);
            foreach (var item in SelectedItems)
            {
                var targetLeft = botMax - item.ActualHeight;
                SetTop(item, targetLeft);
            }
        }

        public void VertialLayout()
        {
            if (SelectedItems == null || SelectedItems.Count < 3)
                return;

            var topCtl = SelectedItems.Min(x => GetTop(x) + x.ActualHeight);
            var botCtrl = SelectedItems.Max(x => GetTop(x));
            var emptyHeight = botCtrl - topCtl;

            var orderCtrl = SelectedItems.OrderBy(x => GetTop(x)).ToList();
            orderCtrl.RemoveAt(0);
            orderCtrl.RemoveAt(orderCtrl.Count - 1);
            var useSpace = orderCtrl.Sum(x => x.ActualHeight);

            var ableSpaceAvg = (emptyHeight - useSpace) / (SelectedItems.Count - 1);
            double nowPostion = topCtl;
            foreach (var item in orderCtrl)
            {
                SetTop(item, nowPostion + ableSpaceAvg);
                nowPostion += item.ActualHeight + ableSpaceAvg;
            }
        }

        public void HorizontalLayout()
        {
            if (SelectedItems == null || SelectedItems.Count < 3)
                return;

            var leftCtl = SelectedItems.Min(x => GetLeft(x) + x.ActualWidth);
            var rightCtrl = SelectedItems.Max(x => GetLeft(x));
            var emptyHeight = rightCtrl - leftCtl;

            var orderCtrl = SelectedItems.OrderBy(x => GetLeft(x)).ToList();
            orderCtrl.RemoveAt(0);
            orderCtrl.RemoveAt(orderCtrl.Count - 1);
            var useSpace = orderCtrl.Sum(x => x.ActualWidth);

            var ableSpaceAvg = (emptyHeight - useSpace) / (SelectedItems.Count - 1);
            double nowPostion = leftCtl;
            foreach (var item in orderCtrl)
            {
                SetLeft(item, nowPostion + ableSpaceAvg);
                nowPostion += item.ActualWidth + ableSpaceAvg;
            }
        }
        #endregion

        #region 按键操作
        public DelegateCommand CopySelectItemsCommand { get; set; }
        public DelegateCommand PasteSelectItemsCommand { get; set; }
        public DelegateCommand DeleteSelectItemsCommand { get; set; }
        public DelegateCommand SetTopLayerCommand { get; set; }
        public DelegateCommand SetBottomLayerCommand { get; set; }

        List<FrameworkElement> copyTemp = new List<FrameworkElement>();

        private void CanvasPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                CopySelectItems();
            }
            else if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                PasteSelectItems();
            }
            else if (e.Key == Key.Delete)
            {
                DeleteSelectItems();
            }
            else if (e.Key == Key.Up)
            {
                var offset = -1;
                foreach (var item in SelectedItems)
                {
                    SetTop(item, (GetTop(item) + offset) < 0 ? 0 : GetTop(item) + offset);
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                var offset = 1;
                foreach (var item in SelectedItems)
                {
                    SetTop(item, (GetTop(item) + offset) < 0 ? 0 : GetTop(item) + offset);
                }
                e.Handled = true;
            }
            else if (e.Key == Key.Left)
            {
                var offset = -1;
                foreach (var item in SelectedItems)
                {
                    SetLeft(item, (GetLeft(item) + offset) < 0 ? 0 : GetLeft(item) + offset);
                }
                e.Handled = true;

            }
            else if (e.Key == Key.Right)
            {
                var offset = 1;
                foreach (var item in SelectedItems)
                {
                    SetLeft(item, (GetLeft(item) + offset) < 0 ? 0 : GetLeft(item) + offset);
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        public void CopySelectItems()
        {
            copyTemp.Clear();
            foreach (var item in SelectedItems)
            {
                copyTemp.Add(item);
            }
        }

        /// <summary>
        /// 粘贴
        /// </summary>
        public void PasteSelectItems()
        {
            SelectedItems.Clear();
            foreach (var item in copyTemp)
            {
                var xml = XamlWriter.Save(item);
                var element = XamlReader.Parse(xml) as FrameworkElement;
                element.Name += "_1";

                SetLeft(element, GetLeft(element) + 10);
                SetTop(element, GetTop(element) + 10);

                Children.Add(element);
                SelectedItems.Add(element);
            }

            // 将复制的内容替换 以便处理连续复制
            copyTemp.Clear();
            foreach (var item in SelectedItems)
            {
                copyTemp.Add(item);
            }
            RefreshSelection();
        }

        /// <summary>
        /// 置于顶层
        /// </summary>
        public void SetTopLayer()
        {
            if (SelectedItems.Count == 0)
                return;

            foreach (var item in SelectedItems)
            {
                Children.Remove(item);
            }

            foreach (var item in SelectedItems)
            {
                Children.Add(item);
            }
        }

        /// <summary>
        /// 置于底层
        /// </summary>
        public void SetBottomLayer()
        {
            if (SelectedItems.Count == 0)
                return;

            foreach (var item in SelectedItems)
            {
                Children.Remove(item);
            }

            foreach (var item in SelectedItems)
            {
                Children.Insert(0, item);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteSelectItems()
        {
            foreach (var item in SelectedItems)
            {
                Children.Remove(item);
            }

            SelectedItems.Clear();
            RefreshSelection();
        }
        #endregion

        #region 运行Xaml 保存 读取
        public List<FrameworkElement> Generator()
        {
            List<FrameworkElement> elements = new List<FrameworkElement>();
            foreach (var item in Children)
            {
                // 排除非自定义的控件们
                if (!(item is IExecutable))
                    continue;

                var xml = XamlWriter.Save(item);
                var ele = XamlReader.Parse(xml) as FrameworkElement;
                elements.Add(ele);
            }

            return elements;
        }

        /// <summary>
        /// 保存数据到文本
        /// </summary>
        public string Save()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Children)
            {
                var xml = XamlWriter.Save(item);
                sb.Append(xml + "\r\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public void Load(string path)
        {
            Children.Clear();
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Unicode))
            {
                while (sr.Peek() > -1)
                {
                    string str = sr.ReadLine();
                    var ele = XamlReader.Parse(str) as FrameworkElement;
                    Children.Add(ele);
                }
            }

            SelectedItems?.Clear();
        }
        #endregion
    }
}
