using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Foro.Data;
using Foro.Helpers;
using Foro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foro.Controllers
{
    [Authorize(Roles = Config.MiembroRolName)]
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

        public async Task<IActionResult> Index()
        {
            var reacciones = await _contexto.Reacciones
                .Include(r => r.Miembro)
                .Include(r => r.Respuesta)
                .ToListAsync();
            return View(reacciones);
        }

        [HttpPost]
        public async Task<IActionResult> Like(int respuestaId, int preguntaId)
        {
            return await Reaccionar(respuestaId, preguntaId, true);
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int respuestaId, int preguntaId)
        {
            return await Reaccionar(respuestaId, preguntaId, false);
        }

        private async Task<IActionResult> Reaccionar(int respuestaId, int preguntaId, bool meGusta)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdString);
            var reaccionador = await _contexto.Respuestas.FindAsync(respuestaId);

            if (!User.Identity.IsAuthenticated || reaccionador == null || reaccionador.MiembroId == userId)
            {
                Console.WriteLine("no se puede reaccionar a la respuesta que es de uno");
                return RedirectToAction("Details", "Preguntas", new { id = preguntaId });

            }

            var existeReaccion = await _contexto.Reacciones
                .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == userId);

            if (existeReaccion == null)
            {
                var nuevaReaccion = new Reaccion
                {
                    RespuestaId = respuestaId,
                    MiembroId = userId,
                    Fecha = DateTime.Now,
                    MeGusta = meGusta,
                };
                _contexto.Reacciones.Add(nuevaReaccion);
            }
            else
            {
                _contexto.Reacciones.Remove(existeReaccion);
            }

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar la reacción. Inténtalo de nuevo.");
                return RedirectToAction("Details", "Preguntas", new { id = preguntaId });
            }

            if (preguntaId == 0)
            {
                return NotFound("La pregunta no fue encontrada.");

            }

            return RedirectToAction("Details", "Preguntas", new { id = preguntaId });
        }
    }
}
