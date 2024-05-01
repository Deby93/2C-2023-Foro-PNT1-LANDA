using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace Foro
{
    public class ReaccionesController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;
        public ReaccionesController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;

        }

        // GET: Reacciones
        public async Task<IActionResult> Index()
        {
            var foroContexto = _contexto.Reacciones.Include(r => r.Miembro)
                .Include(r => r.Respuesta);
            return View(await foroContexto.ToListAsync());
        }

        // GET: Reacciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _contexto.Reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _contexto.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // GET: Reacciones/Create
        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido");
            ViewData["RespuestaId"] = new SelectList(_contexto.Respuestas, "RespuestaId", "Descripcion");
            return View();
        }

        // POST: Reacciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RespuestaId,Fecha,MeGusta")] Reaccion reaccion)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (ModelState.IsValid)
            {
                if (MiembroIdEncontrado != null)
                {
                    reaccion = new Reaccion()
                    {
                        RespuestaId = reaccion.RespuestaId,
                        MiembroId = MiembroIdEncontrado,
                        Fecha = DateTime.Now,
                        MeGusta=reaccion.MeGusta,
                    };
                }
                else
                {
                    NotFound();
                }
                _contexto.Add(reaccion);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index", "Respuestas");

            }
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
            ViewData["RespuestaId"] = new SelectList(_contexto.Respuestas, "RespuestaId", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _contexto.Reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _contexto.Reacciones.FindAsync(id);
            if (reaccion == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_contexto.Respuestas, "RespuestaId", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // POST: Reacciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RespuestaId,MiembroId,Fecha,MeGusta")] Reaccion reaccion)
        {
            if (id != reaccion.MiembroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _contexto.Update(reaccion);
                    await _contexto.SaveChangesAsync();
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
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", reaccion.MiembroId);
            ViewData["RespuestaId"] = new SelectList(_contexto.Respuestas, "RespuestaId", "Descripcion", reaccion.RespuestaId);
            return View(reaccion);
        }

        // GET: Reacciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.Reacciones == null)
            {
                return NotFound();
            }

            var reaccion = await _contexto.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (reaccion == null)
            {
                return NotFound();
            }

            return View(reaccion);
        }

        // POST: Reacciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.Reacciones == null)
            {
                return Problem("Entity set 'ForoContexto.Reaccion'  is null.");
            }
            var reaccion = await _contexto.Reacciones.FindAsync(id);
            if (reaccion != null)
            {
                _contexto.Reacciones.Remove(reaccion);
            }
            
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReaccionExists(int id)
        {
          return (_contexto.Reacciones?.Any(e => e.MiembroId == id)).GetValueOrDefault();
        }
    }
}
