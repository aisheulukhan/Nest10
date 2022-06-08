using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.DAL;
using TaskNest.Models;

namespace TaskNest.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private AppDbContext _context { get;}
        public CategoryController( AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories =await _context.Categories.Include(c=> c.Products).ToListAsync();
            return View(categories);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if ( _context.Categories.FirstOrDefault(c => c.Name.ToLower().Trim() == category.Name.ToLower().Trim()) != null) return RedirectToAction(nameof(Index));
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            category.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            Category category = _context.Categories.Find(id);
            if (category == null) return NotFound();
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            Category existCategory = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (existCategory == null) return NotFound();

            existCategory.Name = category.Name;
            existCategory.Logo = category.Logo;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
