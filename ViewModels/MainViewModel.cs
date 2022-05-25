using BeDesigner.Views;
using DevExpress.Mvvm.DataAnnotations;
using ICSharpCode.AvalonEdit;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeDesigner.ViewModels
{
    [POCOViewModel]
    public class MainViewModel
    {
        public MainViewModel()
        {

        }

        /// <summary>
        /// 是否正在运行状态
        /// </summary>
        public virtual bool IsRunning { get; set; }

        /// <summary>
        /// 当前代码Xaml
        /// </summary>
        public virtual string XamlCode { get; set; }

        public void RunUi()
        {
            IsRunning = !IsRunning;
        }

        /// <summary>
        /// 显示当前xaml代码
        /// </summary>
        public void ShowCode(TextEditor textEditor)
        {
            textEditor.Text = canvasPanel.Save();
        }

        #region 脚本编辑数据
        public CanvasPanel canvasPanel;
        public void Loaded(object obj)
        {
            canvasPanel = obj as CanvasPanel;
        }

        public void Edit(object obj)
        {
            string result = JsEditWindow.ShowEdit(((DevExpress.Xpf.Editors.EditableDataObject)obj).Value.ToString(), canvasPanel.GetAllExecChildren());
            ((DevExpress.Xpf.Editors.EditableDataObject)obj).Value = result;
        }
        #endregion

        #region 图片选择路径
        public void SelectPath(object obj)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
            if (ofd.ShowDialog() == true)
            {
                ((DevExpress.Xpf.Editors.EditableDataObject)obj).Value = ofd.FileName;
            }
        }
        #endregion
    }
}
