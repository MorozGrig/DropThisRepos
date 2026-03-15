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
    public class StonesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stones
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stones.ToListAsync());
        }

        // GET: Stones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stone = await _context.Stones
                .FirstOrDefaultAsync(m => m.IdStone == id);
            if (stone == null)
            {
                return NotFound();
            }

            return View(stone);
        }

        // GET: Stones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStone,NameStone,ColorStone,WeightStone")] Stone stone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stone);
        }

        // GET: Stones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stone = await _context.Stones.FindAsync(id);
            if (stone == null)
            {
                return NotFound();
            }
            return View(stone);
        }

        // POST: Stones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdStone,NameStone,ColorStone,WeightStone")] Stone stone)
        {
            if (id != stone.IdStone)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoneExists(stone.IdStone))
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
            return View(stone);
        }

        // GET: Stones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stone = await _context.Stones
                .FirstOrDefaultAsync(m => m.IdStone == id);
            if (stone == null)
            {
                return NotFound();
            }

            return View(stone);
        }

        // POST: Stones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stone = await _context.Stones.FindAsync(id);
            if (stone != null)
            {
                _context.Stones.Remove(stone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoneExists(int id)
        {
            return _context.Stones.Any(e => e.IdStone == id);
        }
    }
}
