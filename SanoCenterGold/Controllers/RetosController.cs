using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SanoCenterGold.Data;
using SanoCenterGold.Models;

namespace SanoCenterGold.Controllers
{
    public class RetosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public RetosController(ApplicationDbContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Retos
        public async Task<IActionResult> Index()
        {
            var retos = new List<Reto>();
            if (User.IsInRole("admin"))
            {
                retos = await _context.Reto
                    .Include(r => r.Entrenador)
                    .ToListAsync();
            } 
            else if (User.IsInRole("entrenador"))
            {
                var entrenador = await _userManager.GetUserAsync(User);
                retos = await _context.Reto
                .Where(r => r.IdEntrenador == entrenador.IdUsuario)
                .Include(r => r.Entrenador).ToListAsync();
            }
            
            return View(retos);
        }

        // GET: Retos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reto = await _context.Reto
                .Include(r => r.Ejercicios)
                    .ThenInclude(r => r.Ejercicio)
                .Include(r => r.Entrenador)
                .Include(r => r.Usuarios)
                    .ThenInclude(r => r.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewData["Valoraciones"] = _context.Valoracion.ToList();

            if (reto == null)
            {
                return NotFound();
            }
            
            return View(reto);


        }

        // GET: Retos/Create
        public IActionResult Create()
        {
            ViewData["Ejercicios"] = new MultiSelectList(_context.Ejercicio, "Id", "Nombre");
            return View();
        }

        // POST: Retos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Imagen,FechaLimite,IdEntrenador")] Reto reto, string[] ejercicios)
        {
            var currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                List<RetoEjercicio> ejerciciosLista = new List<RetoEjercicio>();

                for (int i = 0; i < ejercicios.Length; i++)
                {
                    ejerciciosLista.Add(new RetoEjercicio()
                    {
                        Ejercicio = await _context.Ejercicio.FindAsync(Convert.ToInt32(ejercicios[i])),
                        Reto = reto
                    });
                }

                reto.Ejercicios = ejerciciosLista;
                reto.Entrenador = currentUser;
                reto.IdEntrenador = currentUser.IdUsuario;

                _context.Add(reto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reto);
        }

        // GET: Retos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reto = await _context.Reto.FindAsync(id);
            if (reto == null)
            {
                return NotFound();
            }
            return View(reto);
        }

        // POST: Retos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Imagen,FechaLimite,IdEntrenador")] Reto reto)
        {
            if (id != reto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetoExists(reto.Id))
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
            return View(reto);
        }

        // GET: Retos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reto = await _context.Reto
                .Include(r => r.Entrenador)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reto == null)
            {
                return NotFound();
            }

            return View(reto);
        }

        // POST: Retos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reto = await _context.Reto
                .FindAsync(id);

            var usuariosReto = _context.RetoUsuario.Where(r => r.IdReto == reto.Id);
            var ejerciciosReto = _context.RetoEjercicio.Include(r => r.Reto).Where(r => r.Reto.Id == reto.Id);

            // borramos la relacion entre los usuarios y este reto
            _context.RetoUsuario.RemoveRange(usuariosReto);

            // borramos la relacion entre los ejercicios y este reto
            _context.RetoEjercicio.RemoveRange(ejerciciosReto);
            _context.SaveChanges();

            _context.Reto.Remove(reto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RetoExists(int id)
        {
            return _context.Reto.Any(e => e.Id == id);
        }
    }
}
