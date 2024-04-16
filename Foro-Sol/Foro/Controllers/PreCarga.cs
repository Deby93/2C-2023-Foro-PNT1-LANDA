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
            _userManager = userManager;
            _roleManager = roleManager;
            _contexto = contexto;
        }
        public IActionResult Seed()
        {
            try
            {
                _contexto.Database.EnsureDeleted();
                _contexto.Database.Migrate();
                CrearRoles().Wait();
                CrearAdministrador().Wait();
                CrearMiembro();
                //CrearEntrada();
                //// AsignoEntrada().Wait();
                CrearCategoria();
                //// AsignoCategoria();
                //CrearPregunta();
                //AsignoPregunta().Wait();
                //CrearRespuesta().Wait();
                ////AsignoRespuesta().Wait();
                CrearReaccion();
                ////AsignoReaccion();

                TempData["Mensaje"] = $"Puede iniciar sesión con {Config.MiembroEmail}{Config.Dominio},  o {Config.AdministradorEmail}{Config.Dominio},  \n siempre todos con la pass {Config.LoginPath}";
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

            return RedirectToAction("Index", "Home");
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

        private async Task CrearAdministrador()
        {
            var hayAdministrador = _contexto.Usuarios.IgnoreQueryFilters().Any(p => p.NormalizedEmail == Config.AdministradorEmail.ToUpper());

            if (!hayAdministrador)
            {

                Usuario usuario = new()
                {
                    UserName = Config.AdministradorEmail.ToUpper(),
                    Email = Config.AdministradorEmail+ Config.Dominio,
                    Apellido= Config.AdministradorRolName.ToUpper(),
                    Nombre = Config.AdministradorRolName.ToUpper(),
                    FechaAlta= DateTime.Now,
                };

                var resultadoCreacion = await _userManager.CreateAsync(usuario, Config.GenericPass);

                if (resultadoCreacion.Succeeded)
                {
                    await AgregarARoles(usuario, Config.RolesParaAdministrador);

                }
            }
        }
 

        private Miembro CrearMiembro()
        {
            Miembro miembro = new();
            if (!_contexto.Miembros.Any())
            {
                int indice = _contexto.Miembros.Count() + 1;
                Miembro miembro1 = new()
                {
                    Email = Config.MiembroEmail + indice.ToString() + Config.Dominio,
                    UserName = Config.MiembroEmail + indice.ToString() + Config.Dominio,
                    Apellido = Config.MiembroRolName.ToUpper(),
                    Nombre = (Config.NombreBaseMiembro.ToUpper() + indice).ToString(),
                                        FechaAlta= DateTime.Now,


                };
                var resultadoCreacion = _userManager.CreateAsync(miembro1, Config.GenericPass);

                if (resultadoCreacion != null)
                {
                    miembro1 = miembro;
                    AgregarARoles(miembro, Config.RolesParaMiembro);
                    miembro1.Id = indice + 1;
                }
            }
            return miembro;
        }

        //private Entrada CrearEntrada()
        //{
        //    Entrada entrada = new();
        //    if (!_contexto.Entradas.Any())
        //    {
        //        Entrada entrada1 = new();
        //        {
        //            bool esPrivada = new Random().Next(2) == 0;

        //            entrada1.Privada = esPrivada;
        //            entrada1.Fecha = DateTime.Now;
        //            entrada1.Categoria = CrearCategoria();
        //            entrada1.MiembrosHabilitados = new();

        //        }
        //        entrada = entrada1;
        //    }
        //    else
        //    {
        //        entrada = _contexto.Entradas.FirstOrDefault();

        //        // entrada = DameEntrada();
        //    }
        //    _ = _contexto.Add(entrada);
        //    _contexto.SaveChanges();
        //    return entrada;
        //}

        //private async Task AsignoEntrada()
        //{
        //    throw new NotImplementedException();
        //}

        private Categoria CrearCategoria()
        {

            Categoria categoria = new();
            if (!_contexto.Categorias.Any())
            {
                Categoria categoria1 = new();
                {
                    categoria1.Entradas = new();
                    categoria1.Nombre = Config.NombresParaCategorias.FirstOrDefault();

                };

                categoria = categoria1;
            }
            else
            {
                //categoria = DameCategoria();
                categoria = _contexto.Categorias.FirstOrDefault();
            }
            _contexto.Add(categoria);
            _contexto.SaveChanges();

            return categoria;
        }

        //private async Task AsignoCategoria()
        //{
        //    throw new NotImplementedException();
        //}


        //private Pregunta CrearPregunta()
        //{
        //    Pregunta pregunta = new();


        //    if (!_contexto.Preguntas.Any())
        //    {
        //        Pregunta pregunta1 = new();
        //        {
        //            pregunta1.Descripcion = Config.DescripcionesParaPreg.FirstOrDefault();
        //            // pregunta1.Respuestas= CrearRespuestas();
        //            //pregunta1.Entrada = CrearEntrada();
        //            pregunta1.Fecha = DateTime.Now;

        //        };
        //        pregunta = pregunta1;
        //        _contexto.Add(pregunta);
        //        _contexto.SaveChanges();

        //    }
        //    return pregunta;
        //}

        //private async Task AsignoPregunta()
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task CrearRespuesta()
        //{
        //    if (!_contexto.Respuestas.Any())
        //    {
        //        Respuesta respuesta1 = new();
        //        {
        //           /// respuesta1.Pregunta = CrearPregunta();
        //            respuesta1.Descripcion = Config.DescripcionesParaRta.FirstOrDefault();
        //            // respuesta1.Reacciones=
        //            respuesta1.Miembro = CrearMiembro();
        //        };

        //        await _contexto.AddAsync(respuesta1);
        //        await _contexto.SaveChangesAsync();
        //    }
        //}

        //private async Task AsignoRespuesta()
        //{
        //    Respuesta respuesta = DameRespuesta();

        //    if (respuesta != null)
        //    {
        //        //  _contexto.Miembros.AddAsync(respuesta);
        //    }


        //}

        private Reaccion CrearReaccion()
        {
            Reaccion reaccion = new();
            if (!_contexto.Reacciones.Any())
            {
                Reaccion reaccion1 = new();
                {
                    bool eslikeada = new Random().Next(2) == 0; // 0 representa false, 1 representa true

                    reaccion1.MeGusta = eslikeada;
                    // reaccion1.Respuesta = DameRespuesta();
                    reaccion1.Fecha = DateTime.Now;
                    reaccion1.Miembro = CrearMiembro();

                };

                reaccion = reaccion1;
            }
            else
            {
                reaccion = _contexto.Reacciones.FirstOrDefault();
                //reaccion = DameReaccion();
            }
            _contexto.Add(reaccion);
            _contexto.SaveChangesAsync();

            return reaccion;
        }

        //private async Task AsignoReaccion()
        //{
        //    throw new NotImplementedException();
        //}

        //private Entrada DameEntrada()
        //{
        //    Entrada entradaDeseada = new();
        //    foreach (Entrada entrada in _contexto.Entradas)
        //    {
        //        entradaDeseada = entrada;
        //    }
        //    return entradaDeseada;
        //}
        ////private Categoria DameCategoria()
        ////{
        ////    Categoria categoriaDeseada = new();
        ////    foreach (Categoria categoria in _contexto.Categorias)
        ////    {
        ////        categoriaDeseada = categoria;
        ////    }
        ////    return categoriaDeseada;
        ////}

        //private Respuesta DameRespuesta()
        //{
        //    Respuesta respuestaDeseada = new();
        //    foreach (Respuesta respuesta in _contexto.Respuestas)
        //    {
        //        respuestaDeseada = respuesta;
        //    }
        //    return respuestaDeseada;
        //}

        //private Reaccion DameReaccion()
        //{
        //    Reaccion reaccionDeseado = new();
        //    foreach (Reaccion reaccion in _contexto.Reacciones)
        //    {
        //        reaccionDeseado = reaccion;
        //    }
        //    return reaccionDeseado;
        //}


    }

}