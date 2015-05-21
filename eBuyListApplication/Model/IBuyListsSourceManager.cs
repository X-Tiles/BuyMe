using System.Collections.Generic;
using eBuyListApplication.Model.Enums;

namespace eBuyListApplication.Model
{
    public interface IBuyListsSourceManager
    {
        List<EBuyList> GetAllLists();
        EBuyList GetListById(int listId);
        int AddNewList(string listName);
        void RemoveListById(int listId);
        void RemoveListsByIds(List<int> listsIds);
        void AddNewProduct(int listId, ListProductItem productItem);
        void RemoveProduct(int listId, int productIndex);
        void RemoveProducts(int listId, List<int> productIndices);
        void ChangeProductState(int listId, int productIndex, bool isBought);
        void ChangeProductCategory(int listId, int productIndex, ProductCategoryIds productCategoryId);
        void SortList(int listId, SortMethods sortMethod);
    }
}
