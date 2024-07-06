
using Foro.Helpers;
using Foro.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


namespace Foro.Controllers
{
    [Authorize(Roles = Config.AdministradorRolName)]

    public class UsuariosController : Controller
    {
        private readonly ForoContexto _context;
        private readonly UserManager<Usuario> _userManager;

        public UsuariosController(ForoContexto context, UserManager<Usuario> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }


        public async Task<IActionResult> Index()
        {
            var administradores = _userManager.GetUsersInRoleAsync("ADMINISTRADOR").Result;
            return View(administradores);
        }

        [Authorize(Roles = Config.AdministradorRolName)]

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

        

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


       
        [HttpPost] 
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("id,Nombre,Apellido,FechaAlta,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.UserName = usuario.Nombre;
                usuario.Email = usuario.Nombre +Config.Dominio;
                usuario.FechaAlta = DateTime.Now;
                
                var resultadoCreacionUsuario = await _userManager.CreateAsync(usuario, Config.GenericPass);
                if (resultadoCreacionUsuario.Succeeded)
                {
                    IdentityResult resultadoAddRol;
                    string rolDefinido;

                        rolDefinido = Config.AdministradorRolName;
                   
                   
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

            return View(usuario);
        }

       
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Email, UserName")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Usuario usuarioEnDb = _context.Usuarios.Find(usuario.Id);
                    usuarioEnDb.Nombre = usuario.Nombre;
                    usuarioEnDb.Apellido = usuario.Apellido;
                    usuarioEnDb.Email = usuario.Email;
                    usuarioEnDb.UserName = usuario.UserName;
                    _context.Update(usuarioEnDb);
                    await _context.SaveChangesAsync();
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

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
