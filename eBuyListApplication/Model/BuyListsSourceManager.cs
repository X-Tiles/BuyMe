using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Windows.Phone.Speech.VoiceCommands;
using Windows.Storage.Streams;
using eBuyListApplication.Model.Enums;
using eBuyListApplication.Resources;

namespace eBuyListApplication.Model
{
    public class BuyListsSourceManager : IBuyListsSourceManager
    {
        private readonly string _buyListsFilePath;

        public BuyListsSourceManager()
        {
            _buyListsFilePath = AppResources.BuyListsFilePath;
        }

        public int AddNewList(string listName)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var listId = 1;
            var buyListNodes = buyListXml.Elements("BuyLists").Elements("BuyList");
            if (buyListNodes != null)
            {
                listId = GetNewListId(buyListNodes);
            }

            var newListNode = new XElement("BuyList", new XAttribute("Id", listId.ToString()), new XAttribute("Name", listName), new XAttribute("SortMethod", SortMethods.USER.ToString()));
            var productsNode = new XElement("Products");
            newListNode.Add(productsNode);
            buyListXml.Elements("BuyLists").First().Add(newListNode);

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListXml.Save(xmlWriter);

            xmlWriter.Close();
            xml.Close();

            return listId;
        }

        private int GetNewListId(IEnumerable<XElement> listsNodes)
        {
            var newId = 1;
            if (listsNodes == null)
                return newId;

            var occupiedIds = listsNodes.Select(listNode => Convert.ToInt32(listNode.Attribute("Id").Value)).ToList();
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

        public List<EBuyList> GetAllLists()
        {
            var buyLists = new List<EBuyList>();

            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var buyListsNode = buyListXml.Elements("BuyLists");
            var buyListNodes = buyListsNode.Elements("BuyList");
            foreach (var buyListNode in buyListNodes)
            {
                var listName = buyListNode.Attribute("Name").Value;
                var listId = Convert.ToInt32(buyListNode.Attribute("Id").Value);
                var sortMethod = buyListNode.Attribute("SortMethod").Value;

                var buyList = new EBuyList(listId, listName)
                {
                    SortMethods = (SortMethods) Enum.Parse(typeof (SortMethods), sortMethod, true)
                };

                var productsNodes = buyListNode.Elements("Products").First().Elements("Product");
                foreach (var productNode in productsNodes)
                {
                    var productId = (ProductIds) Enum.Parse(typeof (ProductIds), productNode.Attribute("ProductId").Value, true);
                    var productName = ((productNode.Attribute("Name") != null) ? productNode.Attribute("Name").Value : Products.GetProductById(productId).Name);
                    var categoryId = (productNode.Attribute("CategoryId") != null ? Convert.ToInt32(productNode.Attribute("CategoryId").Value) : 0);
                    var productCategoryId = (ProductCategoryIds) Enum.Parse(typeof (ProductCategoryIds), productNode.Attribute("ProductCategoryId").Value, true);
                    var count = Convert.ToInt32(productNode.Attribute("Count").Value);
                    var unit = (UnitIds) Enum.Parse(typeof (UnitIds), productNode.Attribute("Unit").Value, true);
                    var isBought = Convert.ToBoolean(productNode.Attribute("IsBought").Value);

                    var product = new ListProductItem(productId, categoryId, productCategoryId, productName)
                    {
                        Count = count,
                        Unit = unit,
                        IsBought = isBought
                    };
                    buyList.AddNewProduct(product);
                }

                buyLists.Add(buyList);
            }

            return buyLists;
        }

        public EBuyList GetListById(int listId)
        {
            var buyLists = GetAllLists();
            foreach (var buyList in buyLists)
            {
                if (buyList.Id == listId)
                {
                    return buyList;
                }
            }

            return null;
        }

        public void RemoveListById(int listId)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var id = 1;
            var buyListNodes = buyListXml.Elements("BuyLists").Elements("BuyList");
            foreach (var buyListNode in buyListNodes)
            {
                if (id != listId) 
                    continue;

                buyListNode.Remove();
            }

            id = 1;
            foreach (var buyListNode in buyListNodes)
            {
                buyListNode.Attribute("Id").Value = id.ToString();
                id++;
            }

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListXml.Save(xmlWriter);

            xmlWriter.Close();
            xml.Close();
        }

        public void RemoveListsByIds(List<int> listsIds)
        {
            if (listsIds == null)
                return;

            foreach (var listId in listsIds)
            {
                RemoveListById(listId);
            }
        }

        public void AddNewProduct(int listId, ListProductItem productItem)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var buyListNode = GetBuyListNode(listId, buyListXml);
            var productsNode = buyListNode.Element("Products");

            var productNameAttribute = new XAttribute("Name", productItem.Name);
            var productIdAttribute = new XAttribute("ProductId", productItem.ProductId);
            var categoryIdAttribute = new XAttribute("CategoryId", productItem.CategoryId);
            var productCategoryIdAttribute = new XAttribute("ProductCategoryId", productItem.ProductCategoryId);
            var countAttribute = new XAttribute("Count", productItem.Count);
            var unitAttribute = new XAttribute("Unit", productItem.Unit);
            var isBoughtAttribute = new XAttribute("IsBought", productItem.IsBought);

            var productNode = new XElement("Product", productNameAttribute, productIdAttribute,
                productCategoryIdAttribute, categoryIdAttribute, countAttribute, unitAttribute, isBoughtAttribute);
            productsNode.Add(productNode);

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListXml.Save(xmlWriter);

            xmlWriter.Close();
            xml.Close();
        }

        public void RemoveProduct(int listId, int productIndex)
        {
            RemoveProducts(listId, new List<int> { productIndex });
        }

        public void RemoveProducts(int listId, List<int> productIndices)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var buyListNode = GetBuyListNode(listId, buyListXml);
            var productsNodes = buyListNode.Element("Products").Elements("Product");
            var productsNodesArray = productsNodes.ToArray();

            var productsNodesToRemove = new List<XElement>();
            foreach (var productIndex in productIndices)
            {
                var productNode = productsNodesArray[productIndex];
                productsNodesToRemove.Add(productNode);
            }

            foreach (var productNodeToRemove in productsNodesToRemove)
            {
                productNodeToRemove.Remove();
            }

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListXml.Save(xmlWriter);
            
            xmlWriter.Close();
            xml.Close();
        }

        public void ChangeProductState(int listId, int productIndex, bool isBought)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListsXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var buyListNode = GetBuyListNode(listId, buyListsXml);
            var productsNodes = buyListNode.Element("Products").Elements("Product");

            var productNode = productsNodes.ElementAt(productIndex);
            productNode.SetAttributeValue("IsBought", isBought.ToString());

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListsXml.Save(xmlWriter);

            xmlWriter.Close();
            xml.Close();
        }

        public void ChangeProductCategory(int listId, int productIndex, ProductCategoryIds productCategoryId)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListsXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var buyListNode = GetBuyListNode(listId, buyListsXml);
            var productsNodes = buyListNode.Element("Products").Elements("Product");

            var productNode = productsNodes.ElementAt(productIndex);
            productNode.SetAttributeValue("ProductCategoryId", productCategoryId.ToString());

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListsXml.Save(xmlWriter);

            xmlWriter.Close();
            xml.Close();
        }

        public void SortList(int listId, SortMethods sortMethod)
        {
            switch (sortMethod)
            {
                case SortMethods.ALPHABETH_ASCENDING:SortListAlphabetically(listId, true);
                    break;
                case SortMethods.ALPHABETH_DESCENDING: SortListAlphabetically(listId, false);
                    break;
                case SortMethods.CATEGORY_ASCENDING: SortListByCategory(listId, true);
                    break;
                case SortMethods.CATEGORY_DESCENDING: SortListByCategory(listId, false);
                    break;
                default:
                    return;
            }
        }

        public void SortListAlphabetically(int listId, bool ascending)
        {
            var xml2 = File.Open(_buyListsFilePath, FileMode.Open, FileAccess.Read);
            var xmlreader = XmlReader.Create(xml2);
            var buyListsXml = XDocument.Load(xmlreader, LoadOptions.PreserveWhitespace);
            xml2.Close();

            var buyListNode = GetBuyListNode(listId, buyListsXml);
            var productsNode = buyListNode.Element("Products");
            var productsNodes = productsNode.Elements("Product");

            var products = new SortedDictionary<string, XElement>();
            foreach (var productNode in productsNodes)
            {
                var productId = (ProductIds) Enum.Parse(typeof(ProductIds), productNode.Attribute("ProductId").Value);
                products.Add( productId == ProductIds.OTHER ? productNode.Attribute("Name").Value : Products.GetProductById(productId).Name, productNode);
            }

            foreach (var productName in products)
            {
                var newProductNode = new XElement(productName.Value);
                productName.Value.Remove();
                productsNode.SetElementValue("Product", newProductNode);
            }

            var xml = File.Open(_buyListsFilePath, FileMode.Create, FileAccess.Write);

            var xmlWriter = XmlWriter.Create(xml);
            buyListsXml.Save(xmlWriter);

            xmlWriter.Close();
            xml.Close();
        }

        public void SortListByCategory(int listId, bool ascending)
        {
            throw new NotImplementedException();
        }

        public XElement GetBuyListNode(int listId, XDocument buyListsXml)
        {
            var buyListNodes = buyListsXml.Elements("BuyLists").Elements("BuyList");
            foreach (var buyListNode in buyListNodes)
            {
                var buyListId = Convert.ToInt32(buyListNode.Attribute("Id").Value);
                if (buyListId == listId)
                    return buyListNode;
            }

            return null;
        }
    }
}
