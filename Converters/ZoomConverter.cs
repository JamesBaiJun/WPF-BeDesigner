using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BeDesigner.Converters
{
    public class ZoomConverter : IValueConverter
    {
        public bool IsHeight { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out double zoom))
            {
                return IsHeight ? zoom * 1080 : zoom * 1920;
            }
            else
            {
                return IsHeight ? 1080 : 1920;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
