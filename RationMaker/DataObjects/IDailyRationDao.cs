using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects
{
    public interface IDailyRationDao
    {
        // gets a daily products
        List<Product> GetDailyProducts();
       
        // gets list of products for one mealtime
        List<Product> GetMealTimeProducts(string mealtime);

        //gets Product of MealTime by name
        Product GetMealTimeProduct(string mealtimeName, string productName);
        
        //gets list of MealTimes
        DailyRation GetDailyRation();

        //add new mealtime
        void Insert(string mealtimeName);

        //add new Product to mealTime
        void Insert(string mealtimeName, Product product);
        
        //delete mealtime by name
        void Delete(string mealtimeName);
        
        //delete product from mealtime
        void Delete(string mealtimeName, string productName);

        void SaveDailyRation(string filename);

        //Clear Daily Ration
        void Clear();
     }
}
