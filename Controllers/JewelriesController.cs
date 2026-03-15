using DropThisSite.Data;
using DropThisSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DropThisSite.Controllers
{
    public class JewelriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JewelriesController(ApplicationDbContext context)
        {
            _context = context;
        }

            // GET: Jewelries
            // ✅ КАТАЛОГ ДЛЯ ПОКУПАТЕЛЕЙ
            public IActionResult Index(int? page = 1)
        {
            int pageSize = 12;
            var jewelry = _context.Jewelries
                .Include(j => j.JewelryTip)
                .Include(j => j.Material)
                .Include(j => j.Stone)
                .Include(j => j.Supplier)
                .OrderBy(j => j.NameJewelry)
                .Skip((page.Value - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(_context.Jewelries.Count() / (double)pageSize);
            ViewBag.CurrentPage = page;
            return View(jewelry);
        }


        // GET: Jewelries/Create
        public IActionResult Create()
        {
            ViewData["IdJewelryTip"] = new SelectList(_context.JewelryTips, "IdJewelryTip", "NameJewelryTip");
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "IdMaterial", "NameMaterial");
            ViewData["IdStone"] = new SelectList(_context.Stones, "IdStone", "NameStone");
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "IdSupplier", "NameSupplier");
            return View();
        }

        // POST: Jewelries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJewelry,NameJewelry,IdJewelryTip,IdMaterial,IdStone,IdSupplier,PriceJewelry")] Jewelry jewelry)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jewelry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "IdMaterial", "NameMaterial", jewelry.IdMaterial);
            ViewData["IdStone"] = new SelectList(_context.Stones, "IdStone", "ColorStone", jewelry.IdStone);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "IdSupplier", "NameSupplier", jewelry.IdSupplier);
            return View(jewelry);
        }

        // GET: Jewelries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelry = await _context.Jewelries.FindAsync(id);
            if (jewelry == null)
            {
                return NotFound();
            }
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "IdMaterial", "NameMaterial", jewelry.IdMaterial);
            ViewData["IdStone"] = new SelectList(_context.Stones, "IdStone", "ColorStone", jewelry.IdStone);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "IdSupplier", "NameSupplier", jewelry.IdSupplier);
            return View(jewelry);
        }

        // POST: Jewelries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJewelry,NameJewelry,IdJewelryTip,IdMaterial,IdStone,IdSupplier,PriceJewelry")] Jewelry jewelry)
        {
            if (id != jewelry.IdJewelry)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jewelry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JewelryExists(jewelry.IdJewelry))
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
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "IdMaterial", "NameMaterial", jewelry.IdMaterial);
            ViewData["IdStone"] = new SelectList(_context.Stones, "IdStone", "ColorStone", jewelry.IdStone);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "IdSupplier", "NameSupplier", jewelry.IdSupplier);
            return View(jewelry);
        }

        // GET: Jewelries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelry = await _context.Jewelries
                .Include(j => j.Material)
                .Include(j => j.Stone)
                .Include(j => j.Supplier)
                .FirstOrDefaultAsync(m => m.IdJewelry == id);
            if (jewelry == null)
            {
                return NotFound();
            }

            return View(jewelry);
        }

        // POST: Jewelries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jewelry = await _context.Jewelries.FindAsync(id);
            if (jewelry != null)
            {
                _context.Jewelries.Remove(jewelry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JewelryExists(int id)
        {
            return _context.Jewelries.Any(e => e.IdJewelry == id);
        }
    }
}
