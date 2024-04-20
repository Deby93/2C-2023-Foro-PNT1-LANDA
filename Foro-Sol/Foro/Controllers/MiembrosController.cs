using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foro.Controllers
{
    public class MiembrosController : Controller
    {
        private readonly ForoContexto _foroContexto;

        public MiembrosController(ForoContexto context)
        {
           _foroContexto = context;
        }

        //// [Authorize(Roles = "Admin","Miembro")] administrador o miembro
        ////administrador y miembro
        //[Authorize(Roles = "Miembro")]
        //[Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name))
            {
                User.IsInRole("Miembro");
                Miembro miembroDb = _foroContexto.Miembros.FirstOrDefault(m=>m.NormalizedUserName==User.Identity.Name.ToUpper());
               
                if(miembroDb != null)
                {

                    ViewBag.Miembro = miembroDb.NombreCompleto;

                }

            }
            return View(await _foroContexto.Miembros.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var miembro = await _foroContexto.Miembros.FirstOrDefaultAsync(m => m.Id == id);
            Console.WriteLine(miembro);
            if (miembro == null)
            {
                return NotFound();
            }

            return View(miembro);
        }

       // [Authorize(Roles = Config.MiembroRolName)]
        public IActionResult CheckIn()
        {
            return RedirectToAction("Create", "Preguntas");
        }

        
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult DeleteAll()
        {
            return View();
        }

       [Authorize(Roles = Config.Miembro)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Email, Telefono")] Miembro miembro)
        {
            if (ModelState.IsValid)
            {
                _foroContexto.Add(miembro);
                await _foroContexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            Miembro miembro;
            if (id == null)
            {
                miembro = _foroContexto.Miembros.FirstOrDefault(m => m.NormalizedEmail == User.Identity.Name.ToUpper());
            }
            else
            {
                miembro = await _foroContexto.Miembros.FindAsync(id);
            }
                if (miembro == null)
            {
                return NotFound();
            }
            return View(miembro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Email, Telefono")] Miembro miembro)
        {
            if (id != miembro.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var mbEnDb = _foroContexto.Miembros.Find(miembro.Id);
                    mbEnDb.FechaAlta= miembro.FechaAlta;
                    mbEnDb.Nombre = miembro.Nombre;
                    mbEnDb.Apellido = miembro.Apellido;

                    _foroContexto.Update(mbEnDb);
                    await _foroContexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MiembroExists(miembro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (User.IsInRole("Miembro"))
                {
                    return RedirectToAction(nameof(CheckIn));
                }
                return RedirectToAction(nameof(Index));
            }
            return View(miembro);
        }

        [Authorize(Roles = Config.Miembro)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _foroContexto.Miembros
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }


        [Authorize(Roles = Config.Miembro)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var miembro = await _foroContexto.Miembros.FindAsync(id);
            _foroContexto.Miembros.Remove(miembro);
            await _foroContexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MiembroExists(int id)
        {
            return _foroContexto.Miembros.Any(e => e.Id == id);
        }
    }
}
