using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    // Product business object
    // ** Enterprise Design Pattern: Domain Model, Identity Field, Foreign key mapping

    public class Product : BusinessObject
    {
       private double calories;
       public string Name { get; set; }
       public double Weight { get; set; }
       public double Protein { get; set; }
       public double Fats { get; set; }
       public double Carbs { get; set; }
       public double Calories100;
       public Category Category { get; set; }
       public Product() { }
       public Product(Product product)
        {
            this.calories = product.calories;
            this.Name = product.Name;
            this.Weight = product.Weight;
            this.Protein = product.Protein;
            this.Fats = product.Fats;
            this.Carbs = product.Carbs;
            this.Calories100 = product.Calories100;
            this.Category = product.Category;
        }
       public override string ToString()
       {
            return String.Format("{0} {1} {2} {3} {4}",Name,Protein,Fats,Carbs,Calories);
       }
       public double Calories
        {
            get
            {
                if (Weight != 100.0) return (Weight * Calories100) / 100;
                return Calories100;
            }
        }
    }
}
