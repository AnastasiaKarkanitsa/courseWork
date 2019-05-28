using BusinessObjects;
using System.IO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataObjects
{
    public class Db
    {
       public static string connectionString { get; set; }
       static Db instance; //экзепляр класса DB когда пользователь захочет создать обект типа дб,

       public Dictionary<string, List<Product>> Products { get; set; }
       public List<Category> Categories { get; set; }
       public DailyRation Ration { get; set; } 

       protected Db(string connectionString )
        {
            if (Db.connectionString == null)
                connectionString = "products.xml";
            else
                Db.connectionString = connectionString;
            Products = new Dictionary<string, List<Product>>();
            Categories = new List<Category>();
            Ration = new DailyRation();
            Read(connectionString);
        }

       private void Read(string connectionString)
        {

            XDocument xdoc = XDocument.Load(connectionString);
            foreach (XElement category in xdoc.Element("Db").Elements("Category"))
            {
                Category categ = new Category()
                {
                    CategoryName = category.Attribute("name").Value,
                    Description = category.Attribute("description").Value
                };

                Categories.Add(categ);
                List<Product> categoryProducts = new List<Product>();
                foreach (XElement product in category.Elements("Product"))
                {
                    Product p = new Product();
                    p.Name = product.Element("Name").Value;
                    p.Weight = Convert.ToDouble(product.Element("Gramms").Value);
                    p.Protein = Convert.ToDouble(product.Element("Protein").Value);
                    p.Fats = Convert.ToDouble(product.Element("Fats").Value);
                    p.Carbs = Convert.ToDouble(product.Element("Carbs").Value);
                    p.Calories100= Convert.ToDouble(product.Element("Calories").Value);

                    p.Category = categ;
                    categoryProducts.Add(p);
                }
                Products[categ.CategoryName] = categoryProducts;
               }
        }

       public static Db GetInstance()
        {
            if (instance == null) return new Db(connectionString);
            return instance;
        }

        // insert a new MealTime
        public void Insert(string mealtimeName)
        {
            if (!Ration.MealTimes.ContainsKey(mealtimeName))
            {
                Ration.MealTimes[mealtimeName] = new MealTime(mealtimeName);
                Ration.N++;
            }
        }

        // delete MealTime by name
        public void Delete(string mealtimeName)
        {
            Ration.MealTimes.Remove(mealtimeName);
            Ration.N--;
        }

        // delete product from mealtime
        public void Delete(string mealtimeName, string productName)
        {
            foreach (Product p in Ration.MealTimes[mealtimeName].Meal)
            {
                if (p.Name == productName)
                {
                    Ration.MealTimes[mealtimeName].Meal.Remove(p);
                    return;
                }
            }
           
        }

        // insert Product to MealTime
        public void Insert(string mealtimeName, Product product)
        {
            Ration.MealTimes[mealtimeName].Meal.Add(new Product(product));
        }

        public void ClearDailyRation()
        {
            Ration = new DailyRation();
        }
        public void SaveDailyRation(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(Ration);
            }
        }
               
    }
}
