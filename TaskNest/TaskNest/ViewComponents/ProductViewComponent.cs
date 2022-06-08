using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.DAL;
using TaskNest.Models;
using TaskNest.ViewModels;

namespace TaskNest.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private AppDbContext _context { get; }
        public ProductViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            int pagecount = (int)Math.Ceiling((double)_context.Products.Count() / 10);

            if (page < 0 || page > pagecount)
            {
                page = 1;
            }
            
            List<Product> products = await _context.Products.Where(p => p.IsDeleted == false)
                               .Skip((page - 1) * 10)
                               .Take(10)
                               .Include(p => p.ProductImages)
                               .Include(p => p.Category).ToListAsync();
            PaginateVM<Product> pagination = new PaginateVM<Product>
            {
                Items = products,
                ActivePage = page,
                PageCount = pagecount
            };

            return View(await Task.FromResult(pagination));
    }
    
}
}
