using eBuyListApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;


namespace eBuyListApplication.ValueConverters
{
    public class IsBoughtToFontStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListProductItem product = (ListProductItem)value;

            //FontStyle myStyleBought = FontStyles.Italic;
            //FontStyle myStyleToBuy = FontStyles.Normal;

            if (product.IsBought)
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
