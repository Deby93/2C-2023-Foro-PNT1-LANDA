using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    [Authorize(Roles = Config.MiembroRolName)]

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

        [HttpPost]
        public async Task<IActionResult> AceptarSolicitud(int entradaId, int miembroId)
        {
            if (entradaId <= 0 || miembroId <= 0)
            {
                return BadRequest("Parámetros inválidos.");
            }

            var miembroHabilitado = await _contexto.MiembrosHabilitados
                .FirstOrDefaultAsync(mh => mh.EntradaId == entradaId && mh.MiembroId == miembroId);

            if (miembroHabilitado == null)
            {
                return NotFound("Solicitud no encontrada.");
            }

            miembroHabilitado.Habilitado = true;
            _contexto.Update(miembroHabilitado);
            await _contexto.SaveChangesAsync();

  
            return RedirectToAction("SolicitudesPendientes", "Entradas");
        }


        [HttpPost]
        public async Task<IActionResult> RechazarSolicitud(int entradaId, int miembroId)
        {
            if (entradaId <= 0 || miembroId <= 0)
            {
                return BadRequest("Parámetros inválidos.");
            }

            var miembroHabilitado = await _contexto.MiembrosHabilitados
                .FirstOrDefaultAsync(mh => mh.EntradaId == entradaId && mh.MiembroId == miembroId);

            if (miembroHabilitado == null)
            {
                return NotFound("Solicitud no encontrada.");
            }

            _contexto.MiembrosHabilitados.Remove(miembroHabilitado);
            await _contexto.SaveChangesAsync();

            return RedirectToAction("SolicitudesPendientes", "Entradas");
        }

    }
}