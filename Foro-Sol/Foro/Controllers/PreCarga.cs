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
        private readonly List<string> roles = new() { "Administrador".ToUpper(), "Miembro".ToUpper() };
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
                //CrearCategoria();
                ////// AsignoCategoria();
                ////CrearPregunta();
                ////AsignoPregunta().Wait();
                ////CrearRespuesta().Wait();
                //////AsignoRespuesta().Wait();
                //CrearReaccion();
                //////AsignoReaccion();

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
                    Email = Config.AdministradorEmail + Config.Dominio,
                    Apellido = Config.AdministradorRolName.ToUpper(),
                    Nombre = Config.AdministradorRolName.ToUpper(),
                    FechaAlta = DateTime.Now,
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
                    FechaAlta = DateTime.Now,
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

        private void CrearCategorias()
        {
            List<Categoria> categorias = new List<Categoria>
            {
                new Categoria() { Nombre = "Futbol" },
                new Categoria() { Nombre = "Tecnologia" },
                new Categoria() { Nombre = "Ciencia" },
                new Categoria() { Nombre = "Musica" },
                new Categoria() { Nombre = "Cine" },
                new Categoria() { Nombre = "Literatura" }
            };

            _contexto.Categorias.AddRange(categorias);
            _contexto.SaveChanges();
        }

        private void CrearEntradas()
        {
            var categorias = _contexto.Categorias.ToList();
            List<Entrada> entradas = new List<Entrada>
            {
                new Entrada() { Titulo = "Últimos avances en IA", Descripcion = "Contenido sobre IA", CategoriaId = categorias.First(c => c.Nombre == "Tecnologia").CategoriaId },
                new Entrada() { Titulo = "Resumen del partido", Descripcion = "Contenido del partido de futbol", CategoriaId = categorias.First(c => c.Nombre == "Futbol").CategoriaId },
                new Entrada() { Titulo = "Descubrimientos recientes", Descripcion = "Contenido sobre ciencia", CategoriaId = categorias.First(c => c.Nombre == "Ciencia").CategoriaId },
                new Entrada() { Titulo = "Nuevos álbumes de música", Descripcion = "Contenido sobre música", CategoriaId = categorias.First(c => c.Nombre == "Musica").CategoriaId },
                new Entrada() { Titulo = "Estrenos de cine", Descripcion = "Contenido sobre cine", CategoriaId = categorias.First(c => c.Nombre == "Cine").CategoriaId},
                new Entrada() { Titulo = "Novedades literarias", Descripcion = "Contenido sobre literatura", CategoriaId = categorias.First(c => c.Nombre == "Literatura").CategoriaId }
            };

            _contexto.Entradas.AddRange(entradas);
            _contexto.SaveChanges();
        }

        private void CrearPreguntas()
        {
            var entradas = _contexto.Entradas.ToList();
            List<Pregunta> preguntas = new List<Pregunta>
            {
                new Pregunta() { Descripcion = "¿Qué opinas sobre la IA?", EntradaId = entradas.First(e => e.Titulo == "Últimos avances en IA").Id },
                new Pregunta() { Descripcion = "¿Quién fue el mejor jugador?", EntradaId = entradas.First(e => e.Titulo == "Resumen del partido").Id },
                new Pregunta() { Descripcion = "¿Cuál es el impacto de este descubrimiento?", EntradaId = entradas.First(e => e.Titulo == "Descubrimientos recientes").Id },
                new Pregunta() { Descripcion = "¿Cuál es tu álbum favorito?", EntradaId = entradas.First(e => e.Titulo == "Nuevos álbumes de música").Id },
                new Pregunta() { Descripcion = "¿Cuál película esperas más?", EntradaId = entradas.First(e => e.Titulo == "Estrenos de cine").Id },
                new Pregunta() { Descripcion = "¿Qué libro recomendarías?", EntradaId = entradas.First(e => e.Titulo == "Novedades literarias").Id }
            };

            _contexto.Preguntas.AddRange(preguntas);
            _contexto.SaveChanges();
        }

        private void CrearRespuestas()
        {
            var preguntas = _contexto.Preguntas.ToList();
            List<Respuesta> respuestas = new List<Respuesta>
            {
                new Respuesta() { Descripcion = "La IA es el futuro.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué opinas sobre la IA?").PreguntaId },
                new Respuesta() { Descripcion = "El mejor jugador fue Messi.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Quién fue el mejor jugador?").PreguntaId },
                new Respuesta() { Descripcion = "El impacto es significativo.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es el impacto de este descubrimiento?").PreguntaId },
                new Respuesta() { Descripcion = "Me encanta el nuevo álbum de Taylor Swift.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es tu álbum favorito?").PreguntaId },
                new Respuesta() { Descripcion = "Espero la nueva película de Marvel.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál película esperas más?").PreguntaId },
                new Respuesta() { Descripcion = "Recomiendo 'Cien Años de Soledad'.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué libro recomendarías?").PreguntaId }
            };

            _contexto.Respuestas.AddRange(respuestas);
            _contexto.SaveChanges();
        }

        private void CrearReacciones()
        {
            if (!_contexto.Reacciones.Any())
            {
                var respuestas = _contexto.Respuestas.ToList();
                List<Reaccion> reacciones = new List<Reaccion>
                {
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "La IA es el futuro.").RespuestaId },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "El mejor jugador fue Messi.").RespuestaId },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "El impacto es significativo.").RespuestaId },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de The Weeknd.").RespuestaId },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Recomiendo 'Cien Años de Soledad'.").RespuestaId }
                };

                _contexto.Reacciones.AddRange(reacciones);
                _contexto.SaveChanges();
               // _logger.LogInformation("Reacciones creadas correctamente.");
            }
            else
            {
               // _logger.LogInformation("Las reacciones ya existen y no se crearon nuevamente.");
            }
        }
    }



}


 