using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;



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
        // GET: Entradas
        //public async Task<IActionResult> Index()
        //{
        //    List<Entrada> listaDeEntradas = new();

        //    listaDeEntradas = await _contexto.Entradas.
        //       Include(e => e.Categoria).
        //       Include(e => e.Miembro).
        //       Include(e => e.MiembrosHabilitados).
        //       OrderByDescending(e => e.Fecha).
        //       ToListAsync();

        //    return View(listaDeEntradas);
        //}


        // GET: Entradas/Details/5
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
                .ToList();

            return View(entradas);
        }


        //  [Authorize(Roles = Config.Miembro)]

        [HttpGet]
        public IActionResult Create()
        {

            ViewData["CategoriaId"] = new SelectList(_contexto.Categorias.OrderBy(p => p.Nombre), "CategoriaId", "Nombre");
            return View();
        }
        // [Authorize(Roles = Config.Miembro)]

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

        [Authorize(Roles = Config.Miembro)]

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

        [Authorize(Roles = Config.Miembro)]
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

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Obtener la entrada principal que se va a eliminar
            var entrada = await _contexto.Entradas.FindAsync(id);
            if (entrada == null)
            {
                return NotFound();
            }

            try
            {
                // Eliminar las dependencias (preguntas, respuestas, reacciones) asociadas
                var preguntas = _contexto.Preguntas.Where(p => p.EntradaId == id);
                var respuestas = _contexto.Respuestas.Where(r => r.Pregunta.EntradaId == id);
                var reacciones = _contexto.Reacciones.Where(re => re.Respuesta.Pregunta.EntradaId == id);

                _contexto.Preguntas.RemoveRange(preguntas);
                _contexto.Respuestas.RemoveRange(respuestas);
                _contexto.Reacciones.RemoveRange(reacciones);

                // Eliminar la entrada principal
                _contexto.Entradas.Remove(entrada);

                // Guardar los cambios en la base de datos
                await _contexto.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que pueda ocurrir al guardar los cambios
                ModelState.AddModelError("", "Error al eliminar la entrada: " + ex.Message);
                return View(entrada); // o redireccionar a una vista de error personalizada
            }
        }





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
    }
}
