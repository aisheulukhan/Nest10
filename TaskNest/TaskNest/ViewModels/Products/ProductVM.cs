using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskNest.ViewModels.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public bool IsDeleted { get; set; }

    }
}
