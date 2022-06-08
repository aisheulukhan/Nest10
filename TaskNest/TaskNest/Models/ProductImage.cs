using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskNest.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool IsFront { get; set; }
        public bool IsBack { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
