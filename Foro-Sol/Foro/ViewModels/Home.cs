
namespace Foro.ViewModels
{
    public class Home
    {
        public int SesionUserId { get; internal set; }
        public List<Entrada> Top5EntradasConMasPreguntaYRespuestas { get; internal set; }
        public List<Entrada> Top5EntradasMasRecientes { get; internal set; }
        public List<Miembro> Top3MiembrosConMasEntradasUltimoMes { get; internal set; }
    }
}
