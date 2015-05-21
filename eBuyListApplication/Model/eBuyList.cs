using System;
using System.Collections.Generic;
using System.Linq;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class EBuyList
    {
        private int _id;
        private string _name;
        private string _note = "";
        private List<ListProductItem> _products;
        private TimeNotification _notification;
        private SortMethods _sortMethods = SortMethods.USER;

        public int NumberOfBoughtProducts
        {
            get { return GetNumberOfBoughtProducts(); }
        }

        public int NumberOfProductsToBuy
        {
            get { return GetNumberOfProducts() - GetNumberOfBoughtProducts(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Note
        {
            get { return _note; }
            set { _note = value; }
        }

        public TimeNotification Notification
        {
            get { return _notification; }
            set { _notification = value; }
        }

        public SortMethods SortMethods
        {
            get { return _sortMethods; }
            set { _sortMethods = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public EBuyList(string name)
        {
            _name = name;
            _products = new List<ListProductItem>();
        }

        public EBuyList(int id, string name)
        {
            _name = name;
            _id = id;
            _products = new List<ListProductItem>();
        }

        public List<ListProductItem> Products
        {
            get { return _products; }
        }

        public void AddNewProduct(ListProductItem product)
        {
            _products.Add(product);
        }

        public void Clear()
        {
            _products.Clear();
        }

        public void RemoveProductByIndex(int index)
        {
            _products.RemoveAt(index);
        }

        public void RemoveProductsByIndexes(List<int> indexes)
        {
            var productsToRemove = indexes.Select(index => _products[index]).ToList();
            foreach (var product in productsToRemove)
            {
                _products.Remove(product);
            }
        }

        public void RemoveCheckedProducts()
        {
            _products.RemoveAll(p => p.IsBought);
        }

        public void Sort(SortMethods sortMethod)
        {
            throw new NotImplementedException();
        }

        public void ChangeProductCheckStatus(int index, bool check)
        {
            if (index >= 0 && index < _products.Count)
            {
                _products[index].IsBought = check;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", @"Index of product on buy list.");
            }
        }

        public void UpdateProduct(int index, ListProductItem product)
        {
            if (index >= 0 && index < _products.Count)
            {
                _products[index] = product;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", @"Index of product on buy list.");
            }
        }

        public void UpdateProductCategory(int index, int categoryId, ProductCategoryIds productCategoryId)
        {
            if (index >= 0 && index < _products.Count)
            {
                _products[index].ProductCategoryId = productCategoryId;
                if (productCategoryId == ProductCategoryIds.USER)
                {
                    _products[index].CategoryId = categoryId;
                }
                else
                {
                    _products[index].CategoryId = 0;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", @"Index of product on buy list.");
            }
        }

        public void UpdateProductName(int index, string productName)
        {
            if (index >= 0 && index < _products.Count)
            {
                _products[index].Name = productName;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index", @"Index of product on buy list.");
            }
        }

        private int GetNumberOfBoughtProducts()
        {
            var numberOfBoughtProducts = 0;
            foreach (var product in _products)
            {
                if (product.IsBought)
                {
                    numberOfBoughtProducts++;
                }
            }
            return numberOfBoughtProducts;
        }

        private int GetNumberOfProducts()
        {
            return _products.Count;
        }

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void MoveProduct(int index, int newIndex)
        {
            throw new NotImplementedException();
        }

        public List<ListProductItem> GetProductsByCheckState(bool check)
        {
            return _products.Where(product => product.IsBought == check).ToList();
        }
    }
}
