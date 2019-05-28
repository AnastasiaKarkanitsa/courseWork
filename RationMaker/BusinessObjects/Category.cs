using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Category : BusinessObject
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public Category() { }
        public override string ToString()
        {
            return String.Format("{0} {1} ", CategoryName, Description);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}
