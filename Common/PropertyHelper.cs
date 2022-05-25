using BeDesigner.CustomerControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BeDesigner.Common
{
    /// <summary>
    /// 编辑脚本时获取控件属性
    /// </summary>
    public class PropertyHelper
    {
        public static string[] ableProperties = new string[] { "IsChecked", "Value", "CurValue", "StatusValue", "NumberValue","Text" };
        public static List<ControlName> GetCustomerControlProperty(List<FrameworkElement> selectItems)
        {
            List<ControlName> result = new List<ControlName>();
            foreach (var control in selectItems)
            {
                var typeCtl = control.GetType();
                if (typeCtl.GetProperties().Count(p => ableProperties.Contains(p.Name)) == 0)
                    continue;

                var pare = new ControlName(control.Name, null);
                result.Add(pare);
                pare.Properties = typeCtl.GetProperties()
                                    .Where(p => ableProperties.Contains(p.Name))
                                    .Select(x => new ControlName(x.Name, pare)).ToList();
            }

            return result;
        }
    }

    public class ControlName
    {
        public ControlName(string name, ControlName parent)
        {
            Name = name;
            Parent = parent;
        }

        public ControlName Parent { get; set; }

        public string Name { get; set; }
        public IEnumerable<ControlName> Properties { get; set; }
    }
}
