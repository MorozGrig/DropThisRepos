using DropThisSite.Data;
using DropThisSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DropThisSite.Controllers
{
    public class JewelriesController : Controller
    {
        private const long MaxImageSizeBytes = 5 * 1024 * 1024;
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png"
        };

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public JewelriesController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Jewelries
        public IActionResult Index(
            int page = 1,
            int categoryId = 0,
            int materialId = 0,
            int brandId = 0,
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

            var selectedBrandId = brandId > 0 ? brandId : supplierId;

            if (selectedBrandId > 0)
                query = query.Where(j => j.IdSupplier == selectedBrandId);

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
            ViewBag.SelectedBrandId = selectedBrandId;
            ViewBag.SelectedSupplierId = selectedBrandId;
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
            PopulateSelectLists();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJewelry,NameJewelry,IdJewelryTip,IdMaterial,IdStone,IdSupplier,PriceJewelry")] Jewelry jewelry, IFormFile? imageFile)
        {
            if (!ValidateImage(imageFile))
            {
                PopulateSelectLists(jewelry);
                return View(jewelry);
            }

            if (ModelState.IsValid)
            {
                jewelry.ImagePath = await SaveImageAsync(imageFile);
                _context.Add(jewelry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateSelectLists(jewelry);
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

            PopulateSelectLists(jewelry);
            return View(jewelry);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelry = await _context.Jewelries
                .Include(j => j.JewelryTip)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJewelry,NameJewelry,IdJewelryTip,IdMaterial,IdStone,IdSupplier,PriceJewelry")] Jewelry jewelry, IFormFile? imageFile)
        {
            if (id != jewelry.IdJewelry)
            {
                return NotFound();
            }

            var existingJewelry = await _context.Jewelries.AsNoTracking().FirstOrDefaultAsync(j => j.IdJewelry == id);
            if (existingJewelry == null)
            {
                return NotFound();
            }

            if (!ValidateImage(imageFile))
            {
                jewelry.ImagePath = existingJewelry.ImagePath;
                PopulateSelectLists(jewelry);
                return View(jewelry);
            }

            jewelry.ImagePath = existingJewelry.ImagePath;

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        DeleteImage(existingJewelry.ImagePath);
                        jewelry.ImagePath = await SaveImageAsync(imageFile);
                    }

                    _context.Update(jewelry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JewelryExists(jewelry.IdJewelry))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            PopulateSelectLists(jewelry);
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
                DeleteImage(jewelry.ImagePath);
                _context.Jewelries.Remove(jewelry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JewelryExists(int id)
        {
            return _context.Jewelries.Any(e => e.IdJewelry == id);
        }

        private void PopulateSelectLists(Jewelry? jewelry = null)
        {
            ViewData["IdJewelryTip"] = new SelectList(_context.JewelryTips, "IdJewelryTip", "NameJewelryTip", jewelry?.IdJewelryTip);
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "IdMaterial", "NameMaterial", jewelry?.IdMaterial);
            ViewData["IdStone"] = new SelectList(_context.Stones, "IdStone", "NameStone", jewelry?.IdStone);
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "IdSupplier", "NameSupplier", jewelry?.IdSupplier);
        }

        private bool ValidateImage(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return true;
            }

            var extension = Path.GetExtension(imageFile.FileName);
            if (!AllowedImageExtensions.Contains(extension))
            {
                ModelState.AddModelError("imageFile", ValidationMessages.InvalidImageType);
            }

            if (imageFile.Length > MaxImageSizeBytes)
            {
                ModelState.AddModelError("imageFile", ValidationMessages.ImageTooLarge);
            }

            return ModelState.IsValid;
        }

        private async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await imageFile.CopyToAsync(stream);

            return $"/images/products/{fileName}";
        }

        private void DeleteImage(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return;
            }

            var normalizedPath = imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_environment.WebRootPath, normalizedPath);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }
}
