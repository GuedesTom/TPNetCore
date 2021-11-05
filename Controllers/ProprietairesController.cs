using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TPNetCore.Data;
using TPNetCore.Models;

namespace TPNetCore.Controllers
{
    public class ProprietairesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProprietairesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Proprietaires
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proprietaires.ToListAsync());
        }

        // GET: Proprietaires/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietaire = await _context.Proprietaires.Include(x => x.Voitures)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proprietaire == null)
            {
                return NotFound();
            }

            return View(proprietaire);
        }

        // GET: Proprietaires/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proprietaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nom,Prenom,Adresse")] Proprietaire proprietaire, IFormFile Photo)
        {
            string rootPath = _webHostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(Photo.FileName) + Guid.NewGuid() + Path.GetExtension(Photo.FileName);
            string path = Path.Combine(rootPath + "/Images/", fileName);
            var fileStream = new FileStream(path, FileMode.Create);
            await Photo.CopyToAsync(fileStream);
            fileStream.Close();
            proprietaire.Photo = fileName;
            if (ModelState.IsValid)
            {
                _context.Add(proprietaire);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proprietaire);
        }

        // GET: Proprietaires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietaire = await _context.Proprietaires.FindAsync(id);
            if (proprietaire == null)
            {
                return NotFound();
            }
            return View(proprietaire);
        }

        // POST: Proprietaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nom,Prenom,Adresse,Photo")] Proprietaire proprietaire)
        {
            if (id != proprietaire.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proprietaire);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProprietaireExists(proprietaire.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proprietaire);
        }

        // GET: Proprietaires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proprietaire = await _context.Proprietaires
                .FirstOrDefaultAsync(m => m.Id == id);
            if (proprietaire == null)
            {
                return NotFound();
            }

            return View(proprietaire);
        }

        // POST: Proprietaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proprietaire = await _context.Proprietaires.FindAsync(id);
            _context.Proprietaires.Remove(proprietaire);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProprietaireExists(int id)
        {
            return _context.Proprietaires.Any(e => e.Id == id);
        }
    }
}
