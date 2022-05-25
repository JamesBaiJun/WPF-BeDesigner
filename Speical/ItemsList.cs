using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BeDesigner.Speical
{
    /// <summary>
    /// 因ObservableCollection无法序列化，继承并实例化一个
    /// </summary>
    public class ItemsList : ObservableCollection<string>
    {
        public ItemsList()
        {
            AddCommand = new DelegateCommand<string>(AddItem);
            DeleteCommand = new DelegateCommand<string>(DeleteItem);
        }

        private void DeleteItem(string obj)
        {
            if (!string.IsNullOrEmpty(obj))
                Remove(obj);
        }

        public DelegateCommand<string> AddCommand { get; }
        public DelegateCommand<string> DeleteCommand { get; }
        private void AddItem(string txt)
        {
            if (!string.IsNullOrEmpty(txt))
                Add(txt);
        }
    }
}
