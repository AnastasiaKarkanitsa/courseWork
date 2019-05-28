using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class ProductDao : IProductDao
    {
        static Db db = Db.GetInstance();

        public List<Product> GetProductsByCategory(string categoryName)
        {
            return db.Products[categoryName];
        }

        public List<Product> SearchProducts(string productName)
        {
            return new List<Product>();
        }
        
        public Product GetProduct(string productName)
        {
            foreach (string key in db.Products.Keys)
            {
                foreach (Product product in db.Products[key])
                {
                    if (product.Name.Equals(productName)) return product;
                }
            }
            return null;
        }
    }
}
