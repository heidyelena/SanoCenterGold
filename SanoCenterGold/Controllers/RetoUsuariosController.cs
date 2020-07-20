using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanoCenterGold.Data;
using SanoCenterGold.Models;

namespace SanoCenterGold.Controllers
{
    public class RetoUsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public RetoUsuariosController(UserManager<Usuario> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: RetoUsuarios
        //[Authorize(Roles = "usuario, admin, entrenador")]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Usuario usuario = await _userManager.GetUserAsync(User);

            if (usuario != null)
            {
                ViewData["Valoraciones"] = _context.Valoracion.Where(v => v.IdUsuario == usuario.IdUsuario).ToList();

                return View(await _context.RetoUsuario.Include(r => r.Reto).ThenInclude(r => r.Valoraciones)
                    .Where(r => r.EstadoDelReto == Enum.EstadoRetoEnum.Finalizado
                    && r.IdUsuario == usuario.IdUsuario).ToListAsync());
            }
            return View();

        }

        // GET: RetoUsuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retoUsuario = await _context.RetoUsuario
                .Include(r => r.Usuario)
                .Include(r => r.Reto)
                .ThenInclude(r => r.Entrenador)
                .FirstOrDefaultAsync(m => m.IdRetoUsuario == id);

            if (retoUsuario == null)
            {
                return NotFound();
            }

            return View(retoUsuario);
        }

        // GET: RetoUsuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RetoUsuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRetoUsuario,IdReto,IdUsuario,FechaCompletado,EstadoDelReto")] RetoUsuario retoUsuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(retoUsuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(retoUsuario);
        }

        // GET: RetoUsuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retoUsuario = await _context.RetoUsuario.FindAsync(id);
            if (retoUsuario == null)
            {
                return NotFound();
            }
            return View(retoUsuario);
        }

        // POST: RetoUsuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRetoUsuario,IdReto,IdUsuario,FechaCompletado,EstadoDelReto")] RetoUsuario retoUsuario)
        {
            if (id != retoUsuario.IdRetoUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retoUsuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetoUsuarioExists(retoUsuario.IdRetoUsuario))
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
            return View(retoUsuario);
        }

        // GET: RetoUsuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retoUsuario = await _context.RetoUsuario
                .FirstOrDefaultAsync(m => m.IdRetoUsuario == id);
            if (retoUsuario == null)
            {
                return NotFound();
            }

            return View(retoUsuario);
        }

        public async Task<IActionResult> Valoracion(int id)
        {
            var valoracion = new Valoracion();
            valoracion.IdReto = id;
            return View(valoracion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GuardarValoracion([Bind("IdReto,Puntuacion")] Valoracion valoracion)
        {
            Usuario usuario = await _userManager.GetUserAsync(User);
            valoracion.IdUsuario = usuario.IdUsuario;

            _context.Add(valoracion);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: RetoUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var retoUsuario = await _context.RetoUsuario.FindAsync(id);
            _context.RetoUsuario.Remove(retoUsuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetoUsuarioExists(int id)
        {
            return _context.RetoUsuario.Any(e => e.IdRetoUsuario == id);
        }
    }
}
