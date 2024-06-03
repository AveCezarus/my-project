using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magazin.Models
{
    public class Receipt
    {
        public DateTime Date { get; set; }
        public double TotalSum { get; set; }
        public List<Product> Products { get; set; }
    }
}
