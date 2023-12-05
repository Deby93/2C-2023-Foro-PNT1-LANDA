using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class RespuestasController : Controller
    {
        private readonly ForoContexto _context;

        public RespuestasController(ForoContexto context)
        {
            _context = context;
        }

        // GET: Respuestas
        public async Task<IActionResult> Index()
        {
            var foroContexto = _context.Respuestas.Include(r => r.Miembro).Include(r => r.Pregunta);
            return View(await foroContexto.ToListAsync());
        }

        // GET: Respuestas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // GET: Respuestas/Create
        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido");
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "PreguntaId", "Descripcion");
            return View();
        }

        // POST: Respuestas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RespuestaId,PreguntaId,MiembroId,Descripcion,Fecha")] Respuesta respuesta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(respuesta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "PreguntaId", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // GET: Respuestas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "PreguntaId", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // POST: Respuestas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RespuestaId,PreguntaId,MiembroId,Descripcion,Fecha")] Respuesta respuesta)
        {
            if (id != respuesta.RespuestaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(respuesta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RespuestaExists(respuesta.RespuestaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", respuesta.MiembroId);
            ViewData["PreguntaId"] = new SelectList(_context.Preguntas, "PreguntaId", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        // GET: Respuestas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _context.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        // POST: Respuestas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Respuestas == null)
            {
                return Problem("Entity set 'ForoContexto.Respuestas'  is null.");
            }
            var respuesta = await _context.Respuestas.FindAsync(id);
            if (respuesta != null)
            {
                _context.Respuestas.Remove(respuesta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RespuestaExists(int id)
        {
          return (_context.Respuestas?.Any(e => e.RespuestaId == id)).GetValueOrDefault();
        }
    }
}
