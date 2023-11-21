using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class UsuariosController : Controller
    {
        private readonly ForoContexto _context;

        public UsuariosController(ForoContexto context)
        {
            _context = context;
        }

        // GET: Usuarios
        public IActionResult Index()
        {
              return _context.Usuarios != null ? 
                          View(_context.Usuarios.ToList()) :
                          Problem("Entity set 'ForoContexto.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //resuelve ambiguedad en el metodo create que es sobrecargado
        [ValidateAntiForgeryToken]
        //Bind indicar que atributos  necesito
        public IActionResult Create([Bind("id,Nombre,Apellido,FechaAlta,Email,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // se pone Usuarios para que la variable sea del mismo tipo la que viene x parametro

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
                //redireccion al index 302
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario =  _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("id,Nombre,Apellido,FechaAlta,Email,Password")] Usuario usuario)
        {
            if (id != usuario.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDB = _context.Usuarios.Find(id);
                    if (usuarioDB !=null)
                    {
                        //Actualizamos
                       
                        usuarioDB.Nombre =usuario.Nombre;
                        usuarioDB.Apellido =usuario.Apellido;
                        usuarioDB.Email =usuario.Email;
                        usuarioDB.FechaAlta =usuario.FechaAlta;
                        usuarioDB.Password =usuario.Password;

                        _context.Usuarios.Update(usuarioDB);
                        _context.SaveChanges();

                    }
                    else
                    {
                        return NotFound();
                    }
                   
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'ForoContexto.Usuarios'  is null.");
            }
            var usuario =  _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
