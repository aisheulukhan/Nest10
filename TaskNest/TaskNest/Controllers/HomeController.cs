using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.DAL;
using TaskNest.Models;
using TaskNest.ViewModels;

namespace TaskNest.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context { get; }
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Carusels = await _context.Carusels.ToListAsync(),
                Categories = await _context.Categories.Where(c=>c.IsDeleted==false).ToListAsync(),
                Products = await _context.Products.OrderByDescending(p => p.StockCount).Where(p=>p.StockCount>0).Take(10).Include(p => p.ProductImages).Include(p => p.Category).ToListAsync(),
                RecentProducts = await _context.Products.OrderByDescending(p => p.Id).Take(3).Include(p => p.ProductImages).Include(p => p.Category).ToListAsync(),
                TopRatedProducts = await _context.Products.OrderByDescending(p => p.Raiting).Take(3).Include(p => p.ProductImages).Include(p => p.Category).ToListAsync()
            };
            return View(homeVM);
        }
        public IActionResult Details()
        {
            return View();
        }
    }
}
