﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using SanoCenterGold.Data;
using SanoCenterGold.Models;

namespace SanoCenterGold.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nombre")]
            public string Nombre { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Apellidos")]
            public string Apellidos { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Fecha de Nacimiento")]
            public DateTime FechaNacimiento { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Dni")]
            public string Dni { get; set; }

            [Required]
            [DataType(DataType.PhoneNumber)]
            [Display(Name = "Teléfono")]
            public int Telefono { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Dirección")]
            public string Direccion { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Localidad")]
            public string Localidad { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Eilge tu Centro Sano")]
            public int Gimnasio { get; set; }

            [Required]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public bool Entrenador { get; set; }

            [DataType(DataType.EmailAddress)]
            public int IdUsuario { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            ViewData["MaxId"] = _context.Users
                .OrderByDescending(x => x.IdUsuario)
                .FirstOrDefault().IdUsuario + 1;

            ViewData["Gimnasios"] = new SelectList(_context.Gimnasio, "Id", "Nombre");

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Usuario 
                {
                    Nombre = Input.Nombre,
                    Apellidos = Input.Apellidos,
                    FechaNacimiento = Input.FechaNacimiento,
                    Dni = Input.Dni,
                    Telefono = Input.Telefono,
                    Direccion = Input.Direccion,
                    Localidad = Input.Localidad,
                    UserName = Input.Email, 
                    Email = Input.Email,
                    IdUsuario = Input.IdUsuario,
                    IdGimnasio = Input.Gimnasio
                   
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if (Input.Entrenador)
                    {
                       await _userManager.AddToRoleAsync(user, "Entrenador");
                    } 
                    else
                    {
                        await _userManager.AddToRoleAsync(user, "Usuario");
                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
