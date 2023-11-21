using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class ReaccionesController : Controller
    {
        private readonly ForoContexto _context;

        public ReaccionesController(ForoContexto context)
        {
            _context = context;
        }

        // GET: Reacciones
        public  IActionResult Index()
        {
            var foroContexto = _context.Reaccion.Include(r => r.Miembro).Include(r => r.Respuesta);
            return View( foroContexto.ToList());
        }

        // GET: Reacciones/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Reaccion == null)
            {
                return NotFound();
            }

            var reaccion =  _context.Reaccion
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefault(m => m.MiembroId == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // GET: Reacciones/Create
        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido");
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "RespuestaId", "Descripcion");
            return View();
        }

        // POST: Reacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("RespuestaId,MiembroId,Fecha,MeGusta")] Reaccion reaccion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reaccion);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "RespuestaId", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Reaccion == null)
            {
                return NotFound();
            }

            var reaccion =  _context.Reaccion.Find(id);
            if (reaccion == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "RespuestaId", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // POST: Reacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  IActionResult Edit(int id, [Bind("RespuestaId,MiembroId,Fecha,MeGusta")] Reaccion reaccion)
        {
            if (id != reaccion.MiembroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reaccion);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReaccionExists(reaccion.MiembroId))
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
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_context.Respuestas, "RespuestaId", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Reaccion == null)
            {
                return NotFound();
            }

            var reaccion =  _context.Reaccion
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefault(m => m.MiembroId == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // POST: Reacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public  IActionResult DeleteConfirmed(int id)
        {
            if (_context.Reaccion == null)
            {
                return Problem("Entity set 'ForoContexto.Reaccion'  is null.");
            }
            var reaccion = _context.Reaccion.Find(id);
            if (reaccion != null)
            {
                _context.Reaccion.Remove(reaccion);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaccionExists(int id)
        {
          return (_context.Reaccion?.Any(e => e.MiembroId == id)).GetValueOrDefault();
        }
    }
}
