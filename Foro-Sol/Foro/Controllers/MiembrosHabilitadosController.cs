using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Foro;

namespace Foro.Controllers
{
    public class MiembrosHabilitadosController : Controller
    {
        private readonly ForoContexto _context;

        public MiembrosHabilitadosController(ForoContexto context)
        {
            _context = context;
        }

        // GET: MiembrosHabilitados
        public async Task<IActionResult> Index()
        {
            var foroContexto = _context.MiembrosHabilitados.Include(m => m.Entrada).Include(m => m.Miembro);
            return View(await foroContexto.ToListAsync());
        }

        // GET: MiembrosHabilitados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MiembrosHabilitados == null)
            {
                return NotFound();
            }

            var miembrosHabilitados = await _context.MiembrosHabilitados
                .Include(m => m.Entrada)
                .Include(m => m.Miembro)
                .FirstOrDefaultAsync(m => m.MiembroId == id);
            if (miembrosHabilitados == null)
            {
                return NotFound();
            }

            return View(miembrosHabilitados);
        }

        // GET: MiembrosHabilitados/Create
        public IActionResult Create()
        {
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo");
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido");
            return View();
        }

        // POST: MiembrosHabilitados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EntradaId,MiembroId,Habilitado")] MiembrosHabilitados miembrosHabilitados)
        {
            if (ModelState.IsValid)
            {
                _context.MiembrosHabilitados.Add(miembrosHabilitados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", miembrosHabilitados.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", miembrosHabilitados.MiembroId);
            return View(miembrosHabilitados);
        }

        // GET: MiembrosHabilitados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MiembrosHabilitados == null)
            {
                return NotFound();
            }

            var miembrosHabilitados = await _context.MiembrosHabilitados.FindAsync(id);
            if (miembrosHabilitados == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", miembrosHabilitados.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", miembrosHabilitados.MiembroId);
            return View(miembrosHabilitados);
        }

        // POST: MiembrosHabilitados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EntradaId,MiembroId,Habilitado")] MiembrosHabilitados miembrosHabilitados)
        {
            if (id != miembrosHabilitados.MiembroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(miembrosHabilitados);
                    await _context.SaveChangesAsync();
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
            ViewData["EntradaId"] = new SelectList(_context.Entradas, "Id", "Titulo", miembrosHabilitados.EntradaId);
            ViewData["MiembroId"] = new SelectList(_context.Miembros, "id", "Apellido", miembrosHabilitados.MiembroId);
            return View(miembrosHabilitados);
        }

        // GET: MiembrosHabilitados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MiembrosHabilitados == null)
            {
                return NotFound();
            }

            var miembrosHabilitados = await _context.MiembrosHabilitados
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
            if (_context.MiembrosHabilitados == null)
            {
                return Problem("Entity set 'ForoContexto.MiembrosHabilitados'  is null.");
            }
            var miembrosHabilitados = await _context.MiembrosHabilitados.FindAsync(id);
            if (miembrosHabilitados != null)
            {
                _context.MiembrosHabilitados.Remove(miembrosHabilitados);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembrosHabilitadosExists(int id)
        {
          return (_context.MiembrosHabilitados?.Any(e => e.MiembroId == id)).GetValueOrDefault();
        }
    }
}
