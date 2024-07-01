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

        //[Authorize(Roles = "Miembro,Administrador")]
        [HttpPost]
        public async Task<ActionResult> CrearMiembro(CrearMiembro viewModel)
        {
            string email = viewModel.Email.ToLower();

            bool emailExists = await _contexto.Usuarios.AnyAsync(u => u.Email == email);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "El correo electrónico ya está en uso.");
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                Miembro miembroACrear = new()
                {
                    Telefono = viewModel.Telefono,
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    UserName = viewModel.UserName,
                    Email =  $"{email}{Config.Dominio}".ToLower(),
                    FechaAlta = DateTime.Now
                };
                miembroACrear.UserName = Config.AdministradorEmail;
                var resultadoCreacion = await _userManager.CreateAsync(miembroACrear, viewModel.Password);

                if (resultadoCreacion.Succeeded)
                {
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

                // Generar un nombre de usuario único
                while (await _userManager.FindByNameAsync(userName) != null)
                {
                    userName = baseUserName + i;
                    i++;
                }

                // Crear el nuevo usuario administrador
                Usuario administradorACrear = new()
                {
                    Nombre = viewModel.Nombre,
                    Apellido = viewModel.Apellido,
                    UserName = viewModel.UserName,
                    Email = $"{email }{ Config.Dominio }".ToLower(),
                    FechaAlta = DateTime.Now,
                };
                administradorACrear.UserName = userName.ToUpper();

                // Guardar el nuevo usuario en la base de datos
                var resultadoCreacion = await _userManager.CreateAsync(administradorACrear, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    // Agregar el nuevo administrador al rol correspondiente
                    var resultado = await _userManager.AddToRoleAsync(administradorACrear, Config.AdministradorRolName);

                    if (resultado.Succeeded)
                    {
                        // Opcionalmente, podrías redirigir a otra página aquí
                        return RedirectToAction("Index", "Home");
                    }
                }

                foreach (var error in resultadoCreacion.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // Si hay errores de validación o el proceso de creación no tuvo éxito, retornar la vista con el modelo
            return View(viewModel);
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> EmailDisponible(string email)
        {
            var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return Json(true); // Email disponible
            }
            else
            {
                return Json($"El correo electrónico {email} ya está en uso.");
            }
        }

        //[AcceptVerbs("Get", "Post")]
        //public async Task<IActionResult> UsuarioDisponible(string userName)
        //{
        //    var user = await _contexto.Usuarios.FirstOrDefaultAsync(u => u.UserName == userName);
        //    if (user == null)
        //    {
        //        return Json(true); // Nombre de usuario disponible
        //    }
        //    else
        //    {
        //        return Json($"El nombre de usuario {userName} ya está en uso.");
        //    }
        //}
    
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
