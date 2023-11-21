using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class MiembrosController : Controller
    {
        private readonly ForoContexto _context;

        public MiembrosController(ForoContexto context)
        {
            _context = context;
        }

        // GET: Miembros
        public  IActionResult Index()
        {
              return _context.Miembros != null ? 
                          View( _context.Miembros.ToList()) :
                          Problem("Entity set 'ForoContexto.Miembros'  is null.");
        }

        // GET: Miembros/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Miembros == null)
            {
                return NotFound();
            }

            var miembro =  _context.Miembros
                .FirstOrDefault(m => m.id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // GET: Miembros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Miembros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Telefono,id,Nombre,Apellido,FechaAlta,Email,Password")] Miembro miembro)
        {
            if (ModelState.IsValid)
            {
                _context.Miembros.Add(miembro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        // GET: Miembros/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Miembros == null)
            {
                return NotFound();
            }

            var miembro = _context.Miembros.Find(id);
            if (miembro == null)
            {
                return NotFound();
            }
            return View(miembro);
        }

        // POST: Miembros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Telefono,id,Nombre,Apellido,FechaAlta,Email,Password")] Miembro miembro)
        {
            if (id != miembro.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(miembro);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembro.id))
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
            return View(miembro);
        }

        // GET: Miembros/Delete/5
        public  IActionResult Delete(int? id)
        {
            if (id == null || _context.Miembros == null)
            {
                return NotFound();
            }

            var miembro =  _context.Miembros
                .FirstOrDefault(m => m.id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

        // POST: Miembros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Miembros == null)
            {
                return Problem("Entity set 'ForoContexto.Miembros'  is null.");
            }
            var miembro = _context.Miembros.Find(id);
            if (miembro != null)
            {
                _context.Miembros.Remove(miembro);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembroExists(int id)
        {
          return (_context.Miembros?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
