using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using eBuyListApplication.Resources;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class StandardProductCategories
    {
        private List<ProductCategory> _categories;

        private void Initialize()
        {
            var categoriesXml = XDocument.Parse(AppResources.ProductCategories);

            _categories = new List<ProductCategory>();
            var categoriesNodes = categoriesXml.Elements("ProductCategories").Elements("ProductCategory");
            foreach (var categoryNode in categoriesNodes)
            {
                try
                {
                    var productCategoryId = (ProductCategoryIds)Enum.Parse(typeof(ProductCategoryIds), categoryNode.Attribute("ProductCategoryId").Value, true);
                    var categoryName = categoryNode.Attribute("Name").Value;
                    var categoryIconName = categoryNode.Attribute("Icon").Value;

                    var categoryIconBytes = (byte[])AppResources.ResourceManager.GetObject(categoryIconName);
                    var categoryIcon = Utilities.ByteArraytoBitmapImage(categoryIconBytes);

                    var category = new ProductCategory(productCategoryId, categoryName, categoryIcon);
                    _categories.Add(category);
                }
                catch (Exception)
                {
                    // TODO Add entry to log file
                }
            }
        }

        public StandardProductCategories()
        {
            Initialize();
        }

        public List<ProductCategory> GetAllCategories()
        {
            var categories = new List<ProductCategory>();

            if (_categories == null)
                return categories;

            foreach (var category in _categories)
            {
                categories.Add(category.Clone());
            }

            return categories;
        }

        public ProductCategory GetCategoryByName(string name)
        {
            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    return category.Clone();
                }
            }

            return null;
        }

        //public ProductCategory GetCategoryById(int id)
        //{
        //    if (_categories == null)
        //        return null;

        //    foreach (var category in _categories)
        //    {
        //        if (category.Id == id)
        //        {
        //            return category.Clone();
        //        }
        //    }

        //    return null;
        //}

        public ProductCategory GetCategoryByCategoryId(ProductCategoryIds categoryId)
        {
            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.CategoryId == categoryId)
                {
                    return category.Clone();
                }
            }

            return null;
        }

        public string GetCategoryNameByCategoryId(ProductCategoryIds categoryId)
        {
            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.CategoryId == categoryId)
                {
                    return category.Name;
                }
            }

            return null;
        }

        public BitmapImage GetCategoryIconByCategoryId(ProductCategoryIds categoryId)
        {
            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.CategoryId == categoryId)
                {
                    return category.Icon;
                }
            }

            return null;
        }

        public List<string> GetAllCategoriesNames()
        {
            if (_categories == null)
                return null;

            var categoriesNames = new List<string>();
            foreach (var category in _categories)
            {
                categoriesNames.Add(category.Name);
            }

            return categoriesNames;
        }

        public List<BitmapImage> GetAllCategoriesIcons()
        {
            if (_categories == null)
                return null;

            var categoriesIcons = new List<BitmapImage>();
            foreach (var category in _categories)
            {
                categoriesIcons.Add(category.Icon);
            }

            return categoriesIcons;
        }
    }
}
