using Foro.Helpers;
using Foro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace Foro.Controllers
{

    public class RespuestasController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;
        private const int UMBRAL_DISLIKES = 10; 


        public RespuestasController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;

        }


        public IActionResult Index()
        {
            var respuestas = _contexto.Respuestas.ToList();

            return View(respuestas);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var respuesta = await _contexto.Respuestas
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        [Authorize(Roles = Config.MiembroRolName)]

        public IActionResult Create()
        {
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido");
            ViewData["PreguntaId"] = new SelectList(_contexto.Preguntas, "PreguntaId", "Descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> Create([Bind("PreguntaId,Descripcion")] Respuesta respuesta)
        {
            var userClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            int MiembroIdEncontrado = userClaim != null ? Int32.Parse(userClaim.Value) : 0;

            if (ModelState.IsValid)
            {
                if (MiembroIdEncontrado != 0)
                {
                    var preguntaAsociada = await _contexto.Preguntas.FindAsync(respuesta.PreguntaId);
                    if (preguntaAsociada == null || (bool)!preguntaAsociada.Activa)
                    {
                        ModelState.AddModelError(string.Empty, "No se puede crear una respuesta para una pregunta inactiva.");
                        return View(respuesta);
                    }

                    if (preguntaAsociada.MiembroId != MiembroIdEncontrado)
                    {
                        bool existeRespuestaConMismaDescripcion = _contexto.Respuestas
                            .Any(r => r.Descripcion == respuesta.Descripcion && r.PreguntaId == respuesta.PreguntaId);

                        if (existeRespuestaConMismaDescripcion)
                        {
                            ModelState.AddModelError("Descripcion", "Ya existe una respuesta con esa descripción.");
                            return View(respuesta);
                        }

                        respuesta.MiembroId = MiembroIdEncontrado;
                        respuesta.Fecha = DateTime.Now;

                        _contexto.Add(respuesta);
                        await _contexto.SaveChangesAsync();
                        return RedirectToAction("Index", "Preguntas");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "No puedes responder a tu propia pregunta.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            ViewData["PreguntaId"] = new SelectList(_contexto.Preguntas, "PreguntaId", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> Edit(int? id)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));

            if (id == null || _contexto.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _contexto.Respuestas.FindAsync(id);
            if (respuesta == null)
            {
                return NotFound();
            }
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
            ViewData["PreguntaId"] = new SelectList(_contexto.Preguntas, "PreguntaId", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> Edit(int id, [Bind("RespuestaId,PreguntaId,Descripcion,Fecha")] Respuesta respuesta)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));

            if (id != respuesta.RespuestaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var respuestaEnDb = await _contexto.Respuestas.FindAsync(id);
                    if (respuestaEnDb == null)
                    {
                        return NotFound();
                    }

                    respuestaEnDb.MiembroId = MiembroIdEncontrado;
                    respuestaEnDb.Miembro = respuesta.Miembro;
                    respuestaEnDb.Descripcion = respuesta.Descripcion;
                    respuestaEnDb.Fecha = respuesta.Fecha;
                    respuestaEnDb.Reacciones = respuesta.Reacciones;
                    _contexto.Respuestas.Update(respuestaEnDb);
                    await _contexto.SaveChangesAsync();

                    return RedirectToAction("Details", "Respuestas", new { id = respuesta.RespuestaId });
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
            }
            ViewData["MiembroId"] = new SelectList(_contexto.Miembros, "id", "Apellido", MiembroIdEncontrado);
            ViewData["PreguntaId"] = new SelectList(_contexto.Preguntas, "PreguntaId", "Descripcion", respuesta.PreguntaId);
            return View(respuesta);
        }

        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.Respuestas == null)
            {
                return NotFound();
            }

            var respuesta = await _contexto.Respuestas
                .Include(r => r.Reacciones)
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            if (respuesta == null)
            {
                return NotFound();
            }

            return View(respuesta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Config.MiembroRolName)]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.Respuestas == null)
            {
                return Problem("Entity set 'ForoContexto.Respuestas'  is null.");
            }
            var respuesta = await _contexto.Respuestas
                .Include(r => r.Reacciones)
                .Include(r => r.Miembro)
                .Include(r => r.Pregunta)
                .FirstOrDefaultAsync(m => m.RespuestaId == id);
            
            if (respuesta != null)
            {
                _contexto.Reacciones.RemoveRange(respuesta.Reacciones);
                _contexto.Respuestas.Remove(respuesta);

                await _contexto.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = Config.MiembroRolName)]

        private bool RespuestaExists(int id)
        {
            return (_contexto.Respuestas?.Any(e => e.RespuestaId == id)).GetValueOrDefault();
        }
    }
}
