using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foro.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContexto _contexto;
        private readonly List<string> roles = new() { "ADMINISTRADOR", "MIEMBRO" };

        public PreCarga(UserManager<Usuario> userManager, RoleManager<Rol> roleManager, ForoContexto contexto)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contexto = contexto;
        }

        public async Task<IActionResult> Seed()
        {
            try
            {
                await _contexto.Database.EnsureDeletedAsync();
                await _contexto.Database.MigrateAsync();

                await CrearRoles();
                await CrearAdministrador();
                var miembros = await CrearMiembros(); // Capturar los miembros creados
                CrearCategorias();
                CrearEntradas(miembros); // Pasar los miembros creados
                CrearPreguntas(miembros); // Pasar los miembros creados
                CrearRespuestas(miembros); // Pasar los miembros creados
                CrearReacciones(miembros); // Pasar los miembros creados

                TempData["Mensaje"] = $"Puede iniciar sesión con {Config.MiembroEmail}{Config.Dominio}, o {Config.AdministradorEmail}{Config.Dominio}, siempre todos con la pass {Config.LoginPath}";
            }
            catch (Exception e)
            {
                return Content($"Error: {e.Message}\n\nInner Exception: {e.InnerException?.Message}");
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task CrearRoles()
        {
            foreach (var rolName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(rolName))
                {
                    await _roleManager.CreateAsync(new Rol(rolName));
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
                    await AgregarARoles(usuario, new List<string> { "ADMINISTRADOR" });
                }
            }
        }

        private async Task<List<Miembro>> CrearMiembros()
        {
            List<Miembro> miembrosCreados = new List<Miembro>();

            if (!_contexto.Miembros.Any())
            {
                for (int i = 1; i <= 5; i++)
                {
                    Miembro miembro = new()
                    {
                        Email = $"{Config.MiembroEmail}{i}{Config.Dominio}",
                        UserName = $"{Config.MiembroEmail}{i}{Config.Dominio}",
                        Apellido = Config.MiembroRolName.ToUpper(),
                        Nombre = $"{Config.NombreBaseMiembro.ToUpper()}{i}",
                        FechaAlta = DateTime.Now,
                    };

                    var resultadoCreacion = await _userManager.CreateAsync(miembro, Config.GenericPass);

                    if (resultadoCreacion.Succeeded)
                    {
                        await AgregarARoles(miembro, new List<string> { "MIEMBRO" });
                        miembrosCreados.Add(miembro);
                    }
                }
            }

            return miembrosCreados;
        }

        private async Task AgregarARoles(Usuario usuario, List<string> roles)
        {
            await _userManager.AddToRolesAsync(usuario, roles);
        }

        private void CrearCategorias()
        {
            if (!_contexto.Categorias.Any())
            {
                List<Categoria> categorias = new()
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
        }

        private void CrearEntradas(List<Miembro> miembros)
        {
            if (!_contexto.Entradas.Any() && miembros.Any())
            {
                var categorias = _contexto.Categorias.ToList();
                List<Entrada> entradas = new()
                {
                    new Entrada() { Titulo = "Últimos avances en IA", Descripcion = "Contenido sobre IA", CategoriaId = categorias.First(c => c.Nombre == "Tecnologia").CategoriaId, MiembroId = miembros[0].Id },
                    new Entrada() { Titulo = "Resumen del partido", Descripcion = "Contenido del partido de futbol", CategoriaId = categorias.First(c => c.Nombre == "Futbol").CategoriaId, MiembroId = miembros[1].Id },
                    new Entrada() { Titulo = "Descubrimientos recientes", Descripcion = "Contenido sobre ciencia", CategoriaId = categorias.First(c => c.Nombre == "Ciencia").CategoriaId, MiembroId = miembros[2].Id },
                    new Entrada() { Titulo = "Nuevos álbumes de música", Descripcion = "Contenido sobre música", CategoriaId = categorias.First(c => c.Nombre == "Musica").CategoriaId, MiembroId = miembros[3].Id },
                    new Entrada() { Titulo = "Estrenos de cine", Descripcion = "Contenido sobre cine", CategoriaId = categorias.First(c => c.Nombre == "Cine").CategoriaId, MiembroId = miembros[4].Id },
                    new Entrada() { Titulo = "Novedades literarias", Descripcion = "Contenido sobre literatura", CategoriaId = categorias.First(c => c.Nombre == "Literatura").CategoriaId, MiembroId = miembros[0].Id }
                };

                _contexto.Entradas.AddRange(entradas);
                _contexto.SaveChanges();
            }
        }

        private void CrearPreguntas(List<Miembro> miembros)
        {
            if (!_contexto.Preguntas.Any() && miembros.Any())
            {
                var entradas = _contexto.Entradas.ToList();
                List<Pregunta> preguntas = new()
                {
                    new Pregunta() { Descripcion = "¿Qué opinas sobre la IA?", EntradaId = entradas.First(e => e.Titulo == "Últimos avances en IA").Id, MiembroId = miembros[0].Id },
                    new Pregunta() { Descripcion = "¿Quién fue el mejor jugador?", EntradaId = entradas.First(e => e.Titulo == "Resumen del partido").Id, MiembroId = miembros[1].Id },
                    new Pregunta() { Descripcion = "¿Cuál es el impacto de este descubrimiento?", EntradaId = entradas.First(e => e.Titulo == "Descubrimientos recientes").Id, MiembroId = miembros[2].Id },
                    new Pregunta() { Descripcion = "¿Cuál es tu álbum favorito?", EntradaId = entradas.First(e => e.Titulo == "Nuevos álbumes de música").Id, MiembroId = miembros[3].Id },
                    new Pregunta() { Descripcion = "¿Cuál película esperas más?", EntradaId = entradas.First(e => e.Titulo == "Estrenos de cine").Id, MiembroId = miembros[4].Id },
                    new Pregunta() { Descripcion = "¿Qué libro recomendarías?", EntradaId = entradas.First(e => e.Titulo == "Novedades literarias").Id, MiembroId = miembros[0].Id }
                };

                _contexto.Preguntas.AddRange(preguntas);
                _contexto.SaveChanges();
            }
        }

        private void CrearRespuestas(List<Miembro> miembros)
        {
            if (!_contexto.Respuestas.Any() && miembros.Any())
            {
                var preguntas = _contexto.Preguntas.ToList();
                List<Respuesta> respuestas = new()
                {
                    new Respuesta() { Descripcion = "La IA es el futuro.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué opinas sobre la IA?").PreguntaId, MiembroId = miembros[0].Id },
                    new Respuesta() { Descripcion = "El mejor jugador fue Messi.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Quién fue el mejor jugador?").PreguntaId, MiembroId = miembros[1].Id },
                    new Respuesta() { Descripcion = "El impacto es significativo.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es el impacto de este descubrimiento?").PreguntaId, MiembroId = miembros[2].Id },
                    new Respuesta() { Descripcion = "Me encanta el nuevo álbum de Taylor Swift.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es tu álbum favorito?").PreguntaId, MiembroId = miembros[3].Id },
                    new Respuesta() { Descripcion = "Espero la nueva película de Marvel.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál película esperas más?").PreguntaId, MiembroId = miembros[4].Id },
                    new Respuesta() { Descripcion = "Recomiendo 'Cien Años de Soledad'.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué libro recomendarías?").PreguntaId, MiembroId = miembros[0].Id }
                };

                _contexto.Respuestas.AddRange(respuestas);
                _contexto.SaveChanges();
            }
        }

        private void CrearReacciones(List<Miembro> miembros)
        {
            if (!_contexto.Reacciones.Any() && miembros.Any())
            {
                var respuestas = _contexto.Respuestas.ToList();
                List<Reaccion> reacciones = new()
                {
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "La IA es el futuro.").RespuestaId, MiembroId = miembros[0].Id },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "El mejor jugador fue Messi.").RespuestaId, MiembroId = miembros[1].Id },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "El impacto es significativo.").RespuestaId, MiembroId = miembros[2].Id },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Taylor Swift.").RespuestaId, MiembroId = miembros[3].Id },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId, MiembroId = miembros[4].Id },
                    new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Recomiendo 'Cien Años de Soledad'.").RespuestaId, MiembroId = miembros[0].Id }
                };

                _contexto.Reacciones.AddRange(reacciones);
                _contexto.SaveChanges();
            }
        }
    }
}
