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
        public async Task<IActionResult> Like(int respuestaId, string reactionType)
        {
            // Obtén el userId del usuario actual
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                // Verifica si ya existe una reacción del usuario para la respuesta específica
                var existeReaccion = await _contexto.Reacciones
                    .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == userId);

                if (existeReaccion != null)
                {
                    if (reactionType == "None")
                    {
                        existeReaccion.MeGusta = reactionType == "Like";
                        _contexto.Reacciones.Update(existeReaccion);
                    }
                    else if ((bool)(existeReaccion.MeGusta = reactionType == "Dislike"))
                    {
                        existeReaccion.MeGusta = reactionType == "Like";
                        _contexto.Reacciones.Update(existeReaccion);
                    }
                }
                else
                {
                    // Crea una nueva reacción si no existe ninguna para la respuesta y el usuario
                    var nuevaReaccion = new Reaccion
                    {
                        RespuestaId = respuestaId,
                        MiembroId = userId,
                        Fecha = DateTime.Now,
                        MeGusta = reactionType == "Like"
                    };
                    _contexto.Reacciones.Add(nuevaReaccion);
                }

                await _contexto.SaveChangesAsync();
                return RedirectToAction("Details", "Respuestas", new { id = respuestaId });
            }
            else
            {
                return BadRequest("El ID del miembro no es valido.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DisLike(int respuestaId, string reactionType)
        {
            // Obtén el userId del usuario actual
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                // Verifica si ya existe una reacción del usuario para la respuesta específica
                var existeReaccion = await _contexto.Reacciones
                    .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == userId);

                if (existeReaccion != null)
                {
                    if (reactionType == "None")
                    {
                        existeReaccion.MeGusta = reactionType == "DisLike";
                        _contexto.Reacciones.Update(existeReaccion);
                    }
                    else if ((bool)(existeReaccion.MeGusta = reactionType == "Like"))
                    {
                        existeReaccion.MeGusta = reactionType == "DisLike";
                        _contexto.Reacciones.Update(existeReaccion);
                    }
                }
                else
                {
                    // Crea una nueva reacción si no existe ninguna para la respuesta y el usuario
                    var nuevaReaccion = new Reaccion
                    {
                        RespuestaId = respuestaId,
                        MiembroId = userId,
                        Fecha = DateTime.Now,
                        MeGusta = reactionType == "DisLike"
                    };
                    _contexto.Reacciones.Add(nuevaReaccion);
                }

                await _contexto.SaveChangesAsync();
                return RedirectToAction("Details", "Respuestas", new { id = respuestaId });
            }
            else
            {
                return BadRequest("El ID del miembro no es valido.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Reset(int respuestaId, string reactionType)
        {
            // Obtén el userId del usuario actual
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdString, out int userId))
            {
                // Verifica si ya existe una reacción del usuario para la respuesta específica
                var existeReaccion = await _contexto.Reacciones
                    .FirstOrDefaultAsync(r => r.RespuestaId == respuestaId && r.MiembroId == userId);

                if (existeReaccion != null)
                {
                    if (reactionType == "Like")
                    {
                        existeReaccion.MeGusta = reactionType == "None";
                        _contexto.Reacciones.Update(existeReaccion);
                    }
                    else if ((bool)(existeReaccion.MeGusta = reactionType == "DisLike"))
                    {
                        existeReaccion.MeGusta = reactionType == "None";
                        _contexto.Reacciones.Update(existeReaccion);
                    }
                }
                else
                {
                    // Crea una nueva reacción si no existe ninguna para la respuesta y el usuario
                    var nuevaReaccion = new Reaccion
                    {
                        RespuestaId = respuestaId,
                        MiembroId = userId,
                        Fecha = DateTime.Now,
                        MeGusta = reactionType == "None"
                    };
                    _contexto.Reacciones.Add(nuevaReaccion);
                }

                await _contexto.SaveChangesAsync();
                return RedirectToAction("Details", "Respuestas", new { id = respuestaId });
            }
            else
            {
                return BadRequest("El ID del miembro no es valido.");
            }
        }

    }
}