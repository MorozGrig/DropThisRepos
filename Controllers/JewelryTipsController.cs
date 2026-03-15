using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DropThisSite.Data;
using DropThisSite.Models;

namespace DropThisSite.Controllers
{
    public class JewelryTipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JewelryTipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JewelryTips
        public async Task<IActionResult> Index()
        {
            return View(await _context.JewelryTips.ToListAsync());
        }

        // GET: JewelryTips/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelryTip = await _context.JewelryTips
                .FirstOrDefaultAsync(m => m.IdJewelryTip == id);
            if (jewelryTip == null)
            {
                return NotFound();
            }

            return View(jewelryTip);
        }

        // GET: JewelryTips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JewelryTips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJewelryTip,NameJewelryTip")] JewelryTip jewelryTip)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jewelryTip);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jewelryTip);
        }

        // GET: JewelryTips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelryTip = await _context.JewelryTips.FindAsync(id);
            if (jewelryTip == null)
            {
                return NotFound();
            }
            return View(jewelryTip);
        }

        // POST: JewelryTips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdJewelryTip,NameJewelryTip")] JewelryTip jewelryTip)
        {
            if (id != jewelryTip.IdJewelryTip)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jewelryTip);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JewelryTipExists(jewelryTip.IdJewelryTip))
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
            return View(jewelryTip);
        }

        // GET: JewelryTips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jewelryTip = await _context.JewelryTips
                .FirstOrDefaultAsync(m => m.IdJewelryTip == id);
            if (jewelryTip == null)
            {
                return NotFound();
            }

            return View(jewelryTip);
        }

        // POST: JewelryTips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jewelryTip = await _context.JewelryTips.FindAsync(id);
            if (jewelryTip != null)
            {
                _context.JewelryTips.Remove(jewelryTip);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JewelryTipExists(int id)
        {
            return _context.JewelryTips.Any(e => e.IdJewelryTip == id);
        }
    }
}
