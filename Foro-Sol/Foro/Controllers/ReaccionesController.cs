using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace Foro
{
    //[Authorize(Roles = Config.Miembro)]

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


        [HttpPost]
        public async Task<IActionResult> Like(int respuestaId, int preguntaId)
        {
            // Obtén el userId del usuario actual
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reaccionador = _contexto.Respuestas.FirstOrDefault(p => p.RespuestaId == respuestaId);
            if (!User.Identity.IsAuthenticated || reaccionador == null || userIdString[0] == reaccionador.MiembroId)
            {
                return NotFound();

            }
            else
            {
                var existeReaccion = await _contexto.Reacciones
                    .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == userIdString[0]);

                if (existeReaccion == null)
                {
                    var nuevaReaccion = new Reaccion
                    {
                        RespuestaId = respuestaId,
                        MiembroId = userIdString[0],
                        Fecha = DateTime.Now,
                        MeGusta = true,
                    };
                    _contexto.Reacciones.Add(nuevaReaccion);
                    await _contexto.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _contexto.Reacciones.Remove(existeReaccion);
                        await _contexto.SaveChangesAsync();
                    }
                    catch
                    {

                    }
                }
            }
            return RedirectToAction("Details", "Respuestas", new { id = preguntaId });
        }

        [HttpPost]
        public async Task<IActionResult> Dislike(int respuestaId, int preguntaId)
        {
            // Obtén el userId del usuario actual
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reaccionador = _contexto.Respuestas.FirstOrDefault(p => p.RespuestaId == respuestaId);
            if (!User.Identity.IsAuthenticated || reaccionador == null || userIdString[0] == reaccionador.MiembroId)
            {
                return NotFound();

            }
            else
            {
                var existeReaccion = await _contexto.Reacciones
                    .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == userIdString[0]);

                if (existeReaccion == null)
                {
                    var nuevaReaccion = new Reaccion
                    {
                        RespuestaId = respuestaId,
                        MiembroId = userIdString[0],
                        Fecha = DateTime.Now,
                        MeGusta = false,
                    };
                    _contexto.Reacciones.Add(nuevaReaccion);
                    await _contexto.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _contexto.Reacciones.Remove(existeReaccion);
                        await _contexto.SaveChangesAsync();
                    }
                    catch
                    {

                    }
                }
            }
            return RedirectToAction("Details", "Respuestas", new { id = preguntaId });
        }



    }
}