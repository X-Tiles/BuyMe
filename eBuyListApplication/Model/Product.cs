using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class Product
    {
        protected ProductIds _productId;
        protected string _name;

        protected int _categoryId;
        protected ProductCategoryIds _productCategoryId;

        public ProductIds ProductId
        {
            get { return _productId; }
            set { _productId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int CategoryId
        {
            get { return _categoryId; }
            set { _categoryId = value; }
        }

        public ProductCategoryIds ProductCategoryId
        {
            get { return _productCategoryId; }
            set { _productCategoryId = value; }
        }

        public Product()
        {
        }

        public Product(ProductIds productId, string name, ProductCategoryIds productCategoryId) 
            : this(productId, name, 0, productCategoryId)
        {
        }

        public override string ToString()
        {
            return _name;
        }

        public Product(ProductIds productId, string name, int categoryId, ProductCategoryIds productCategoryId)
        {
            _productId = productId;
            _name = name;
            _categoryId = categoryId;
            _productCategoryId = productCategoryId;
        }

        public Product Clone()
        {
            return new Product(_productId, _name, _categoryId, _productCategoryId);
        }
    }
}
