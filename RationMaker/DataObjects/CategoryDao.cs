using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    // Data access object for Category
    // ** DAO Pattern

    public class CategoryDao : ICategoryDao
    {
        static Db db = Db.GetInstance();

        public List<Category> GetCategories()
        {
            return db.Categories;
        }
        public Category GetCategoryByProduct(Product product)
        {
            return product.Category;
        }
        public Category GetCategoryByProduct(string productName)
        {
            ProductDao productDao = new ProductDao();
            return productDao.GetProduct(productName).Category;
        }

    }
}
