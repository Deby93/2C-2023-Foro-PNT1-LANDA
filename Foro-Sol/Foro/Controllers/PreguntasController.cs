
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class PreguntasController : Controller
    {
        private readonly ForoContexto _context;

        public PreguntasController(ForoContexto context)
        {
            _context = context;
        }

        // GET: Preguntas
        public IActionResult Index()
        {
            var foroContexto = _context.Pregunta.Include(p => p.Entrada).Include(p => p.Miembro);
            return View( foroContexto.ToList());
        }

        // GET: Preguntas/Details/5
        public  IActionResult Details(int? id)
        {
            if (id == null || _context.Pregunta == null)
            {
                return NotFound();
            }

            var pregunta =  _context.Pregunta
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefault(m => m.PreguntaId == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // GET: Preguntas/Create
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo");
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido");
            return View();
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Create([Bind("PreguntaId,MiembroId,EntradaId,Descripcion,Fecha,Activa")] Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pregunta);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // GET: Preguntas/Edit/5
        public  IActionResult Edit(int? id)
        {
            if (id == null || _context.Pregunta == null)
            {
                return NotFound();
            }

            var pregunta =  _context.Pregunta.Find(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // POST: Preguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PreguntaId,MiembroId,EntradaId,Descripcion,Fecha,Activa")] Pregunta pregunta)
        {
            if (id != pregunta.PreguntaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pregunta);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PreguntaExists(pregunta.PreguntaId))
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
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // GET: Preguntas/Delete/5
        public  IActionResult Delete(int? id)
        {
            if (id == null || _context.Pregunta == null)
            {
                return NotFound();
            }

            var pregunta =  _context.Pregunta
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .FirstOrDefaultAsync(m => m.PreguntaId == id);
            if (pregunta == null)
            {
                return NotFound();
            }

            return View(pregunta);
        }

        // POST: Preguntas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
            if (_context.Pregunta == null)
            {
                return Problem("Entity set 'ForoContexto.Pregunta'  is null.");
            }
            var pregunta = _context.Pregunta.Find(id);
            if (pregunta != null)
            {
                _context.Pregunta.Remove(pregunta);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool PreguntaExists(int id)
        {
          return (_context.Pregunta?.Any(e => e.PreguntaId == id)).GetValueOrDefault();
        }
    }
}
