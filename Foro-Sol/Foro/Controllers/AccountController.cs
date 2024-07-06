using Foro.Helpers;
using Foro.Models;
using Foro.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro.Controllers
{
    public class AccountController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;
        private readonly RoleManager<Rol> _rolManager;

        public AccountController(
            ForoContexto context,
            UserManager<Usuario> userManager,
            SignInManager<Usuario> signinManager,
            RoleManager<Rol> rolManager
            )
        {
            this._contexto = context;
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._rolManager = rolManager;
        }

        public ActionResult CrearMiembro()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CrearMiembro(CrearMiembro viewModel)
        {
            if (ModelState.IsValid)
            {
                Miembro miembroACrear = new();
                miembroACrear.Nombre = viewModel.Nombre;
                miembroACrear.Telefono = viewModel.Telefono;
                miembroACrear.Apellido = viewModel.Apellido;
                miembroACrear.UserName = viewModel.UserName;
                miembroACrear.Email = viewModel.Email;
                miembroACrear.FechaAlta = DateTime.Now;


                var resultadoCreacion = await _userManager.CreateAsync(miembroACrear, viewModel.Password);

                if (resultadoCreacion.Succeeded)
                {


                    if (!_rolManager.Roles.Any())
                    {
                        await CrearRolesBase();
                    }

                    var resultado = await _userManager.AddToRoleAsync(miembroACrear, Config.MiembroRolName);

                    if (resultado.Succeeded)
                    {
                        await _signinManager.SignInAsync(miembroACrear, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                }

                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(viewModel);
        }


        [HttpGet]
        public ActionResult CrearAdmin()
        {
            return View();
        }

        [Authorize(Roles = Config.AdministradorRolName)]

        [HttpPost]
        public async Task<ActionResult> CrearAdmin(CrearAdmin viewModel)
        {
            string email = viewModel.Nombre.ToLower();

            bool emailExists = await _contexto.Usuarios.AnyAsync(u => u.Email == email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "El correo electrónico ya está en uso.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                int i = 3;
                string baseUserName = "Administrador";
                string userName = baseUserName;

                while (await _userManager.FindByNameAsync(userName) != null)
                {
                    userName = baseUserName + i;
                    i++;
                }

                Usuario administradorACrear = new()
                {
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    UserName = viewModel.UserName,
                    Email = $"{userName}{Config.Dominio}".ToLower(),
                    FechaAlta = DateTime.Now,
                };
                administradorACrear.UserName = userName.ToUpper();

                var resultadoCreacion = await _userManager.CreateAsync(administradorACrear, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _userManager.AddToRoleAsync(administradorACrear, Config.AdministradorRolName);

                    if (resultado.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

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
            if (_signinManager.IsSignedIn(User) && User.IsInRole("Usuario"))
            {
                var emailUsado = await _userManager.FindByIdAsync(email);
                if (email.Length > 10 && (email.Substring(email.Length - 10) == "@admin.com"))
                {
                    if (emailUsado == null)
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json($"El correo {email} ya está en uso!");
                    }
                }
                else
                {
                    return Json($"El correo {email} es invalido, Ingrese en el siguiente formato: Example@admin.com");
                }
            }
            else
            {
                var emailUsado = _contexto.Usuarios.Any(p => p.Email == email);

                if (email.Length > 10 && !(email.Substring(email.Length - 10) == "@admin.com") && !emailUsado)
                {
                    return Json(true);
                }
                else
                {
                    return Json($"El correo {email} ya está en uso o no corresponde al tipo de mail valido ");
                }
            }

        }

        [HttpGet]
        public IActionResult UsuarioDisponible(string username)
        {
            var usuarioUsado = _contexto.Usuarios.FirstOrDefault(p => p.UserName == username);
            if (usuarioUsado == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"El Usuario {username} ya está en uso.");
            }
        }

        private async Task CrearRolesBase()
        {
            List<string> roles = new() { "Miembro", "Administrador" };

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
                var usuario = _contexto.Usuarios.FirstOrDefault(p => p.Email == ViewModel.Email || p.UserName == ViewModel.Email);

                if (usuario == null)
                {
                    ModelState.AddModelError(string.Empty, "Inicio de sesión inválido");
                }
                else
                {
                    var resultadoSignIn = await _signinManager.PasswordSignInAsync(usuario, ViewModel.Password, ViewModel.RememberMe, false);
                    if (resultadoSignIn.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Inicio de sesión inválido");
                    }
                }
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