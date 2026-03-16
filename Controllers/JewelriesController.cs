using DropThisSite.Data;
using DropThisSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
        public IActionResult Index(
            int page = 1,
            int categoryId = 0,
            int materialId = 0,
            int supplierId = 0,
            int? minPrice = null,
            int? maxPrice = null,
            string? priceRange = null,
            string? search = null,
            string? sort = null)
        {
            const int pageSize = 12;

            IQueryable<Jewelry> query = _context.Jewelries
                .Include(j => j.JewelryTip)
                .Include(j => j.Material)
                .Include(j => j.Stone)
                .AsQueryable();

            if (categoryId > 0)
                query = query.Where(j => j.IdJewelryTip == categoryId);

            if (materialId > 0)
                query = query.Where(j => j.IdMaterial == materialId);

            if (supplierId > 0)
                query = query.Where(j => j.IdSupplier == supplierId);

            if (minPrice.HasValue)
                query = query.Where(j => j.PriceJewelry >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(j => j.PriceJewelry <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var normalizedSearch = search.Trim().ToLower();
                query = query.Where(j => j.NameJewelry != null && j.NameJewelry.ToLower().Contains(normalizedSearch));
            }

            query = priceRange switch
            {
                "0-10000" => query.Where(j => j.PriceJewelry <= 10000),
                "10000-50000" => query.Where(j => j.PriceJewelry >= 10000 && j.PriceJewelry <= 50000),
                "50000+" => query.Where(j => j.PriceJewelry >= 50000),
                _ => query
            };

            query = sort switch
            {
                "price_asc" => query.OrderBy(j => j.PriceJewelry),
                "price_desc" => query.OrderByDescending(j => j.PriceJewelry),
                _ => query.OrderBy(j => j.IdJewelry)
            };

            var totalCount = query.Count();
            var model = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.JewelryTips = _context.JewelryTips.ToList();
            ViewBag.Materials = _context.Materials.ToList();
            ViewBag.Suppliers = _context.Suppliers.ToList();
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.SelectedMaterialId = materialId;
            ViewBag.SelectedSupplierId = supplierId;
            ViewBag.SelectedMinPrice = minPrice;
            ViewBag.SelectedMaxPrice = maxPrice;
            ViewBag.SelectedPriceRange = priceRange ?? string.Empty;
            ViewBag.SelectedSearch = search ?? string.Empty;
            ViewBag.SelectedSort = sort ?? string.Empty;

            return View(model);
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
