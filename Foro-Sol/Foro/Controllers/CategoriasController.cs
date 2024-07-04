using Foro.Data;
using Foro.Helpers;
using Foro.Models;
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
            else { 
            Categoria unaCategoria= _contexto.Categorias.FirstOrDefault(c => c.CategoriaId == id);
            }
            var categoria = await _contexto.Categorias
                .Include(c => c.Entradas) // Incluir las entradas relacionadas con la categoría
                .FirstOrDefaultAsync(cate => cate.CategoriaId == id);
          

            List<Entrada> listaDeEntradas = new();
            listaDeEntradas = await _contexto.Entradas
              .Include(e => e.Categoria)
           .Include(e => e.Miembro)
              .Include(e => e.Preguntas)
              .OrderBy(c => c.Fecha)
              .Where(m => m.CategoriaId == id).ToListAsync();

            if (categoria == null && listaDeEntradas==null)
            {
                return NotFound();
            }

            var listaDeCategorias = await _contexto.Categorias.
                 OrderBy(c => c.Nombre).
                 ToListAsync();
            int cantidadEntradas = categoria.Entradas.Count;

            ViewBag.unaCategoria = categoria;           
            ViewBag.CantidadEntradas = cantidadEntradas;
            ViewBag.Categorias = listaDeCategorias;
            if (User.IsInRole(Config.MiembroRolName) && User.Claims.Any())
            {
                int UsuarioId = 0;
                UsuarioId = Int32.Parse(User.Claims.First().Value);
            }
            return View(listaDeEntradas);
        }

        [Authorize(Roles = Config.AdministradorRolName + "," + Config.MiembroRolName)]
        // GET: Categorias/Create

        public IActionResult Create()
        {
            return View();
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Config.AdministradorRolName + "," + Config.MiembroRolName)]
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
        [Authorize(Roles = Config.AdministradorRolName + "," + Config.MiembroRolName)]

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

    

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Config.AdministradorRolName + "," + Config.MiembroRolName)]
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

      

        private bool CategoriaExists(int id)
        {
          return (_contexto.Categorias?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }

    }
}
