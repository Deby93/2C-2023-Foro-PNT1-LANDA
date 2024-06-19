using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro.Controllers
{
    public class AccountController : Controller
    {
        private readonly ForoContexto _context;
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
            this._context = context;
            this._userManager = userManager;
            this._signinManager = signinManager;
            this._rolManager = rolManager;
        }

        public ActionResult CrearMiembro()
        {
            return View();
        }

        //[Authorize(Roles = "Miembro,Administrador")]
        [HttpPost]
        public async Task<ActionResult> CrearMiembro(CrearMiembro viewModel)
        {
            //Hago con model lo que necesito.

            if (ModelState.IsValid)
            {
                Miembro miembroACrear = new ()
                {
                    Telefono = viewModel.Telefono,
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    UserName = viewModel.UserName,
                    Email = (viewModel.Email).ToLower(),
                    FechaAlta = DateTime.Now

                };

                var resultadoCreacion = await _userManager.CreateAsync(miembroACrear, viewModel.Password);

                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _userManager.AddToRoleAsync(miembroACrear, Config.MiembroRolName);

                    if (resultado.Succeeded)
                    {
                        //pudo crear - le hago sign-in directamente.
                        await _signinManager.SignInAsync(miembroACrear, isPersistent: false);
                        //TODO - Actualizar los models y vistas para sacar password por consigna


                        return RedirectToAction("Index", "Home");
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
        public ActionResult CrearAdmin()
        {
            return View();
        }

        [Authorize(Roles = Config.AdministradorRolName)]

        [HttpPost]
        public async Task<ActionResult> CrearAdmin(CrearAdmin viewModel)
        {
            if (ModelState.IsValid)
            {
                Usuario administradorACrear =  new()
                {
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    UserName = viewModel.UserName,
                    Email = (viewModel.UserName + Config.Dominio).ToLower(),
                    FechaAlta = DateTime.Now,
                };
                    var resultadoCreacion = await _userManager.CreateAsync(administradorACrear, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    var resultado = await _userManager.AddToRoleAsync(administradorACrear, Config.AdministradorRolName);

                    if (resultado.Succeeded)
                    {
                        await _signinManager.SignInAsync(administradorACrear, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                //tratamiento de errores
                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(viewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> EmailDisponible(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Json(true); // Email disponible
            }
            else
            {
                return Json($"El correo electrónico {email} ya está en uso.");
            }
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> UsuarioDisponible(string userName)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return Json(true); // Nombre de usuario disponible
            }
            else
            {
                return Json($"El nombre de usuario {userName} ya está en uso.");
            }
        }
    
    private async Task CrearRolesBase()
        {
            List<string> roles = new() {"Miembro", "Administrador"};

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
                //var user = await _signinManager.PasswordSignInAsync(ViewModel.Email, ViewModel.Password, ViewModel.RememberMe, false);
                var usuario = _context.Usuarios.FirstOrDefault(p => p.Email == ViewModel.Email || p.UserName == ViewModel.Email);
       
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
