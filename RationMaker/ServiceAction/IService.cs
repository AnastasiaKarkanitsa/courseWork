using BusinessObjects;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAction
{
    // single interface to all 'repositories'

    public interface IService
    {
        // Category Repository
        List<Category> GetCategories();
        Category GetCategoryByProduct(string productName);
        Category GetCategoryByProduct(Product product);

        // Product Repository
        Product GetProduct(string productName);
        List<Product> GetProductsByCategory(string categoryName);

        // Ration Repository
        DailyRation GetRation();
        Product GetMealTimeProduct(string mealtimeName, string productName);
        void InsertMealTime(string mealtimeName);
        void InsertProduct(string mealtimeName, Product product);
        void DeleteMealTime(string mealtimeName);
        void DeleteMealTimeProduct(string mealtimeName, string productName);
        void SaveRation(string filename);
        void ClearRation();

        bool Login(string email, string password);
        void Logout();
    }
}
