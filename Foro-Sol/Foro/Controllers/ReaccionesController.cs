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

        // Acción para dar like a una respuesta
        public async Task<IActionResult> Like(int respuestaId)
        {
            // Lógica para dar like a la respuesta con el ID proporcionado
            // Por ejemplo:
            var respuesta = await _contexto.Respuestas.FindAsync(respuestaId);
            if (respuesta != null)
            {
                //respuesta.Likes++;
                await _contexto.SaveChangesAsync();
                TempData["Mensaje"] = "Has dado like a la respuesta.";
            }
            else
            {
                TempData["Mensaje"] = "La respuesta no se encontró.";
            }

            return RedirectToAction("Index", "Home"); // Redirige a la página principal u otra vista
        }

        // Acción para dar dislike a una respuesta
        public async Task<IActionResult> Dislike(int respuestaId)
        {
            // Lógica para dar dislike a la respuesta con el ID proporcionado
            // Por ejemplo:
            var respuesta = await _contexto.Respuestas.FindAsync(respuestaId);
            if (respuesta != null)
            {
               // respuesta.Dislikes++;
                await _contexto.SaveChangesAsync();
                TempData["Mensaje"] = "Has dado dislike a la respuesta.";
            }
            else
            {
                TempData["Mensaje"] = "La respuesta no se encontró.";
            }

            return RedirectToAction("Index", "Home"); // Redirige a la página principal u otra vista
        }

        // Acción para resetear las reacciones de una respuesta
        public async Task<IActionResult> ResetReactions(int respuestaId)
        {
            // Lógica para resetear las reacciones de la respuesta con el ID proporcionado
            // Por ejemplo:
            var respuesta = await _contexto.Respuestas.FindAsync(respuestaId);
            if (respuesta != null)
            {
              //  respuesta.Likes = 0;
               // respuesta.Dislikes = 0;
                await _contexto.SaveChangesAsync();
                TempData["Mensaje"] = "Se han reseteado las reacciones de la respuesta.";
            }
            else
            {
                TempData["Mensaje"] = "La respuesta no se encontró.";
            }

            return RedirectToAction("Index", "Home"); // Redirige a la página principal u otra vista
        }

        // Otras acciones relacionadas con las reacciones

    }
}


