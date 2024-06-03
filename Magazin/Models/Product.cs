using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Models
{
    
    public class Product
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
  
}
