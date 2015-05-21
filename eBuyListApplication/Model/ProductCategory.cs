using System.Windows.Media.Imaging;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class ProductCategory
    {
        private int _id;
        private ProductCategoryIds _categoryId;
        private string _name;
        private BitmapImage _icon;

        public int Id
        {
            get { return _id; }
        }

        public ProductCategoryIds CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public BitmapImage Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public ProductCategory()
        {
        }

        public ProductCategory(ProductCategoryIds categoryId, string name, BitmapImage icon)
            : this(0, categoryId, name, icon)
        {
        }

        public ProductCategory(int id, string name, BitmapImage icon)
            : this(id, ProductCategoryIds.USER, name, icon)
        {
        }

        private ProductCategory(int id, ProductCategoryIds categoryId, string name, BitmapImage icon)
        {
            _id = id;
            _categoryId = categoryId;
            _name = name;
            _icon = icon;
        }

        public ProductCategory Clone()
        {
            return new ProductCategory(_id, _categoryId, _name, _icon);
        }
    }
}
