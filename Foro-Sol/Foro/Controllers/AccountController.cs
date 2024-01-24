using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foro.Controllers
{
    //[Authorize] autorizacion minima sesion iniciada
    public class AccountController : Controller
    {
        private readonly ForoContexto _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;
        private readonly RoleManager<Rol> _rolManager;
        private const string passDefault = "Password1!";

        public AccountController(
            ForoContexto context,
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signinManager,
            RoleManager<Rol> rolManager
            )
        {
            this._context = context;
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._rolManager = rolManager;
        }

        public ActionResult Registrar()
        {
            return View();
        }

        //[Authorize(Roles = "Miembro,Administrador")]
        [HttpPost]
        public async Task<ActionResult> Registrar(RegistroUsuario viewModel)
        {
            //Hago con model lo que necesito.

            if (ModelState.IsValid)
            {
                var miembroACrear = new Miembro
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email
                };

                var resultadoCreacion = await _userManager.CreateAsync(miembroACrear, viewModel.Password);

                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _userManager.AddToRoleAsync(miembroACrear, "Miembro");

                    if (resultado.Succeeded)
                    {
                        //pudo crear - le hago sign-in directamente.
                        await _signinManager.SignInAsync(miembroACrear, isPersistent: false);
                        //TODO - Actualizar los models y vistas para sacar password por consigna


                        return RedirectToAction("Edit", "Miembros", new { id = miembroACrear.Id });
                    }

                }

                //no pudo
                //tratamiento de errores
                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EmailDisponible(string email)
        {
            var usuarioExistente = await _userManager.FindByEmailAsync(email);

            if (usuarioExistente == null)
            {
                //No hay un Persona existente con ese email
                return Json(true);
            }
            else
            {
                //El mail ya está en uso
                return Json($"El correo {email} ya está en uso.");
            }
            //Utilizo JSON, Jquery Validate method, espera una respuesta de este tipo.
            //Para que esto funcione desde luego, tienen que estar como siempre las librerias de Jquery disponibles.
            //Importante, que estén en el siguiente ORDEN!!!!!
            //jquery.js
            //jquery.validate.js
            //jquery.validate.unobtrisive.js

            //Jquery está en el Layout, y luego las otras dos, están definidas en el archivo _ValidationScriptsPartial.cshtml. 
            //Si incluyen el render de la sección de script esa, estará entonces disponible.
        }
        private async Task CrearRolesBase()
        {
            List<string> roles = new List<string>() { "Miembro", "Administrador" };

            foreach (string rol in roles)
            {
                await CrearRole(rol);
            }
        }

        private async Task CrearRole(string rolName)
        {
            if (!await _rolManager.RoleExistsAsync(rolName))
            {
                await _rolManager.CreateAsync(new Rol(rolName));
            }
        }

        //Empleado por defecto
        public async Task<IActionResult> CrearMiembroGod()
        {
            var miembroGod = await _userManager.FindByNameAsync("God");

            if (miembroGod != null)
            {
                await _signinManager.SignOutAsync();
                await _userManager.DeleteAsync(miembroGod);
            }

            Miembro account = new Miembro()
            {
                Nombre = "Sr",
                Apellido = "God",
                Email = "miembrogod@ort.edu.ar",
                UserName = "God"
            };


            var resuAdm = await _userManager.CreateAsync(account, passDefault);

            if (resuAdm.Succeeded)
            {
                await CrearRolesBase();

                
                var resultadoRolGod = await _userManager.AddToRoleAsync(account, "Administrador");

                if (resultadoRolGod.Succeeded)
                {
                    await _signinManager.SignInAsync(account, isPersistent: true);
                    TempData["Mensaje"] = $"Miembro creado {account.Email} y {passDefault}";

                }

            }

            return RedirectToAction("Index", "Miembros");
        }
        public ActionResult IniciarSesion(string returnurl)
        {
            TempData["returnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IniciarSesion(Login ViewModel)
        {
            string returnUrl = TempData["returnUrl"] as string;
            if (ModelState.IsValid)
            {
                var user = await _signinManager.PasswordSignInAsync(ViewModel.Email, ViewModel.Password, ViewModel.RememberMe, false);

                if (user.Succeeded)
                {
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                        return Redirect(returnUrl);

                    if (User.IsInRole("Miembro")) { return RedirectToAction("CheckIn", "Miembros"); }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Inicio de sesión inválido.");
            }
            return View(ViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccesoDenegado(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
    }
}
