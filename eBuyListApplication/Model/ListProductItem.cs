using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class ListProductItem : Product
    {
        private int _count;
        private UnitIds _unit;
        private bool _isBought;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public UnitIds Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public bool IsBought
        {
            get { return _isBought; }
            set { _isBought = value; }
        }

        public ListProductItem()
        {
        }

        public ListProductItem(Product product)
            : this(product.ProductId, product.CategoryId, product.ProductCategoryId, product.Name)
        {
        }

        public ListProductItem(ProductIds productId, int categoryId, ProductCategoryIds productCategoryId, string name)
            : base(productId, name, categoryId, productCategoryId)
        {
        }
    }
}
