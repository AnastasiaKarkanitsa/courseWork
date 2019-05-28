using BusinessObjects;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAction
{
    // implementation of IService interface. 

    // ** Facade pattern.
    // ** Repository pattern (Service could be split up in individual Repositories: Product, Category, etc).

    public class Service : IService
    {
        static readonly ICategoryDao categoryDao = new CategoryDao();
        static readonly IProductDao productDao = new ProductDao();
        static readonly IDailyRationDao rationDao = new DailyRationDao();
        
        // Category Services
        public List<Category> GetCategories()
        {
            return categoryDao.GetCategories();
        }
        public Category GetCategoryByProduct(string productName)
        {
            return categoryDao.GetCategoryByProduct(productName);
        }
        public Category GetCategoryByProduct(Product product)
        {
            return categoryDao.GetCategoryByProduct(product);
        }

        // Product Services
        public Product GetProduct(string productName)
        {
           return productDao.GetProduct(productName);
        }
        public List<Product> GetProductsByCategory(string categoryName)
        {
            return productDao.GetProductsByCategory(categoryName);
        }
        public List<Product> SearchProducts(string productName)
        {
            return productDao.SearchProducts(productName);
        }

        // Ration Services
        public DailyRation GetRation()
        {
            return rationDao.GetDailyRation();
        }

        public Product GetMealTimeProduct(string mealtimeName, string productName)
        {
            return rationDao.GetMealTimeProduct(mealtimeName, productName);
        }

        public void InsertMealTime(string mealtimeName)
        {
            rationDao.Insert(mealtimeName);
        }

        public void InsertProduct(string mealtimeName, Product product)
        {
            rationDao.Insert(mealtimeName, product);
        }

        public void DeleteMealTime(string mealtimeName)
        {
            rationDao.Delete(mealtimeName);
        }

        public void DeleteMealTimeProduct(string mealtimeName, string productName)
        {
            rationDao.Delete(mealtimeName, productName);

        }

        public void SaveRation(string filename)
        {
            rationDao.SaveDailyRation(filename); 
        }

        public void ClearRation()
        {
            rationDao.Clear();
        }

        // Authentication and Authorization Services
        public bool Login(string email, string password)
        {
           // return Security.Login(email, password);
            return true;
        }
        public void Logout()
        {
            //Security.Logout();
        }
    }
}
