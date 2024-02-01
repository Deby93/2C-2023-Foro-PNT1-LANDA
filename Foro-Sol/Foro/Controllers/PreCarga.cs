using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContexto _contexto;
        private readonly List<string> roles = new List<string>() { "Administrador".ToUpper(), "Miembro".ToUpper(), "Usuario".ToUpper() };
        public PreCarga(UserManager<Usuario> userManager, RoleManager<Rol> roleManager, ForoContexto contexto)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._contexto = contexto;
        }
        public IActionResult Seed()
        {
            try
            {
                _contexto.Database.EnsureDeleted();
                _contexto.Database.Migrate();
                CrearRoles().Wait();
                CrearAdministrador().Wait();
                CrearMiembro().Wait();
                CrearEntrada().Wait();
                AsignoEntrada().Wait();
                CrearUsuario().Wait();
                CrearPregunta().Wait();
                AsignoPregunta().Wait();
                CrearRespuesta().Wait();
                AsignoRespuesta().Wait();

                TempData["Mensaje"] = $"Puede iniciar sesión con {Config.MiembroEmail}{Config.Dominio},  o {Config.AdministradorEmail}{Config.Dominio},  \n siempre todos con la pass {Config.LoginPath}";
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task AsignoRespuesta()
        {
            throw new NotImplementedException();
        }

        private async Task AsignoPregunta()
        {
            throw new NotImplementedException();
        }

        private async Task CrearEntrada()
        {
            throw new NotImplementedException();
        }

        private async Task AsignoEntrada()
        {
            throw new NotImplementedException();
        }

        private async Task CrearPregunta()
        {
            throw new NotImplementedException();
        }

        private async Task CrearRespuesta()
        {
            throw new NotImplementedException();
        }

        private async Task CrearUsuario()
        {
        }

        private async Task CrearMiembro()
        {
            if (!_contexto.Miembros.Any())
            {
                int indice = 1;
                Miembro miembro1 = new Miembro()
                {
                    Email = Config.MiembroEmail + indice.ToString() + Config.Dominio,
                    UserName = Config.MiembroEmail + indice.ToString() + Config.Dominio,
                    Apellido = Config.MiembroRolName.ToUpper(),
                    Nombre = (Config.NombreBase.ToUpper() + indice).ToString(),

                };
                var resultadoCreacion = await _userManager.CreateAsync(miembro1, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    await AgregarARoles(miembro1, Config.RolesParaMiembro);
                }
            }
        }
        private async Task CrearAdministrador()
        {
            var hayAdministrador = _contexto.Usuarios.IgnoreQueryFilters().Any(p => p.NormalizedEmail == Config.AdministradorEmail.ToUpper());
            if (!hayAdministrador)
            {
                Usuario usuario = new Usuario();
                usuario.UserName = Config.AdministradorEmail.ToUpper();
                usuario.Email = Config.AdministradorEmail+ Config.Dominio;
                

                var resultadoCreacion = await _userManager.CreateAsync(usuario, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    await AgregarARoles(usuario, Config.RolesParaAdministrador);
                }
            }
        }

        private async Task AgregarARoles(Usuario usuario, List<string> roles)
        {
            await _userManager.AddToRolesAsync(usuario, roles);
        }

        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName.ToUpper())) { 
                    await _roleManager.CreateAsync(new Rol(rolName.ToUpper())); }
            }

            }
        }
    }
