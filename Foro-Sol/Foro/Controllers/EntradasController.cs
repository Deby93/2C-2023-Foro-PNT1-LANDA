using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;




namespace Foro
{
    public class EntradasController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;
        public EntradasController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;

        }

        public ActionResult Index()
        {
            // Obtener la entrada con más dislikes
            var entradaConMasDislikes = _contexto.Entradas
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                        .ThenInclude(r => r.Reacciones)
                .SelectMany(e => e.Preguntas.SelectMany(p =>
                    p.Respuestas.Select(r => new
                    {
                        Entrada = e,
                        Dislikes = r.Reacciones.Count(re => re.MeGusta.HasValue && !re.MeGusta.Value)
                    })
                ))
                .OrderByDescending(x => x.Dislikes)
                .FirstOrDefault();

            // Obtener el ID de la entrada con más dislikes
            int? idEntradaConMasDislikes = null;
            if (entradaConMasDislikes != null)
            {
                idEntradaConMasDislikes = entradaConMasDislikes.Entrada.Id;
            }

            ViewBag.EntradaConMasDislikesId = idEntradaConMasDislikes;

            // Obtener las entradas para la vista, incluyendo las relaciones necesarias
            var entradas = _contexto.Entradas
                .Include(e => e.Preguntas)
                    .ThenInclude(p => p.Respuestas)
                        .ThenInclude(r => r.Reacciones)
                .Include(e => e.MiembrosHabilitados) // Incluir MiembrosHabilitados
        .ToList();

            return View(entradas);
        }
        public async Task<IActionResult> Details(int? id)
        {
            Entrada unaEntrada = _contexto.Entradas.FirstOrDefault(p => p.Id == id);

            if (unaEntrada != null)
            {
                if ((bool)unaEntrada.Privada)
                {
                    if ((User.IsInRole(Config.MiembroRolName)))
                    {
                        int miID = Int32.Parse(User.Claims.First().Value);
                        var estaPendienteDeAutorizacion = _contexto.MiembrosHabilitados.Any((mh => mh.MiembroId == miID && mh.EntradaId == id && !mh.Habilitado));
                        bool habilitado = _contexto.MiembrosHabilitados.Any(mh => mh.EntradaId == id && mh.MiembroId == miID && mh.Habilitado);
                        if ((id == null || estaPendienteDeAutorizacion || (unaEntrada.MiembroId != miID && !habilitado && (bool)unaEntrada.Privada)))
                        {
                            return NotFound();
                        }
                    }
                }
            }
            var entrada = await _contexto.Entradas
                                  .Include(e => e.Categoria)
                                  .Include(e => e.Miembro)
                                  .Include(e => e.Preguntas)
                                      .ThenInclude(p => p.Respuestas)
                                          .ThenInclude(r => r.Reacciones)
                                  .Include(e => e.Preguntas)
                                      .ThenInclude(p => p.Miembro) 
                                  .OrderBy(p => p.Fecha)
                                  .FirstOrDefaultAsync(m => m.Id == id);

            var preguntasOrdenadas = entrada.Preguntas.OrderByDescending(p => p.Respuestas.Sum(r => r.Reacciones.Count(reaccion => (bool)reaccion.MeGusta))).ToList();

            List<Pregunta> listaDePreguntas = new();
            listaDePreguntas = await _contexto.Preguntas
              .Include(e => e.Miembro)
              .Include(e => e.Entrada)
              .OrderBy(p => p.Fecha)
              .Where(m => m.EntradaId == id).ToListAsync();

            if (entrada == null || listaDePreguntas == null)
            {
                return NotFound();
            }

            ViewBag.Entrada = entrada;
            if ((User.IsInRole(Config.MiembroRolName)))
            {
                int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewBag.UserId = userId;
            }
            ViewBag.Preguntas = preguntasOrdenadas;
           
            return View(listaDePreguntas);

        }

         [Authorize(Roles = Config.MiembroRolName)]

        [HttpGet]
        public IActionResult Create()
        {

            ViewData["CategoriaId"] = new SelectList(_contexto.Categorias.OrderBy(p => p.Nombre), "CategoriaId", "Nombre");
            return View();
        }

        [Authorize(Roles = Config.MiembroRolName)]
        // GET: Entradas/Create
        public async Task<IActionResult> Create([Bind("EntradaId,Titulo,Descripcion,CategoriaId, Fecha,Privada,Categoria,")] Entrada entrada)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (ModelState.IsValid)
            {
                var existingEntrada = await _contexto.Entradas.FirstOrDefaultAsync(e => e.Titulo.ToLower() == entrada.Titulo.ToLower() && e.Id != entrada.Id);
                if (existingEntrada != null)
                {
                    ModelState.AddModelError("entrada.Titulo", "Ya existe una entrada con ese titulo.");

                    return RedirectToAction("Create", "Entradas");
                }


                if (MiembroIdEncontrado != null)
                {
                    entrada = new Entrada()
                    {
                        Titulo = entrada.Titulo,
                        Descripcion = entrada.Descripcion,
                        MiembroId = MiembroIdEncontrado,
                        CategoriaId = entrada.CategoriaId,
                        Fecha = DateTime.Now,
                        Privada = entrada.Privada,
                        Miembro = entrada.Miembro,
                        Categoria = entrada.Categoria
                    };
                }
                else
                {
                    NotFound();
                }
                _contexto.Add(entrada);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Create", "Preguntas", new { id = entrada.Id });

            }
            ViewData["CategoriaId"] = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "Id", "Apellido", MiembroIdEncontrado);
            return View(entrada);
        }

        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> Edit(int? id)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (id == null || _contexto.Entradas == null)
            {
                return NotFound();
            }

            var entrada = await _contexto.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
            return View(entrada);
        }

        [Authorize(Roles = Config.MiembroRolName)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Fecha,Descripcion,CategoriaId,Privada")] Entrada entrada)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (id != entrada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var entradaEnDb = await _contexto.Entradas.FindAsync(id);
                    if (entradaEnDb == null)
                    {
                        return NotFound();
                    }

                    entradaEnDb.MiembroId = MiembroIdEncontrado;
                    entradaEnDb.Titulo = entrada.Titulo;
                    entradaEnDb.Privada = entrada.Privada;
                    entradaEnDb.Descripcion = entrada.Descripcion;
                    entradaEnDb.Fecha = entrada.Fecha;


                    _contexto.Entradas.Update(entradaEnDb);
                    await _contexto.SaveChangesAsync();

                    return RedirectToAction("Details", "Entradas", new { id = entradaEnDb.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntradaExists(entrada.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["CategoriaId"] = new SelectList(_contexto.Categorias, "CategoriaId", "Nombre", entrada.CategoriaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
            return View(entrada);
        }

        [Authorize(Roles = Config.AdministradorRolName)]
        // GET: Entradas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.Entradas == null)
            {
                return NotFound();
            }

            var entrada = await _contexto.Entradas
                .Include(e => e.Categoria)
                .Include(e => e.Miembro)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (entrada == null)
            {
                return NotFound();
            }

            return View(entrada);
        }

        [Authorize(Roles = Config.AdministradorRolName)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Obtener la entrada principal que se va a eliminar
            var entrada = await _contexto.Entradas
            .Include(e => e.Preguntas)
                .ThenInclude(p => p.Respuestas)
                 .ThenInclude(r => r.Reacciones)
                 .Include(e => e.MiembrosHabilitados)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (entrada == null)
            {
                return NotFound();
            }

            try
            {

                bool puedeBorrarse = entrada.Preguntas.Any(p => p.Respuestas.Any(r => r.ContadorDislikes >= Config.UMBRAL_DISLIKES));

                if (puedeBorrarse)
                {

                    // Eliminar las dependencias (preguntas, respuestas, reacciones) asociadas
                    var preguntas = _contexto.Preguntas.Where(p => p.EntradaId == id);
                    var respuestas = _contexto.Respuestas.Where(r => r.Pregunta.EntradaId == id);
                    var reacciones = _contexto.Reacciones.Where(re => re.Respuesta.Pregunta.EntradaId == id);
                    


                    _contexto.MiembrosHabilitados.RemoveRange(entrada.MiembrosHabilitados);
                    _contexto.Preguntas.RemoveRange(preguntas);
                    _contexto.Respuestas.RemoveRange(respuestas);
                    _contexto.Reacciones.RemoveRange(reacciones);

                    // Eliminar la entrada principal
                    _contexto.Entradas.Remove(entrada);

                    // Guardar los cambios en la base de datos
                    await _contexto.SaveChangesAsync();
                }
                else
                {

                    // No se puede borrar la entrada porque ninguna respuesta tiene suficientes dislikes
                    TempData["ErrorMessage"] = "La entrada no puede ser borrada porque ninguna respuesta tiene suficientes dislikes.";
                    return RedirectToAction(nameof(Index));

                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir al guardar los cambios
                ModelState.AddModelError("", "Error al eliminar la entrada: " + ex.Message);
                return View(entrada); // o redireccionar a una vista de error personalizada
            }
        }

        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> MisEntradas()
        {
            var idMiembro = Int32.Parse(_userManager.GetUserId(User));
            if (idMiembro == 0)
            {
                return NotFound();
            }
            List<Entrada> misEntradas = new();

            misEntradas = await _contexto.Entradas.
            Include(e => e.Categoria).
            Include(e => e.Miembro).
            OrderByDescending(e => e.Fecha).
              Where(e => e.MiembroId == idMiembro).
            ToListAsync();

            var listaDeCategorias = await _contexto.Categorias.
                Include(c => c.Entradas).
                OrderBy(c => c.Nombre).
                ToListAsync();


            ViewBag.Categorias = listaDeCategorias;
            return View(misEntradas);
        }
        private bool EntradaExists(int id)
        {
            return (_contexto.Entradas?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize(Roles = Config.MiembroRolName)]

        public int EntradaConMasDislikes()
        {
            Console.WriteLine("Inicio");
            var entradaConMasDislikes = _contexto.Entradas
                .SelectMany(e => e.Preguntas.SelectMany(p => p.Respuestas.Select(r => new
                {
                    Entrada = e,
                    Pregunta = p,
                    Respuesta = r,
                    Dislikes = r.Reacciones.Count(reaccion => (bool)!reaccion.MeGusta)
                })))
                .OrderByDescending(x => x.Dislikes)
                .FirstOrDefault();


            return (entradaConMasDislikes.Entrada.Id);

        }
        [Authorize(Roles = Config.MiembroRolName)]

        public IActionResult SolicitudesPendientes()
        {
            try
            {
                // Obtener el ID del usuario logueado de manera segura
                var userId = Int32.Parse(_userManager.GetUserId(User));
                if (userId == 0)
                {
                    throw new InvalidOperationException("No se pudo obtener el ID del usuario.");
                }

                // Obtener las entradas del usuario
                var entradasPropias = _contexto.Entradas
                                              .Include(e => e.MiembrosHabilitados)
                                  .ThenInclude(mh => mh.Miembro)
                                   .Include(e => e.Miembro)
                                              .Where(e => e.MiembroId == userId && (bool)e.Privada)
                                              .ToList();

                // Obtener las solicitudes pendientes de aprobación
                var solicitudesPendientes = entradasPropias.SelectMany(e => e.MiembrosHabilitados)
                                                           .Where(mh => !mh.Habilitado)
                                                           .ToList();

                return View(solicitudesPendientes);
            }
            catch (Exception ex)
            {
                // Manejar la excepción de manera apropiada (por ejemplo, registrarla y mostrar un mensaje de error amigable)
                ViewBag.ErrorMessage = "Ocurrió un error al procesar la solicitud: " + ex.Message;
                return View("Error"); // Asegúrate de tener una vista de Error para mostrar mensajes amigables
            }
        }
        [Authorize(Roles = Config.MiembroRolName)]

        public IActionResult SolicitarAprobacion(int id)
        {
            // Obtener el ID del usuario logueado
            int userId = Int32.Parse(_userManager.GetUserId(User));

            // Verificar si la entrada existe y es privada
            var entrada = _contexto.Entradas.Include(e => e.MiembrosHabilitados)
                                           .FirstOrDefault(e => e.Id == id && (bool) e.Privada);

            if (entrada == null)
            {
                return NotFound();
            }

            // Verificar si el usuario ya ha solicitado acceso
            var solicitudExistente = entrada.MiembrosHabilitados
                                            .FirstOrDefault(mh => mh.MiembroId == userId);

            if (solicitudExistente == null)
            {
                // Crear una nueva solicitud de aprobación
                var nuevaSolicitud = new MiembrosHabilitados
                {
                    EntradaId = id,
                    MiembroId = userId,
                    Habilitado = false // Estado pendiente de aprobación
                };
                _contexto.MiembrosHabilitados.Add(nuevaSolicitud);
                _contexto.SaveChanges();
            }

            // Redirigir a la vista de solicitudes pendientes
            return RedirectToAction("Index");
        }
    }
}
