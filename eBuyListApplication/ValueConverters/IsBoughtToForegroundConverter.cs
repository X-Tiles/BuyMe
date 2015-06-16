using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using eBuyListApplication.Model;
using System.Windows;

namespace eBuyListApplication.ValueConverters
{
    public class IsBoughtToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListProductItem product = (ListProductItem)value;

            if (product.IsBought)
                return 0.5;
            else
                return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
