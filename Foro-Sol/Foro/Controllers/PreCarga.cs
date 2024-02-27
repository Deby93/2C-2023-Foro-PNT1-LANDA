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
        private readonly List<string> roles = new() { "Administrador".ToUpper(), "Miembro".ToUpper(), "Usuario".ToUpper() };
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
                //AsignoRespuesta().Wait();
                CrearReaccion().Wait();

                TempData["Mensaje"] = $"Puede iniciar sesión con {Config.MiembroEmail}{Config.Dominio},  o {Config.AdministradorEmail}{Config.Dominio},  \n siempre todos con la pass {Config.LoginPath}";
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task CrearReaccion()
        {
            if (!_contexto.Reacciones.Any())
            {
                Reaccion reaccion1 = new();
                {
                    reaccion1.MeGusta = false;
                    reaccion1.Respuesta = DameRespuesta();
                    reaccion1.Fecha = DateTime.Now;
                    reaccion1.Miembro = DameMiembro();

                };

                await _contexto.AddAsync(reaccion1);
                await _contexto.SaveChangesAsync();
            }
        }

        private Miembro DameMiembro()
        {
            Miembro miembroDeseado = new();
            foreach (Miembro miembro in _contexto.Miembros)
            {
                miembroDeseado = miembro;
            }
            return miembroDeseado;
        }

        private Respuesta DameRespuesta()
        {
            Respuesta respuestaDeseada = new();
            foreach (Respuesta respuesta in _contexto.Respuestas)
            {
                respuestaDeseada =respuesta;
            }
            return respuestaDeseada;
        }
        private Categoria DameCategoria()
        {
            Categoria categoriaDeseada = new();
            foreach (Categoria categoria in _contexto.Categorias)
            {
                categoriaDeseada = categoria;
            }
            return categoriaDeseada;
        }

        
        //private async Task AsignoRespuesta()
        //{
        //    Respuesta respuesta = new();

        //   // Miembros.add(respuesta);

        //}

        private async Task AsignoPregunta()
        {
            throw new NotImplementedException();
        }

        private async Task CrearEntrada()
        {
            if (!_contexto.Entradas.Any())
            {
                Entrada entrada1 = new();
                {
                    entrada1.Privada = false;
                    entrada1.Fecha = DateTime.Now;
                    entrada1.Categoria =DameCategoria();
                    //entrada1.CantidadDePreguntasYRespuestas = 0;

                }

                await _contexto.AddAsync(entrada1);
                await _contexto.SaveChangesAsync();
            }
        }

        private async Task AsignoEntrada()
        {
            throw new NotImplementedException();
        }

        private async Task CrearPregunta()
        {
            if (!_contexto.Preguntas.Any())
            {
                int indice = _contexto.Preguntas.Count() + 1;
                Pregunta pregunta1 = new();
                {
                    pregunta1.MiembroId = indice + 1;
                    pregunta1.Descripcion = "Cual es el color de Pantone del 2024?";
                    CrearRespuesta();
                    //pregunta1.Entrada = "Tendencia";
                    pregunta1.Fecha = DateTime.Now;
                    
                };

                await _contexto.AddAsync(pregunta1);
                await _contexto.SaveChangesAsync();
            }
        }

        private async Task CrearRespuesta()
        {
            if (!_contexto.Respuestas.Any())
            {
                Respuesta respuesta1 = new();
                {
                    ;
                };

                await _contexto.AddAsync(respuesta1);
                await _contexto.SaveChangesAsync();
            }
        }

        private async Task CrearUsuario()
        {
            if (!_contexto.Usuarios.Any())
            {
                int indice = _contexto.Usuarios.Count() + 1;
                Usuario usuario1 = new()
                {
                    Email = Config.UsuarioEmail + indice.ToString() + Config.Dominio,
                    UserName = Config.UsuarioEmail + indice.ToString() + Config.Dominio,
                    Apellido = Config.UsuarioRolName.ToUpper(),
                    Nombre = (Config.UsuarioRolName.ToUpper() + indice).ToString(),

                };
                var resultadoCreacion = await _userManager.CreateAsync(usuario1, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    await AgregarARoles(usuario1, Config.RolesParaUsuario);
                    usuario1.Id = indice + 1;
                }

            }
        }

        private async Task CrearMiembro()
        {
            if (!_contexto.Miembros.Any())
            {
                int indice = _contexto.Miembros.Count() + 1;
                Miembro miembro1 = new()
                {
                    Email = Config.MiembroEmail + indice.ToString() + Config.Dominio,
                    UserName = Config.MiembroEmail + indice.ToString() + Config.Dominio,
                    Apellido = Config.MiembroRolName.ToUpper(),
                    Nombre = (Config.NombreBaseMiembro.ToUpper() + indice).ToString(),

                };
                var resultadoCreacion = await _userManager.CreateAsync(miembro1, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    await AgregarARoles(miembro1, Config.RolesParaMiembro);
                    miembro1.Id = indice + 1;
                }
            }
        }
        private async Task CrearAdministrador()
        {
            var hayAdministrador = _contexto.Usuarios.IgnoreQueryFilters().Any(p => p.NormalizedEmail == Config.AdministradorEmail.ToUpper());

            if (!hayAdministrador)
            {
               
                Usuario usuario = new()
                {
                    UserName = Config.AdministradorEmail.ToUpper(),
                    Email = Config.AdministradorEmail + Config.Dominio
                };


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
                if (!await _roleManager.RoleExistsAsync(rolName.ToUpper()))
                {
                    await _roleManager.CreateAsync(new Rol(rolName.ToUpper()));
                }
            }

        }
    }
}