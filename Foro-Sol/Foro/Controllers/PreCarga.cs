using Foro.Helpers;
using Foro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Foro.Controllers
{
    public class PreCarga : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<Rol> _roleManager;
        private readonly ForoContexto _contexto;
        private readonly List<string> roles = new() { Config.AdministradorRolName, Config.MiembroRolName };

        public PreCarga(UserManager<Usuario> userManager, RoleManager<Rol> roleManager, ForoContexto contexto)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
        }


        public async Task<IActionResult> Seed()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await CrearRoles();
                }

                await CrearAdministrador();
                await CrearMiembros();
                await CrearCategorias();
                await CrearEntradas();
                await CrearMiembrosHabilitados();
                await CrearPreguntas();
                await CrearRespuestas();
                await CrearReacciones();
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
        private static readonly Random random = new Random();

        private async Task CrearAdministrador()
        {
            string[] nombres = { "Dario", "Camila", "Luis", "Marina", "Carlos", "Andrea", "Jorge" };
            string[] apellidos = { "Hernadez", "Torres", "Batalla", "Ramirez", "Estevez", "Araujo", "Juarez" };
            var usuariosConRolAdmin = await _userManager.GetUsersInRoleAsync(Config.AdministradorRolName);
            if (usuariosConRolAdmin.Count == 0)
            {
                if (!_contexto.Usuarios.Any())
                {
                    Random random = new Random();
                    for (int i = 1; i <= 2; i++)
                    {
                        string nombreAleatorio = nombres[random.Next(nombres.Length)];
                        string apellidoAleatorio = apellidos[random.Next(apellidos.Length)];
                        Usuario usuario = new()
                        {
                            UserName = $"{Config.AdministradorEmail}{i}".ToUpper(),
                            Email = $"{Config.AdministradorEmail}{i}{Config.Dominio}".ToLower(),
                            Apellido = nombreAleatorio.ToUpper(),
                            Nombre = apellidoAleatorio.ToUpper(),
                            FechaAlta = DateTime.Now,
                        };

                        var resultadoCreacion = await _userManager.CreateAsync(usuario, Config.GenericPass);

                        if (resultadoCreacion.Succeeded)
                        {
                            await AgregarARoles(usuario, new List<string> { Config.AdministradorRolName });
                        }
                    }
                }
            }
        }



        private async Task<List<Miembro>> CrearMiembros()
        {
            List<Miembro> miembrosCreados = new List<Miembro>();

            string[] nombres = { "Juan", "María", "Pedro", "Ana", "Luisa", "Carlos", "Laura", "Javier", "Sofía" };
            string[] apellidos = { "García", "Martínez", "López", "Fernández", "Pérez", "González", "Rodríguez", "Sánchez", "Romero" };
            var usuariosConRolMiembro = await _userManager.GetUsersInRoleAsync(Config.MiembroRolName);
            if (usuariosConRolMiembro.Count == 0)
            {
                Random random = new Random();

                for (int i = 1; i <= 9; i++)
                {
                    string nombreAleatorio = nombres[random.Next(nombres.Length)];
                    string apellidoAleatorio = apellidos[random.Next(apellidos.Length)];

                    Miembro miembro = new()
                    {
                        Email = $"{Config.MiembroEmail}{i}{Config.Dominio}".ToLower(),
                        UserName = $"{Config.MiembroEmail}{i}",
                        Apellido = apellidoAleatorio.ToUpper(),
                        Nombre = nombreAleatorio.ToUpper(),
                        FechaAlta = DateTime.Now,
                        Telefono = $"112233445{i}"
                    };

                    var resultadoCreacion = await _userManager.CreateAsync(miembro, Config.GenericPass);

                    if (resultadoCreacion.Succeeded)
                    {
                        await AgregarARoles(miembro, new List<string> { Config.MiembroRolName });
                        miembrosCreados.Add(miembro);
                    }
                }
            }

            return miembrosCreados;
        }

        private async Task AgregarARoles(Miembro miembro, List<string> roles)
        {
            foreach (var rol in roles)
            {
                if (!await _userManager.IsInRoleAsync(miembro, rol))
                {
                    await _userManager.AddToRoleAsync(miembro, rol);
                }
            }
        }



        private async Task AgregarARoles(Usuario usuario, List<string> roles)
        {
            await _userManager.AddToRolesAsync(usuario, roles);
        }

        private async Task CrearCategorias()
        {
            if (!await _contexto.Categorias.AnyAsync())
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

                await _contexto.Categorias.AddRangeAsync(categorias);
                await _contexto.SaveChangesAsync(); ;
            }
        }

        private async Task CrearEntradas()
        {
            var miembros = await _userManager.GetUsersInRoleAsync(Config.MiembroRolName);
            if (!await _contexto.Entradas.AnyAsync() && miembros.Any())
            {
                DateTime dateTime1 = new DateTime(2024, 07, 02, 06, 52, 14);
                DateTime dateTime2 = new DateTime(2024, 07, 01, 07, 51, 16);
                DateTime dateTime3 = new DateTime(2024, 06, 28, 12, 50, 50);
                DateTime dateTime4 = new DateTime(2024, 07, 02, 18, 18, 18);
                DateTime dateTime5 = new DateTime(2024, 06, 17, 22, 59, 30);
                DateTime dateTime6 = new DateTime(2024, 07, 08, 14, 12, 44);
                DateTime dateTime7 = new DateTime(2021, 07, 07, 18, 50, 33);
                DateTime dateTime8 = new DateTime(2022, 03, 28, 03, 50, 50);
                DateTime dateTime9 = new DateTime(2022, 01, 30, 02, 50, 50);
                DateTime dateTime10 = new DateTime(2022, 02, 12, 06, 52, 14);
                DateTime dateTime11 = new DateTime(2021, 02, 05, 07, 51, 16);
                DateTime dateTime12 = new DateTime(2021, 11, 15, 12, 50, 50);
                DateTime dateTime13 = new DateTime(2021, 02, 01, 06, 51, 10);

                var categorias = await _contexto.Categorias.ToListAsync();
                List<Entrada> entradas = new List<Entrada>
        {
            new Entrada() { Titulo = "Últimos avances en IA", Descripcion = "Contenido sobre IA", Fecha = dateTime1, Privada = true, CategoriaId = categorias.First(c => c.Nombre == "Tecnologia").CategoriaId, MiembroId = miembros[0].Id },
            new Entrada() { Titulo = "Resumen del partido", Descripcion = "Contenido del partido de futbol", Fecha = dateTime2, Privada = false, CategoriaId = categorias.First(c => c.Nombre == "Futbol").CategoriaId, MiembroId = miembros[1].Id },
            new Entrada() { Titulo = "Descubrimientos recientes", Descripcion = "Contenido sobre ciencia", Fecha = dateTime3, Privada = true, CategoriaId = categorias.First(c => c.Nombre == "Ciencia").CategoriaId, MiembroId = miembros[2].Id },
            new Entrada() { Titulo = "Nuevos álbumes de música", Descripcion = "Contenido sobre música", Fecha = dateTime4, Privada = false, CategoriaId = categorias.First(c => c.Nombre == "Musica").CategoriaId, MiembroId = miembros[3].Id },
            new Entrada() { Titulo = "Estrenos de cine", Descripcion = "Contenido sobre cine", Fecha = dateTime5, Privada = false, CategoriaId = categorias.First(c => c.Nombre == "Cine").CategoriaId, MiembroId = miembros[4].Id },
            new Entrada() { Titulo = "Novedades literarias", Descripcion = "Contenido sobre literatura", Fecha = dateTime6, Privada = false, CategoriaId = categorias.First(c => c.Nombre == "Literatura").CategoriaId, MiembroId = miembros[0].Id }
        };

                await _contexto.Entradas.AddRangeAsync(entradas);
                await _contexto.SaveChangesAsync();
            }
        }


        private async Task CrearMiembrosHabilitados()
        {
            var miembros = await _userManager.GetUsersInRoleAsync(Config.MiembroRolName);
            if (!await _contexto.MiembrosHabilitados.AnyAsync() && miembros.Any())
            {
                var entradas = await _contexto.Entradas.ToListAsync();

                List<MiembrosHabilitados> miembrosHabilitados = new List<MiembrosHabilitados>
        {
            new MiembrosHabilitados() { EntradaId = entradas[0].Id, MiembroId = miembros[5].Id, Habilitado = true },
            new MiembrosHabilitados() { EntradaId = entradas[0].Id, MiembroId = miembros[1].Id, Habilitado = true },
            new MiembrosHabilitados() { EntradaId = entradas[0].Id, MiembroId = miembros[2].Id, Habilitado = true },
            new MiembrosHabilitados() { EntradaId = entradas[3].Id, MiembroId = miembros[2].Id, Habilitado = true },
            new MiembrosHabilitados() { EntradaId = entradas[3].Id, MiembroId = miembros[4].Id, Habilitado = true },
            new MiembrosHabilitados() { EntradaId = entradas[0].Id, MiembroId = miembros[3].Id, Habilitado = false },
            new MiembrosHabilitados() { EntradaId = entradas[3].Id, MiembroId = miembros[1].Id, Habilitado = false }
        };

                await _contexto.MiembrosHabilitados.AddRangeAsync(miembrosHabilitados);
                await _contexto.SaveChangesAsync();
            }
        }



        private async Task CrearPreguntas()
        {
            var miembros = await _userManager.GetUsersInRoleAsync(Config.MiembroRolName);
            if (!_contexto.Preguntas.Any() && miembros.Any())
            {
                DateTime dateTime1 = new(2024, 07, 02, 06, 52, 14);
                DateTime dateTime2 = new(2024, 07, 01, 07, 51, 16);
                DateTime dateTime3 = new(2024, 06, 28, 12, 50, 50);
                DateTime dateTime4 = new(2024, 07, 02, 18, 18, 18);
                DateTime dateTime5 = new(2024, 06, 17, 22, 59, 30);
                DateTime dateTime6 = new(2024, 07, 08, 14, 12, 44);
                DateTime dateTime7 = new(2025, 07, 07, 18, 50, 33);
                DateTime dateTime8 = new(2022, 03, 28, 03, 50, 50);
                DateTime dateTime9 = new(2022, 01, 30, 02, 50, 50);
                DateTime dateTime10 = new(2022, 02, 12, 06, 52, 14);
                DateTime dateTime11 = new(2021, 02, 05, 07, 51, 16);
                DateTime dateTime12 = new(2021, 11, 15, 12, 50, 50);
                DateTime dateTime13 = new(2021, 02, 01, 06, 51, 10);

                var entradas = await _contexto.Entradas.ToListAsync(); 

                List<Pregunta> preguntas = new()
        {
            new Pregunta() { Descripcion = "¿Qué opinas sobre la IA?", Fecha = dateTime1, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Últimos avances en IA").Id, MiembroId = miembros[0].Id },
            new Pregunta() { Descripcion = "¿Quién fue el mejor jugador?", Fecha = dateTime2, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Resumen del partido").Id, MiembroId = miembros[1].Id },
            new Pregunta() { Descripcion = "¿Cuál es el impacto de este descubrimiento?", Fecha = dateTime3, Activa = false, EntradaId = entradas.First(e => e.Titulo == "Descubrimientos recientes").Id, MiembroId = miembros[2].Id },
            new Pregunta() { Descripcion = "¿Cuál es tu álbum favorito?", Fecha = dateTime4, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Nuevos álbumes de música").Id, MiembroId = miembros[3].Id },
            new Pregunta() { Descripcion = "¿Cuál película esperas más?", Fecha = dateTime5, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Estrenos de cine").Id, MiembroId = miembros[4].Id },
            new Pregunta() { Descripcion = "¿Qué libro recomendarías?", Fecha = dateTime6, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Novedades literarias").Id, MiembroId = miembros[0].Id },
            new Pregunta() { Descripcion = "¿Qué opinas sobre el trabajo remoto?", Fecha = dateTime2, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Últimos avances en IA").Id, MiembroId = miembros[0].Id },
            new Pregunta() { Descripcion = "¿Quién fue el mejor entrenador?", Fecha = dateTime3, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Resumen del partido").Id, MiembroId = miembros[1].Id },
            new Pregunta() { Descripcion = "¿Cuál es el descubrimiento?", Fecha = dateTime4, Activa = false, EntradaId = entradas.First(e => e.Titulo == "Descubrimientos recientes").Id, MiembroId = miembros[2].Id },
            new Pregunta() { Descripcion = "¿Cuál es tu artista favorito?", Fecha = dateTime5, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Nuevos álbumes de música").Id, MiembroId = miembros[3].Id },
            new Pregunta() { Descripcion = "¿Cuál película te gustó más?", Fecha = dateTime6, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Estrenos de cine").Id, MiembroId = miembros[4].Id },
            new Pregunta() { Descripcion = "¿Qué libro te gustó?", Fecha = dateTime7, Activa = true, EntradaId = entradas.First(e => e.Titulo == "Novedades literarias").Id, MiembroId = miembros[0].Id }
        };

                _contexto.Preguntas.AddRange(preguntas);
                await _contexto.SaveChangesAsync();
            }
        }


        private async Task CrearRespuestas()
        {
            var miembros = await _userManager.Users.ToListAsync();
            if (!_contexto.Respuestas.Any())
            {
                var preguntas = await _contexto.Preguntas.ToListAsync();
                List<Respuesta> respuestas = new List<Respuesta>
        {
            new Respuesta { Descripcion = "La IA es el futuro.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué opinas sobre la IA?").PreguntaId, MiembroId = miembros[0].Id, Fecha = new DateTime(2024, 02, 09, 06, 13, 50) },
            new Respuesta { Descripcion = "El mejor jugador fue Messi.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Quién fue el mejor jugador?").PreguntaId, MiembroId = miembros[1].Id, Fecha = new DateTime(2024, 02, 09, 06, 14, 50) },
            new Respuesta { Descripcion = "El impacto es significativo.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es el impacto de este descubrimiento?").PreguntaId, MiembroId = miembros[2].Id, Fecha = new DateTime(2024, 02, 09, 06, 15, 50) },
            new Respuesta { Descripcion = "Me encanta el nuevo álbum de Maria Becerra.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es tu álbum favorito?").PreguntaId, MiembroId = miembros[3].Id, Fecha = new DateTime(2024, 02, 09, 06, 16, 50) },
            new Respuesta { Descripcion = "Espero la nueva película de Marvel.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál película esperas más?").PreguntaId, MiembroId = miembros[4].Id, Fecha = new DateTime(2024, 02, 09, 06, 17, 50) },
            new Respuesta { Descripcion = "Recomiendo 'Cien Años de Soledad'.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué libro recomendarías?").PreguntaId, MiembroId = miembros[0].Id, Fecha = new DateTime(2024, 02, 09, 06, 18, 50) },
            new Respuesta { Descripcion = "La IA es lo mejor.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué opinas sobre la IA?").PreguntaId, MiembroId = miembros[0].Id, Fecha = new DateTime(2024, 01, 09, 06, 14, 50) },
            new Respuesta { Descripcion = "El mejor jugador fue Angelito.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Quién fue el mejor jugador?").PreguntaId, MiembroId = miembros[1].Id, Fecha = new DateTime(2024, 01, 09, 06, 12, 50) },
            new Respuesta { Descripcion = "El impacto es significativo.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es el impacto de este descubrimiento?").PreguntaId, MiembroId = miembros[2].Id, Fecha = new DateTime(2024, 01, 09, 06, 15, 50) },
            new Respuesta { Descripcion = "Me encanta el nuevo álbum de Sam Smith.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál es tu álbum favorito?").PreguntaId, MiembroId = miembros[3].Id, Fecha = new DateTime(2024, 03, 09, 06, 15, 50) },
            new Respuesta { Descripcion = "Espero la nueva película de Intensamente.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Cuál película te gustó más?").PreguntaId, MiembroId = miembros[4].Id, Fecha = new DateTime(2024, 08, 10, 06, 16, 50) },
            new Respuesta { Descripcion = "Recomiendo 'Rebecca'.", PreguntaId = preguntas.First(p => p.Descripcion == "¿Qué libro te gustó?").PreguntaId, MiembroId = miembros[0].Id, Fecha = new DateTime(2023, 07, 09, 06, 16, 50) }
        };

                _contexto.Respuestas.AddRange(respuestas);
                await _contexto.SaveChangesAsync();
            }
        }




        private async Task CrearReacciones()
        {
            var miembros = await _userManager.GetUsersInRoleAsync(Config.MiembroRolName);
            if (!_contexto.Reacciones.Any() && miembros.Any())
            {
                var respuestas = await _contexto.Respuestas.ToListAsync(); 

                List<Reaccion> reacciones = new()
        {
            new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Recomiendo 'Cien Años de Soledad'.").RespuestaId, MiembroId = miembros[3].Id },
            new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Recomiendo 'Cien Años de Soledad'.").RespuestaId, MiembroId = miembros[1].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId, MiembroId = miembros[1].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId, MiembroId = miembros[2].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId, MiembroId = miembros[3].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId, MiembroId = miembros[0].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Espero la nueva película de Marvel.").RespuestaId, MiembroId = miembros[5].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Maria Becerra.").RespuestaId, MiembroId = miembros[4].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Maria Becerra.").RespuestaId, MiembroId = miembros[5].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Maria Becerra.").RespuestaId, MiembroId = miembros[6].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Maria Becerra.").RespuestaId, MiembroId = miembros[1].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Maria Becerra.").RespuestaId, MiembroId = miembros[2].Id },
            new Reaccion() { MeGusta = false, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Maria Becerra.").RespuestaId, MiembroId = miembros[7].Id },
            new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Me encanta el nuevo álbum de Sam Smith.").RespuestaId, MiembroId = miembros[8].Id },
            new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "El mejor jugador fue Messi.").RespuestaId, MiembroId = miembros[3].Id },
            new Reaccion() { MeGusta = true, RespuestaId = respuestas.First(r => r.Descripcion == "Recomiendo 'Rebecca'.").RespuestaId, MiembroId = miembros[4].Id },
        };

                _contexto.Reacciones.AddRange(reacciones);
                await _contexto.SaveChangesAsync(); 
            }
        }

    }
}