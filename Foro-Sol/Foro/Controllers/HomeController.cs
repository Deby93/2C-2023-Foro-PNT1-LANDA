using Foro.Models;
using Foro.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Foro.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ForoContexto _contexto;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signinManager;

        public HomeController(ILogger<HomeController> logger, ForoContexto context, UserManager<Usuario> userManager, SignInManager<Usuario> signinManager) { 
            _logger = logger;
            _contexto = context;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public IActionResult Index(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            return View();
        }
        public async Task<IActionResult> Tops()
        {
            Home home = new();

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                home.SesionUserId = Int32.Parse(claim.Value);
            }
            else
            {
                home.SesionUserId = 0;
            }
            var entradasTemp = _contexto.Entradas
                            .Include(p => p.Miembro)
                            .Include(p => p.MiembrosHabilitados)
                            .Include(p => p.Preguntas)
                            .ThenInclude(p => p.Respuestas)
                            .ToList();

            home.Top5EntradasConMasPreguntaYRespuestas = entradasTemp.OrderByDescending(e => e.CantidadDePreguntasYRespuestas).Take(5).ToList();
            home.Top5EntradasMasRecientes = await _contexto.Entradas.Include(p => p.Miembro).Include(p => p.MiembrosHabilitados).OrderByDescending(p => p.Fecha).Take(5).ToListAsync();

            var entradasUltimoMesAux = _contexto.Entradas.Where(p => p.Fecha > DateTime.Today.AddDays(-30));

            if (entradasUltimoMesAux.Any())
            {
                var top3 = ObtenerTop3(entradasUltimoMesAux);
                home.Top3MiembrosConMasEntradasUltimoMes = top3;
            }
            return View(home);
        }
        private List<Miembro> ObtenerTop3(IQueryable<Entrada> entradasUltimoMesAux)
        {
            int maxMiembroId = entradasUltimoMesAux.Max(e => e.MiembroId);

            int[] cantidadDeEntradasPorMiembro = new int[maxMiembroId];


           
            foreach (Entrada e in entradasUltimoMesAux)
            {
                cantidadDeEntradasPorMiembro[e.MiembroId - 1]++;
            }
            return Top3(cantidadDeEntradasPorMiembro);
        }

        private List<Miembro> Top3(int[] cantidadDeEntradasPorMiembro)
        {
            List<Miembro> top3 = new();
            int maxCantEntradas = cantidadDeEntradasPorMiembro.Max();
            int i = 0;
            while (i < cantidadDeEntradasPorMiembro.Length && top3.Count < 3)
            {
                if (maxCantEntradas > 0 && maxCantEntradas == cantidadDeEntradasPorMiembro[i])
                {
                    Miembro unMiembro = _contexto.Miembros.First(p => p.Id - 1 == i);
                    top3.Add(unMiembro);
                    cantidadDeEntradasPorMiembro[i] = 0;
                    maxCantEntradas = cantidadDeEntradasPorMiembro.Max();
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            return top3;
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}