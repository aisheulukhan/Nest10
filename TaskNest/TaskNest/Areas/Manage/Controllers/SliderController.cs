using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Nest.Utilies.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaskNest.DAL;
using TaskNest.Models;

namespace TaskNest.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="Admin")]
    public class SliderController : Controller
    {
        
        private AppDbContext _context { get; }

        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Carusels);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Carusel carusel)
        {
            if (carusel.Photo.CheckSize(800))
            {
                ModelState.AddModelError("Photo", "File size must be less than 800kb");
                return View();
            }
            if (!carusel.Photo.CheckType("image/"))
            {
                ModelState.AddModelError("Photo", "File must be image");
                return View();
            }
            carusel.Image = await carusel.Photo.SaveFileAsync(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider"));
            await _context.Carusels.AddAsync(carusel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Carusel carusel = _context.Carusels.Find(id);
            if (carusel == null) return NotFound();
            if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", carusel.Image)))
            {
                System.IO.File.Delete(Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", carusel.Image));
            }
            _context.Carusels.Remove(carusel);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            Carusel carusel = _context.Carusels.FirstOrDefault(c =>c.Id == id);
            if (carusel == null) return NotFound();
            return View(carusel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Carusel carusel)
        {
            if (ModelState.IsValid)
            {
                var s = await _context.Carusels.FindAsync(carusel.Id);
                s.Title = carusel.Title;
                s.Description = carusel.Description;
                if (carusel.Photo != null)
                {
                    if (carusel.Image != null)
                    {
                        string filePath = Path.Combine(_env.WebRootPath, "assets", "imgs", "slider", carusel.Image);
                        System.IO.File.Delete(filePath);
                    }
                    s.Image = ProcessUploadedFile(carusel);
                }
                _context.Update(s);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        private string ProcessUploadedFile(Carusel carusel)
        {
            string uniqueFileName = null;

            if (carusel.Photo != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "assets", "imgs", "slider");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + carusel.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    carusel.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
