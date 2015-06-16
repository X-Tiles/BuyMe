using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using eBuyListApplication.Model;
using System.Windows.Media.Imaging;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.ValueConverters
{
    public class ProductToCategoryImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListProductItem product = (ListProductItem)value;

            ProductCategoryIds productCategoryID = (ProductCategoryIds)product.ProductCategoryId;

            ProductCategories categories = new ProductCategories();

            BitmapImage categoryIcon = categories.GetCategoryIconByCategoryId(productCategoryID);

            return categoryIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
