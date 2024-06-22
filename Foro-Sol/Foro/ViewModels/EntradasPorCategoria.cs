
using System.ComponentModel.DataAnnotations;

namespace Foro
{
    public class EntradasPorCategoria
    {
        public int CategoriaId { get; set; }

        public List<Categoria> Categorias { get; set; }

        public int UsuarioLogueadoId { get; set; }

        public List<Miembro> Miembros { get; set; }

        public List<Entrada> Entradas { get; set; }

    }
}