using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using eBuyListApplication.Model;

namespace eBuyListApplication.ValueConverters
{
    public class ProductToCategoryNameConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListProductItem product = (ListProductItem)value;

            var categoryId = product.ProductCategoryId;

            ProductCategories category = new ProductCategories();

            var categoriesNames = category.GetCategoryNameByCategoryId(categoryId);

            return categoriesNames;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
