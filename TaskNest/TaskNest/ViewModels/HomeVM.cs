using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.Models;

namespace TaskNest.ViewModels
{
    public class HomeVM
    {
        public List<Carusel> Carusels { get; set; }
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<Product> RecentProducts { get; set; }
        public List<Product> TopRatedProducts { get; set; }
    }
}
