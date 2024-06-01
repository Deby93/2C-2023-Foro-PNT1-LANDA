using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class CategoriasController : Controller
    {
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;


        public CategoriasController(ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager)
        {
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _contexto.Categorias.OrderBy(c => c.Nombre).ToListAsync());
        }

        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _contexto.Categorias
                .Include(c => c.Entradas) // Incluir las entradas relacionadas con la categoría
                .FirstOrDefaultAsync(cate => cate.CategoriaId == id);

            if (categoria == null)
            {
                return NotFound();
            }

            // Obtener la cantidad de entradas para la categoría actual
            int cantidadEntradas = categoria.Entradas.Count;

            // Pasa la cantidad de entradas a la vista
            ViewBag.CantidadEntradas = cantidadEntradas;

            return View(categoria);
        }


        // GET: Categorias/Create

        public IActionResult Create()
        {
            return View();
        }

      
       // [Authorize(Roles = Config.Miembro)]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {

                var existingCategoria = await _contexto.Categorias.FirstOrDefaultAsync(c => c.Nombre.ToLower() == categoria.Nombre.ToLower() && c.CategoriaId != categoria.CategoriaId);
                if (existingCategoria != null)
                {
                    ModelState.AddModelError("categoria.Nombre", "Ya existe una categoría con este nombre.");
                    return View(categoria);
                }

                _contexto.Categorias.Add(categoria);
                await _contexto.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // GET: Categorias/Edit/5
       // [Authorize(Roles = Config.Miembro)]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _contexto.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

       // [Authorize(Roles = Config.Miembro)]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre")] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    _contexto.Update(categoria);
                    await _contexto.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.CategoriaId))
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
            return View(categoria);
        }

      //  [Authorize(Roles = Config.Miembro)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _contexto.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _contexto.Categorias
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

     //   [Authorize(Roles = Config.Miembro)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_contexto.Categorias == null)
            {
                return Problem("Entity set 'ForoContexto.Categorias'  is null.");
            }
            var categoria = await _contexto.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _contexto.Categorias.Remove(categoria);
            }
            
            await _contexto.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
          return (_contexto.Categorias?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }
    }
}
