using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using eBuyListApplication.Model.Enums;
using eBuyListApplication.Resources;

namespace eBuyListApplication.Model
{
    public class ProductCategories
    {
        private List<ProductCategory> _categories;
        private StandardProductCategories _standardCategories;

        private readonly string _categoriesFilePath;

        private void Initialize()
        {
            _standardCategories = new StandardProductCategories();

            var categoriesXml = XDocument.Load(_categoriesFilePath);

            _categories = new List<ProductCategory>();
            var categoriesNodes = categoriesXml.Elements("ProductCategories").Elements("ProductCategory");
            foreach (var categoryNode in categoriesNodes)
            {
                try
                {
                    var categoryId = Convert.ToInt32(categoryNode.Attribute("Id").Value);
                    var categoryName = categoryNode.Attribute("Name").Value;
                    var categoryIconName = categoryNode.Attribute("Icon").Value;

                    var categoryIconBytes = (byte[])AppResources.ResourceManager.GetObject(categoryIconName);
                    var categoryIcon = Utilities.ByteArraytoBitmapImage(categoryIconBytes);
                    
                    var category = new ProductCategory(categoryId, categoryName, categoryIcon);
                    _categories.Add(category);
                }
                catch (Exception)
                {
                    // TODO Add entry to log file
                }
            }
        }

        public ProductCategories()
        {
            _categoriesFilePath = AppResources.CategoriesFilePath;
            Initialize();
        }

        public List<ProductCategory> GetAllCategories()
        {
            var categories = new List<ProductCategory>();
            categories.AddRange(_standardCategories.GetAllCategories());

            if (_categories == null)
                return categories;

            // NOTE User categories
            categories.AddRange(_standardCategories.GetAllCategories());

            // NOTE User categories
            categories.AddRange(_categories.Select(category => category.Clone()));

            return categories;
        }

        public List<string> GetAllCategoriesNames()
        {
            if (_categories == null)
                return null;

            var categoriesNames = new List<string>();

            // NOTE Standard categories
            categoriesNames.AddRange(_standardCategories.GetAllCategoriesNames());

            // NOTE User categories
            categoriesNames.AddRange(_categories.Select(category => category.Name));

            return categoriesNames;
        }

        public List<BitmapImage> GetAllCategoriesIcons()
        {
            if (_categories == null)
                return null;

            var categoriesIcons = new List<BitmapImage>();

            // NOTE Standard categories
            categoriesIcons.AddRange(_standardCategories.GetAllCategoriesIcons());

            // NOTE User categories
            categoriesIcons.AddRange(_categories.Select(category => category.Icon).ToList());

            return categoriesIcons;
        }

        public ProductCategory GetCategoryByName(string name)
        {
            if (_categories == null)
            {
                return _standardCategories.GetCategoryByName(name);
            }

            foreach (var category in _categories)
            {
                if (category.Name.ToLowerInvariant().Equals(name.ToLowerInvariant()))
                {
                    return category.Clone();
                }
            }

            return _standardCategories.GetCategoryByName(name);
        }

        public ProductCategory GetCategoryById(int id)
        {
            if (id < 1)
                throw new ArgumentOutOfRangeException("id", @"Id of category must be greater than 0.");

            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.Id == id)
                {
                    return category.Clone();
                }
            }

            return null;
        }

        public ProductCategory GetCategoryByCategoryId(ProductCategoryIds categoryId)
        {
            return _standardCategories.GetCategoryByCategoryId(categoryId);
        }

        public string GetCategoryNameByCategoryId(ProductCategoryIds categoryId)
        {
            return _standardCategories.GetCategoryNameByCategoryId(categoryId);
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

        public string GetCategoryNameByCategoryId(int id)
        {
            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.Id == id)
                {
                    return category.Name;
                }
            }

            return null;
        }

        public BitmapImage GetCategoryIconByCategoryId(int id)
        {
            if (_categories == null)
                return null;

            foreach (var category in _categories)
            {
                if (category.Id == id)
                {
                    return category.Icon;
                }
            }

            return null;
        }

        public void AddNewCategory(string categoryName, string categoryIconName)
        {
            var alreadyExist = (GetCategoryByName(categoryName) != null);
            if (alreadyExist)
            {
                throw new ArgumentException("Kategoria o podanej nazwie już istnieje.");
            }

            var userCategoriesXml = XDocument.Load(_categoriesFilePath);
            var categoriesNodes = userCategoriesXml.Elements("ProductCategories").Elements("ProductCategory");

            var categoryId = GetNewCategoryId(categoriesNodes);

            var newCategoryNode = new XElement("ProductCategory");
            newCategoryNode.SetAttributeValue("Id", categoryId);
            newCategoryNode.SetAttributeValue("Name", categoryName);
            newCategoryNode.SetAttributeValue("Icon", categoryIconName);

            var categoriesNode = userCategoriesXml.Elements("ProductCategories");
            categoriesNode.First().SetElementValue("ProductCategory", newCategoryNode);

            var xml = File.Open(_categoriesFilePath, FileMode.Open);
            userCategoriesXml.Save(xml);
            xml.Close();
        }

        private int GetNewCategoryId(IEnumerable<XElement> categoriesNodes)
        {
            int newId = 1;
            if (categoriesNodes == null) 
                return newId;

            var occupiedIds = new List<int>();
            foreach (var categoryNode in categoriesNodes)
            {
                occupiedIds.Add(Convert.ToInt32(categoryNode.Attribute("Id").Value));
            }
            occupiedIds.Sort();

            var minId = 1;
            var maxId = occupiedIds[occupiedIds.Count - 1];
            for (int i = minId; i <= maxId + 1; i++)
            {
                if (occupiedIds.Contains(i))
                {
                    continue;
                }

                newId = i;
                break;
            }
            return newId; 
        }

        public void RemoveCategoryByName(string categoryName)
        {
            var userCategoriesXml = XDocument.Load(_categoriesFilePath);

            var categoriesNodes = userCategoriesXml.Elements("ProductCategories").Elements("ProductCategory");
            foreach (var categoryNode in categoriesNodes)
            {
                if (categoryNode.Attribute("Name").Value == categoryName)
                {
                    categoryNode.Remove();
                }
            }

            var xml = File.Open(_categoriesFilePath, FileMode.Open);
            userCategoriesXml.Save(xml);
            xml.Close();
        }

        public void RemoveAllCategories()
        {
            var userCategoriesXml = XDocument.Load(_categoriesFilePath);

            var categoriesNodes = userCategoriesXml.Elements("ProductCategories").Elements("ProductCategory");
            foreach (var categoryNode in categoriesNodes)
            {
                categoryNode.Remove();
            }

            var xml = File.Open(_categoriesFilePath, FileMode.Open);
            userCategoriesXml.Save(xml);
            xml.Flush();
            xml.Close();
        }
    }
}
