using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using eBuyListApplication.Resources;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class Products
    {
        private static List<Product> _products;

        private static void Initialize()
        {
            var productsXml = XDocument.Parse(AppResources.Products);

            _products = new List<Product>();
            var productsNodes = productsXml.Element("Products").Elements("Product");
            foreach (var productNode in productsNodes)
            {
                try
                {
                    var productId = (ProductIds) Enum.Parse(typeof (ProductIds), productNode.Attribute("ProductId").Value, true);
                    var productName = productNode.Attribute("Name").Value;
                    var productCategoryId = (ProductCategoryIds) Enum.Parse(typeof (ProductCategoryIds), productNode.Attribute("ProductCategoryId").Value, true);

                    var product = new Product(productId, productName, productCategoryId);
                    _products.Add(product);
                }
                catch (Exception)
                {
                    // TODO Add entry to log file
                }
            }
        }

        public static List<Product> GetAllProducts()
        {
            if (_products == null)
            {
                Initialize();
            }

            var products = new List<Product>();

            if (_products == null)
                return products;

            foreach (var product in _products)
            {
                products.Add(product.Clone());
            }

            return products;
        }

        public static Product GetProductByName(string name)
        {
            if (_products == null)
            {
                Initialize();
            }

            return (from product in _products where product.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()) select product.Clone()).FirstOrDefault();
        }

        public static Product GetProductById(ProductIds productId)
        {
            if (_products == null)
            {
                Initialize();
            }

            foreach (var product in _products)
            {
                if (product.ProductId == productId)
                {
                    return product.Clone();
                }
            }

            return null;
        }

        public static List<Product> GetProductsByNamePattern(string pattern)
        {
            var products = new List<Product>();

            if (_products == null)
            {
                Initialize();
            }

            foreach (var product in _products)
            {
                if (product.Name.ToLowerInvariant().StartsWith(pattern.ToLowerInvariant()))
                {
                    products.Add(product.Clone());
                }
            }

            return products;
        }

        public static List<Product> GetProductsByCategory(ProductCategoryIds productCategoryId)
        {
            var products = new List<Product>();

            if (_products == null)
            {
                Initialize();
            }

            foreach (var product in _products)
            {
                if (product.ProductCategoryId == productCategoryId)
                {
                    products.Add(product.Clone());
                }
            }

            return products;
        }
    }
}
