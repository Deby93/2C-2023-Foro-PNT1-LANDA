using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public ActionResult Registrar()
        {
            return View();
        }

        //[Authorize(Roles = "Miembro,Administrador")]
        [HttpPost]
        public async Task<ActionResult> Registrar(Registrar viewModel)
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
                    Email = viewModel.Email,
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
