﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    //[Authorize(Roles = Config.Miembro)]

    public class PreguntasController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;

        public PreguntasController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;

        }

        // GET: Preguntas
        public async Task<IActionResult> Index()
        {
            var preguntas = await _contexto.Preguntas.ToListAsync();
            return View(preguntas);
        }

        // GET: Preguntas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pregunta = await _contexto.Preguntas
                .Include(p => p.Entrada)
                .Include(p => p.Miembro)
                .Include(p => p.Respuestas)
                .FirstOrDefaultAsync(m => m.PreguntaId == id);

            List<Respuesta> respuestas = new();
            respuestas = await _contexto.Respuestas
             .Include(p => p.Miembro)
             .Include(p => p.Pregunta)
             .Include(p => p.Reacciones)
             .OrderBy(p => p.Fecha)
             .Where(m => m.PreguntaId == id).ToListAsync();
           _= respuestas.Count;
            if (pregunta == null)
            {
                return NotFound();
            }
            var preguntas = _contexto.Preguntas.Where(p => p.PreguntaId== id).ToList();
            return View(preguntas);
            // ViewBag.idMasLikes = ObtenerMasLikes(pregunta);
            // ViewBag.idMasDisLikes = ObtenerMasDislike(pregunta);

            
        }
       

        // GET: Preguntas/Create
        public IActionResult Create()
        {

            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo");
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido");
            return View();
        }

        // POST: Preguntas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PreguntaId,EntradaId,Descripcion,Fecha,Activa")] Pregunta pregunta)
        {

            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (ModelState.IsValid)
            {
                if (MiembroIdEncontrado != null)
                {
                    pregunta = new Pregunta()
                    {

                        MiembroId = MiembroIdEncontrado,
                        EntradaId = pregunta.EntradaId,
                        Descripcion = pregunta.Descripcion,
                        Fecha = DateTime.Now,
                        Activa = pregunta.Activa,
                    };
                }
                else
                {
                    NotFound();
                }
                _contexto.Add(pregunta);
                await _contexto.SaveChangesAsync();
                return RedirectToAction("Index", "Entradas");

            }
            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
            return View(pregunta);
        }

        // GET: Preguntas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _contexto.Preguntas == null)
            {
                return NotFound();
            }

            var pregunta = await _contexto.Preguntas.FindAsync(id);
            if (pregunta == null)
            {
                return NotFound();
            }
            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "Id", "Titulo", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", pregunta.MiembroId);
            return View(pregunta);
        }

        // POST: Preguntas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("PreguntaId,EntradaId,Descripcion,Fecha,Activa")] Pregunta pregunta)
        {

            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));
            if (id != pregunta.PreguntaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var preguntaEnDb = await _contexto.Preguntas.FindAsync(id);
                    if (preguntaEnDb == null)
                    {
                        return NotFound();
                    }

                    preguntaEnDb.MiembroId = MiembroIdEncontrado;
                    preguntaEnDb.EntradaId = pregunta.EntradaId;
                    preguntaEnDb.Descripcion = pregunta.Descripcion;
                    preguntaEnDb.Fecha = pregunta.Fecha;
                    preguntaEnDb.Activa = pregunta.Activa;

                    _contexto.Preguntas.Update(preguntaEnDb);
                    await _contexto.SaveChangesAsync();

                    return RedirectToAction("Details", "Preguntas", new { id = pregunta.PreguntaId });
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
            }

            ViewData["EntradaId"] = new SelectList(_contexto.Entradas, "EntradaId", "Descripcion", pregunta.EntradaId);
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "Id", "Apellido", MiembroIdEncontrado);
            return View(pregunta);
        }
    

        
        // GET: Preguntas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.Preguntas == null)
            {
                return NotFound();
            }

            var pregunta = await _contexto.Preguntas
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.Preguntas == null)
            {
                return Problem("Entity set 'ForoContexto.Pregunta'  is null.");
            }
            var pregunta = await _contexto.Preguntas.FindAsync(id);
            if (pregunta != null)
            {
                _contexto.Preguntas.Remove(pregunta);
            }
            
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PreguntaExists(int id)
        {
          return (_contexto.Preguntas?.Any(e => e.PreguntaId == id)).GetValueOrDefault();
        }


    }
}
