using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanoCenterGold.Data;
using SanoCenterGold.Models;

namespace SanoCenterGold.Controllers
{
    public class RetoEjerciciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetoEjerciciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RetoEjercicios
        public async Task<IActionResult> Index()
        {
            return View(await _context.RetoEjercicio.ToListAsync());
        }

        // GET: RetoEjercicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retoEjercicio = await _context.RetoEjercicio
                .FirstOrDefaultAsync(m => m.IdRetoEjercicio == id);
            if (retoEjercicio == null)
            {
                return NotFound();
            }

            return View(retoEjercicio);
        }

        // GET: RetoEjercicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RetoEjercicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRetoEjercicio,IdReto,IdEjercicio")] RetoEjercicio retoEjercicio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(retoEjercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(retoEjercicio);
        }

        // GET: RetoEjercicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retoEjercicio = await _context.RetoEjercicio.FindAsync(id);
            if (retoEjercicio == null)
            {
                return NotFound();
            }
            return View(retoEjercicio);
        }

        // POST: RetoEjercicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRetoEjercicio,IdReto,IdEjercicio")] RetoEjercicio retoEjercicio)
        {
            if (id != retoEjercicio.IdRetoEjercicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retoEjercicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetoEjercicioExists(retoEjercicio.IdRetoEjercicio))
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
            return View(retoEjercicio);
        }

        // GET: RetoEjercicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retoEjercicio = await _context.RetoEjercicio
                .FirstOrDefaultAsync(m => m.IdRetoEjercicio == id);
            if (retoEjercicio == null)
            {
                return NotFound();
            }

            return View(retoEjercicio);
        }

        // POST: RetoEjercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var retoEjercicio = await _context.RetoEjercicio.FindAsync(id);
            _context.RetoEjercicio.Remove(retoEjercicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetoEjercicioExists(int id)
        {
            return _context.RetoEjercicio.Any(e => e.IdRetoEjercicio == id);
        }
    }
}
