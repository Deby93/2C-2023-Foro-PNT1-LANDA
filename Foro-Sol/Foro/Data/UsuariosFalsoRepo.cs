namespace Foro.Data
{
    public class UsuariosFalsoRepo
    {
        public static List<Usuario> GetUsuarios()
        {
            List<Usuario> usuarios = new();
            Usuario usuario = new() { Nombre = "Natalia", Apellido = "Martinez", Email = "natalia@ort.edu.ar", FechaAlta = DateTime.Now, UserName = "admin" };
            Usuario usuario1 = new() { Nombre = "Mariana", Apellido = "Perez", Email = "mariana@ort.edu.ar", FechaAlta = DateTime.Now, UserName = "admin1" };
            Usuario usuario2 = new() { Nombre = "Martin", Apellido = "Gomez", Email = "martin@ort.edu.ar", FechaAlta = DateTime.Now, UserName = "admin2" };
            Usuario usuario3 = new() { Nombre = "Federico", Apellido = "Suarez", Email = "federico@admin@ort.edu.ar", FechaAlta = DateTime.Now, UserName = "admin3" };
            Usuario usuario4 = new() { Nombre = "Juliana", Apellido = "Figueroa", Email = "juliana@ort.edu.ar", FechaAlta = DateTime.Now, UserName = "admin4" };

            usuarios.Add(usuario);
            usuarios.Add(usuario1);
            usuarios.Add(usuario2);
            usuarios.Add(usuario3);
            usuarios.Add(usuario4);

            return usuarios;
        }
    }
}

