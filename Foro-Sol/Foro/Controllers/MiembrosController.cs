using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Foro.Controllers
{
   [Authorize(Roles = Config.MiembroRolName)]
    public class MiembrosController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;


        public MiembrosController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;
        }
        // GET: Miembros
        public async Task<IActionResult> Index()
        {

            var listaOrdenadaPorFecha = await _contexto.Miembros.OrderByDescending(p => p.FechaAlta).ToListAsync();

            return View(listaOrdenadaPorFecha);
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _contexto.Miembros.FirstOrDefaultAsync(m => m.Id == id);
            if (miembro == null)
            {
                return NotFound();
            }

            int cantPreguntas = await _contexto.Preguntas.CountAsync(p => p.MiembroId == id);
            int cantRespuestas = await _contexto.Respuestas.CountAsync(r => r.MiembroId == id);
            int cantReacciones = await _contexto.Reacciones.CountAsync(reac => reac.MiembroId == id);

            ViewBag.cantPreguntas = cantPreguntas;
            ViewBag.cantRespuestas = cantRespuestas;
            ViewBag.cantReacciones = cantReacciones;
            return View(miembro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _contexto.Miembros.FindAsync(id);
            if (miembro == null || id != Int32.Parse(User.Claims.First().Value))
            {
                return NotFound();
            }

            return View(miembro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Telefono,Id,Nombre,Apellido,Email")] Miembro miembro)
        {
            var MiembroIdEncontrado = Int32.Parse(_userManager.GetUserId(User));


            if (id != MiembroIdEncontrado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var miembroEnDb = await _contexto.Miembros.FindAsync(id);
                    if (miembroEnDb == null)
                    {
                        return NotFound();
                    }

                    miembroEnDb.Telefono = miembro.Telefono;
                   

                    _contexto.Miembros.Update(miembroEnDb);
                    await _contexto.SaveChangesAsync();

                    return RedirectToAction("Index", "Miembros");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(MiembroIdEncontrado))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(miembro);
        }
        
        private bool MiembroExists(int id)
        {
            return _contexto.Miembros.Any(e => e.Id == id);
        }
    }
}