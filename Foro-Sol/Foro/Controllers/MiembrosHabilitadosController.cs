using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    [Authorize(Roles = Config.Miembro)]

    public class MiembrosHabilitadosController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;

        public MiembrosHabilitadosController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        // GET: MiembrosHabilitados
        public async Task<IActionResult> Index()
        {
            var foroContexto = _contexto.MiembrosHabilitados.Include(m => m.Entrada).Include(m => m.Miembro);
            return View(await foroContexto.ToListAsync());
        }

        // GET: MiembrosHabilitados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _contexto.MiembrosHabilitados == null)
            {
                return NotFound();
            }

            var miembrosHabilitados = await _contexto.MiembrosHabilitados
                .Include(m => m.Entrada)
                .Include(m => m.Miembro)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (miembrosHabilitados == null)
            {
                return NotFound();
            }

            return View(miembrosHabilitados);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo");
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntradaId,Habilitado")] MiembrosHabilitados miembrosHabilitados)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (ModelState.IsValid)
            {
                if (MiembroIdEncontrado != null)
                {
                    miembrosHabilitados = new MiembrosHabilitados()
                    {
                        MiembroId = MiembroIdEncontrado,
                        EntradaId = miembrosHabilitados.EntradaId,
                        Habilitado = miembrosHabilitados.Habilitado,
                    };
                }
                else
                {
                    NotFound();
                }
                _contexto.Add(miembrosHabilitados);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index", "Miembros");
            }
                ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo", miembrosHabilitados.EntradaId);
                ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
                return View(miembrosHabilitados);
        }
        

        // GET: MiembrosHabilitados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (id == null || _contexto.MiembrosHabilitados == null)
            {
                return NotFound();
            }

            var miembrosHabilitados = await _contexto.MiembrosHabilitados.FindAsync(id);
            if (miembrosHabilitados == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo", miembrosHabilitados.EntradaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido",MiembroIdEncontrado);
            return View(miembrosHabilitados);
        }

        // POST: MiembrosHabilitados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EntradaId,Habilitado")] MiembrosHabilitados miembrosHabilitados)
        {
            if (id != miembrosHabilitados.MiembroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexto.Update(miembrosHabilitados);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembrosHabilitadosExists(miembrosHabilitados.MiembroId))
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
            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo", miembrosHabilitados.EntradaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", miembrosHabilitados.MiembroId);
            return View(miembrosHabilitados);
        }

        // GET: MiembrosHabilitados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.MiembrosHabilitados == null)
            {
                return NotFound();
            }

            var miembrosHabilitados = await _contexto.MiembrosHabilitados
                .Include(m => m.Entrada)
                .Include(m => m.Miembro)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (miembrosHabilitados == null)
            {
                return NotFound();
            }

            return View(miembrosHabilitados);
        }

        // POST: MiembrosHabilitados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.MiembrosHabilitados == null)
            {
                return Problem("Entity set 'ForoContexto.MiembrosHabilitados'  is null.");
            }
            var miembrosHabilitados = await _contexto.MiembrosHabilitados.FindAsync(id);
            if (miembrosHabilitados != null)
            {
                _contexto.MiembrosHabilitados.Remove(miembrosHabilitados);
            }

            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembrosHabilitadosExists(int id)
        {
            return (_contexto.MiembrosHabilitados?.Any(e => e.MiembroId == id)).GetValueOrDefault();
        }
    }
}
