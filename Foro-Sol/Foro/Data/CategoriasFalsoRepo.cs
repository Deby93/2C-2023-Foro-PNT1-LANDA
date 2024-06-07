namespace Foro
{
    public class CategoriasFalsoRepositorio
    {
        public static List<Categoria> GetCategorias()
        {
            Categoria categoria1 = new() { Nombre = "Tendencias" };
            Categoria categoria2 = new() { Nombre = "Turismo" };
            Categoria categoria3 = new() { Nombre = "Cine" };
            Categoria categoria4 = new() { Nombre = "Publicidad y Marketing" };
            Categoria categoria5 = new() { Nombre = "Entretenimiento" };
            Categoria categoria6 = new() { Nombre = "Deporte" };
            Categoria categoria7 = new() { Nombre = "Ultimas Noticias" };


            List<Categoria> categorias = new()
            {
                categoria1,
                categoria2,
                categoria3,
                categoria4,
                categoria5,
                categoria6,
                categoria7
            };
            return categorias;
        }
    }
}
