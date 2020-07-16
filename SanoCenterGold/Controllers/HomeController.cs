using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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

            //ViewData["RetosDeUsuario"] = _context.Reto
            //    .Include(r => r.Ejercicios)
            //    .ThenInclude(r => r.Ejercicio)
            //    .ToList();


            // 1. CSS de la pagina principal, bonito y cuqui para cada tarea
            // 2. Encontrar la forma de acceder al campo UsuarioId en los dos
            // ViewData que hay aquí abajo
            // 3. Almacenar la IdUsuario correctamente *
            // 4. Crear una pagina para el administrador donde poder cambiar los roles de los usuarios



            ViewData["RetosDeUsuario"] = _context.Reto
                .Include(r => r.Usuarios)
                .Include(r => r.Ejercicios)
                .ThenInclude(r => r.Ejercicio)
                .Where(r => r.Usuarios.Any(u => u.IdUsuario == 1))
                .ToList();

            ViewData["RetosGeneral"] = _context.Reto
                .Include(r => r.Ejercicios)
                .ThenInclude(r => r.Ejercicio)
                .Where(r => r.Usuarios.All(u => u.IdUsuario != 1))
                .ToList();


            return View();
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
