using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SanoCenterGold.Data;
using SanoCenterGold.Models;

namespace SanoCenterGold.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Usuario> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Usuario> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            Usuario usuario = await _userManager.GetUserAsync(User);
            Usuario usuarioPorEmail = await _userManager.FindByEmailAsync("email");
            Usuario usuarioPorId = await _userManager.FindByIdAsync("id");

            if (usuario != null)
            {
                ViewData["RetosDeUsuario"] = _context.Reto
                    .Include(r => r.Usuarios)
                    .Include(r => r.Valoraciones)
                    .Include(r => r.Ejercicios)
                    .ThenInclude(r => r.Ejercicio)
                    .Where(r => r.Usuarios.Any(u => u.IdUsuario == usuario.IdUsuario
                        && (u.EstadoDelReto == Enum.EstadoRetoEnum.Apuntado
                        || u.EstadoDelReto == Enum.EstadoRetoEnum.Iniciado)))
                    .Select(r => new Reto
                    {
                        Entrenador = r.Entrenador,
                        FechaLimite = r.FechaLimite,
                        Id = r.Id,
                        IdEntrenador = r.IdEntrenador,
                        Imagen = r.Imagen,
                        Nombre = r.Nombre,
                        Ejercicios = r.Ejercicios,
                        Usuarios = r.Usuarios.Where(u => u.IdUsuario == usuario.IdUsuario).ToList()
                    })
                    .ToList();

                var retoValoracion = new List<KeyValuePair<int, float>>();
                var valoraciones = _context.Valoracion.ToList();

                foreach (var valoracion in valoraciones.GroupBy(v => v.IdReto))
                {
                    float media = 0;
                    foreach (var puntuacion in valoracion)
                    {
                        media += puntuacion.Puntuacion;
                    }
                    media /= valoracion.Count();
                    retoValoracion.Add(new KeyValuePair<int, float>(valoracion.Key, media));
                }

                ViewData["Valoraciones"] = retoValoracion;

                ViewData["RetosGeneral"] = _context.Reto
                    .Include(r => r.Valoraciones)
                    .Include(r => r.Ejercicios)                    
                    .ThenInclude(r => r.Ejercicio)
                    .Where(r => r.Usuarios.All(u => u.IdUsuario != usuario.IdUsuario) 
                    || r.Usuarios.Any(u => u.IdUsuario == usuario.IdUsuario && 
                        (u.EstadoDelReto == Enum.EstadoRetoEnum.Abandonado 
                            || u.EstadoDelReto == Enum.EstadoRetoEnum.SinApuntar)))
                    .ToList();
            }


            return View();
        }

        public async Task<IActionResult> Apuntarse(int idReto)
        {
            Usuario usuario = await _userManager.GetUserAsync(User);
            var retoBuscado = _context.Reto.Where(x => x.Id == idReto).Include(x => x.Usuarios).FirstOrDefault();
            var retoApuntado = retoBuscado.Usuarios.Where(u => u.IdUsuario == usuario.IdUsuario).FirstOrDefault();

            if (retoApuntado == null)
            {
                RetoUsuario reto = new RetoUsuario();
                reto.IdReto = idReto;
                reto.IdUsuario = usuario.IdUsuario;
                reto.Reto = _context.Reto.Where(r => r.Id == idReto).FirstOrDefault();
                reto.Usuario = usuario;
                reto.EstadoDelReto = Enum.EstadoRetoEnum.Apuntado;
                _context.Add(reto);
            }
            else
            {
                retoApuntado.EstadoDelReto = Enum.EstadoRetoEnum.Apuntado;
                _context.Update(retoApuntado);
            }




            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Abandonar(int idReto)
        {
            Usuario usuario = await _userManager.GetUserAsync(User);

            var reto = _context.Reto.Where(x => x.Id == idReto).Include(x => x.Usuarios).FirstOrDefault();
            var retoAbandonado = reto.Usuarios.Where(u => u.IdUsuario == usuario.IdUsuario).FirstOrDefault();
            retoAbandonado.EstadoDelReto = Enum.EstadoRetoEnum.Abandonado;

            _context.Update(retoAbandonado);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IniciarReto(int idReto)
        {
            Usuario usuario = await _userManager.GetUserAsync(User);
            var retoBuscado = _context.Reto.Where(x => x.Id == idReto).Include(x => x.Usuarios).FirstOrDefault();
            var retoApuntado = retoBuscado.Usuarios.Where(u => u.IdUsuario == usuario.IdUsuario).FirstOrDefault();


            retoApuntado.EstadoDelReto = Enum.EstadoRetoEnum.Iniciado;
            _context.Update(retoApuntado);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> FinalizarReto(int idReto)
        {
            Usuario usuario = await _userManager.GetUserAsync(User);
            var retoBuscado = _context.Reto.Where(x => x.Id == idReto).Include(x => x.Usuarios).FirstOrDefault();
            var retoApuntado = retoBuscado.Usuarios.Where(u => u.IdUsuario == usuario.IdUsuario).FirstOrDefault();

            retoApuntado.FechaCompletado = DateTime.Now;
            retoApuntado.EstadoDelReto = Enum.EstadoRetoEnum.Finalizado;
            usuario.RetosCompletados = usuario.RetosCompletados + 1;

            _context.Update(usuario);
            _context.Update(retoApuntado);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Ranking()
        {
            var usuarios = await _userManager.Users.ToListAsync();

            ViewData["Gimnasios"] = _context.Gimnasio.ToList(); 

            return View(usuarios); ;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
