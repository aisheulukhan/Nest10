using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.DAL;
using TaskNest.Models;
using TaskNest.ViewModels.Products;

namespace TaskNest.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private AppDbContext _context { get; set; }
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Product> prdcts = await _context.Products.Include(p => p.ProductImages).Include(p => p.Category).ToListAsync();
            List<ProductVM> productVms = new List<ProductVM>();
            foreach (var item in prdcts)
            {
                ProductVM product = new ProductVM
                {
                    Id = item.Id,
                    Name = item.Name,
                    Category = item.Category.Name,
                    Price = item.Price,
                    Image = item.ProductImages.FirstOrDefault(pi=>pi.IsFront == true).Image,
                    IsDeleted = item.IsDeleted
                };
                productVms.Add(product);
            }
            return View(productVms);
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.Where(c => c.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories.Where(c=>c.IsDeleted==false).ToList();
                return View();
            }
            if (_context.Products.Any(p => p.Name.Trim().ToLower() == product.Name.Trim().ToLower()))
            {
                ViewBag.Categories = _context.Categories.Where(c=>c.IsDeleted==false).ToList();
                ModelState.AddModelError("Name", "This name already exits");
                return View();
            }
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product == null) return NotFound();
            if (product.IsDeleted == true)
            {
                _context.Products.Remove(product);
            }
            else
            {
                product.IsDeleted = true;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Product product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            Product existProduct = _context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (existProduct == null) return NotFound();

            existProduct.Name = product.Name;
            existProduct.Description = product.Description;
            existProduct.Price = product.Price;
            existProduct.Raiting = product.Raiting;
            //existProduct.CategoryId = product.CategoryId;
            existProduct.StockCount = product.StockCount;

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
