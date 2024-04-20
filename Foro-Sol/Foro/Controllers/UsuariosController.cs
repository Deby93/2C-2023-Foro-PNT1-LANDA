using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace Foro
{
    public class UsuariosController : Controller
    {
        private readonly ForoContexto _context;
        private readonly UserManager<Usuario> _userManager;
        //inyectar el usermanager

        public UsuariosController(ForoContexto context, UserManager<Usuario> usermanager)
        {
            _context = context;
            _userManager = usermanager;//inyectado
        }


        [AllowAnonymous] // Permite que los usuarios no autenticados puedan ver la lista de usuarios.
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return _context.Usuarios != null ?
                        View(await _context.Usuarios.ToListAsync()) :
                        Problem("Entity set 'ForoContexto.Usuarios'  is null.");
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }


        // GET: Usuarios/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //resuelve ambiguedad en el metodo create que es sobrecargado
        [ValidateAntiForgeryToken]
        //Bind indicar que atributos  necesito

        [Authorize(Roles = Config.Administrador)] // Requiere que el usuario esté autenticado y tenga el rol de "Administrador" para crear usuarios.
        public async Task<IActionResult> Create([Bind("id,Nombre,Apellido,FechaAltail,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.UserName = usuario.Nombre;
                usuario.Email = usuario.Nombre +Config.Dominio;
                usuario.FechaAlta = DateTime.Now;
                var resultadoCreacionUsuario = await _userManager.CreateAsync(usuario, Config.GenericPass);
                // se pone Usuarios para que la variable sea del mismo tipo la que viene x parametro
                if (resultadoCreacionUsuario.Succeeded)
                {
                    IdentityResult resultadoAddRol;
                    string rolDefinido;

                        rolDefinido = Config.Administrador;
                   
                   
                    resultadoAddRol = await _userManager.AddToRoleAsync(usuario, rolDefinido);

                    if (resultadoAddRol.Succeeded)
                    {
                        return RedirectToAction("Index", "Usuarios");
                    }
                    else
                    {
                        return Content($"No se ha podido agregar el rol {rolDefinido}");

                    }
                }


            }
            //_context.Usuarios.Add(usuario);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            //redireccion al index 302

            return View(usuario);
        }

        [Authorize(Roles =Config.Administrador)]
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [Authorize(Roles = Config.Administrador)]
        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Nombre,Apellido,FechaAlta,Email,Password")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioDB = await _context.Usuarios.FindAsync(id);
                    if (usuarioDB !=null)
                    {
                        //Actualizamos
                       
                        usuarioDB.Nombre =usuario.Nombre;
                        usuarioDB.Apellido =usuario.Apellido;
                        usuarioDB.Email =usuario.Email;
                        usuarioDB.FechaAlta =usuario.FechaAlta;
                        //usuarioDB.Password =usuario.Password;

                        _context.Usuarios.Update(usuarioDB);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        return NotFound();
                    }
                   
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [Authorize(Roles = Config.Miembro)]
        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'ForoContexto.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null )
            {
                _context.Usuarios.Remove(usuario);
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
