using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace DataObjects
{
    public class DailyRationDao : IDailyRationDao
    {
        static Db db = Db.GetInstance();

        // gets a daily products
        public List<Product> GetDailyProducts()
        {
            List<Product> dailyProducts = new List<Product>();
            foreach (string s in db.Ration.MealTimes.Keys)
            {
                dailyProducts.AddRange(db.Ration.MealTimes[s].Meal);
            }
            return dailyProducts;
        }

        // gets list of products for one mealtime
        public List<Product> GetMealTimeProducts(string mealtime)
        {
            return db.Ration.MealTimes[mealtime].Meal;
        }

        //gets Product of MealTime by name
        public Product GetMealTimeProduct(string mealtimeName, string productName)
        {
            if (db.Ration.MealTimes.ContainsKey(mealtimeName))
            {
                foreach (Product p in db.Ration.MealTimes[mealtimeName].Meal)
                {
                    if (p.Name == productName) return p;
                }
            }
            return null;
        }

        //gets list of MealTimes
        public DailyRation GetDailyRation()
        {
            return db.Ration;
        }

        public void Insert(string mealtimeName)
        {
            db.Insert(mealtimeName);
        }

        public void Insert(string mealtimeName, Product product)
        {
            db.Insert(mealtimeName,product);
        }

        public void Delete(string mealtimeName)
        {
            db.Delete(mealtimeName);
        }

        public void Delete(string mealtimeName, string productName)
        {
            db.Delete(mealtimeName, productName);
        }

        public void Clear()
        {
            db.ClearDailyRation();
        }

        public void SaveDailyRation(string filename)
        {
            db.SaveDailyRation(filename);
        }


    }
}
