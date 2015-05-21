using System;
using System.Collections.Generic;
using System.Linq;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public class EBuyListsManager
    {
        public static int MaxNumberOfLists = 5;
        public static int MaxNumberOfItemsLists = 50;

        private List<EBuyList> _eBuyLists;
        private readonly IBuyListsSourceManager _xmlManager;

        public EBuyListsManager()
        {
            _xmlManager = new BuyListsSourceManager();
            Initialize();
        }

        private void Initialize()
        {
            _eBuyLists = _xmlManager.GetAllLists();
        }

        public int AddNewList(string listName)
        {
            if (_eBuyLists.Count == MaxNumberOfLists)
            {
                throw new IndexOutOfRangeException(string.Format("The maximum number of lists that user can define is {0}", MaxNumberOfLists));
            }

            var listId = _xmlManager.AddNewList(listName);
            
            var newBuyList = new EBuyList(listId, listName);
            _eBuyLists.Add(newBuyList);

            return listId;
        }

        public void RemoveListByIndex(int listId)
        {
            var listsIds = GetIdsOfLists();
            if (!listsIds.Contains(listId))
                return;

            _xmlManager.RemoveListById(listId);

            var index = _eBuyLists.TakeWhile(eBuyList => eBuyList.Id != listId).Count();
            _eBuyLists.RemoveAt(index);

            var id = 1;
            foreach (var eBuyList in _eBuyLists)
            {
                eBuyList.Id = id++;
            }
        }

        private List<int> GetIdsOfLists()
        {
            var ids = new List<int>();
            foreach (var buyList in _eBuyLists)
            {
                ids.Add(buyList.Id);
            }
            return ids;
        }

        public void RemoveListsByIndexes(List<int> listsIds)
        {
            if (listsIds == null)
                return;

            foreach (var listId in listsIds)
            {
                RemoveListByIndex(listId);
            }
        }

        public List<EBuyList> GetAllLists()
        {
            return _eBuyLists;
        }

        public EBuyList GetListByIndex(int listId)
        {
            return _eBuyLists.FirstOrDefault(buyList => buyList.Id == listId);
        }

        public void AddNewProductToList(int listId, Product product)
        {
            var productItem = new ListProductItem(product);
            AddNewProductToList(listId, productItem);
        }

        public void AddNewProductToList(int listId, string productName)
        {
            var product = Products.GetProductByName(productName);
            var productItem = (product != null ? new ListProductItem(product) : new ListProductItem(ProductIds.OTHER, 0, ProductCategoryIds.OTHER, productName));

            AddNewProductToList(listId, productItem);
        }

        private void AddNewProductToList(int listId, ListProductItem productItem)
        {
            _xmlManager.AddNewProduct(listId, productItem);

            var buyList = GetListByIndex(listId);
            buyList.AddNewProduct(productItem);
        }

        public void RemoveProduct(int listId, int productIndex)
        {
            RemoveProducts(listId, new List<int> {productIndex});
        }

        public void RemoveProduct(int listId, ListProductItem productItem)
        {
            var buyList = GetListByIndex(listId);
            var productIndex = GetProductIndex(productItem, buyList);

            RemoveProducts(listId, new List<int> { productIndex });
        }

        private static int GetProductIndex(ListProductItem productItem, EBuyList buyList)
        {
            var productIndex = 0;
            foreach (var product in buyList.Products)
            {
                if (product == productItem)
                    break;

                productIndex++;
            }
            return productIndex;
        }

        public void RemoveProducts(int listId, List<int> productIndexes)
        {
            _xmlManager.RemoveProducts(listId, productIndexes);

            var buyList = GetListByIndex(listId);
            buyList.RemoveProductsByIndexes(productIndexes);
        }

        public void ChangeProductState(int listId, ListProductItem productItem, bool isBought)
        {
            var buyList = GetListByIndex(listId);
            var productIndex = GetProductIndex(productItem, buyList);

            _xmlManager.ChangeProductState(listId, productIndex, isBought);

            buyList.Products[productIndex].IsBought = isBought;
        }

        public void ChangeProductCategory(int listId, ListProductItem productItem, ProductCategoryIds productCategoryId)
        {
            var buyList = GetListByIndex(listId);
            var productIndex = GetProductIndex(productItem, buyList);

            _xmlManager.ChangeProductCategory(listId, productIndex, productCategoryId);

            buyList.Products[productIndex].ProductCategoryId = productCategoryId;
        }

        public void SortListAlphabetically(int listId, bool ascending)
        {
            _xmlManager.SortList(listId, SortMethods.ALPHABETH_ASCENDING);
        }
    }
}
