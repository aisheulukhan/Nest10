using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskNest.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required,MaxLength(150)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        public float Raiting { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public int StockCount { get; set; }
        public bool IsDeleted { get; set; }

        public Category Category { get; set; }
        public List<ProductImage> ProductImages { get; set; }

    }
}
