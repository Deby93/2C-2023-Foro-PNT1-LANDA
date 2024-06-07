namespace Foro.Data
{
    public class MiembrosHabilitadosFalsoRepo
    {
        public static List<Miembro> GetMiembrosHabilitados()
        {
            Miembro miem = new() { Nombre = "Richard", Apellido = "Hinostroza", Email = "miembro@miembro.com", FechaAlta = DateTime.Now, Telefono = "1168062288", UserName = "Miembro" };
            Miembro miem1 = new() { Nombre = "Javier", Apellido = "Linares", Email = "miembro1@miembro.com", FechaAlta = DateTime.Now, Telefono = "1125440441", UserName = "Miembro1" };
            Miembro miem2 = new() { Nombre = "Juan", Apellido = "Gutierrez", Email = "miembro2@miembro.com", FechaAlta = DateTime.Now, Telefono = "1152528787", UserName = "Miembro2" };
            Miembro miem3 = new() { Nombre = "Nadia", Apellido = "Sosa", Email = "miembro3@miembro.com", FechaAlta = DateTime.Now, Telefono = "1122225555", UserName = "Miembro3" };
            Miembro miem4 = new() { Nombre = "Juana", Apellido = "Mendoza", Email = "miembro4@miembro.com", FechaAlta = DateTime.Now, Telefono = "1144445555", UserName = "Miembro4" };
            Miembro miem5 = new() { Nombre = "Ricardo", Apellido = "Martinez", Email = "miembro5@miembro.com", FechaAlta = DateTime.Now, Telefono = "1122226666", UserName = "Miembro5" };
            Miembro miem6 = new() { Nombre = "Isabel", Apellido = "Estrada", Email = "miembro6@miembro.com", FechaAlta = DateTime.Now, Telefono = "1121215252", UserName = "Miembro6" };
            Miembro miem7 = new() { Nombre = "Monica", Apellido = "Gamboa", Email = "miembro7@miembro.com", FechaAlta = DateTime.Now, Telefono = "1162623636", UserName = "Miembro7" };

            List<Miembro> miembros = new();
            miembros.Add(miem);
            miembros.Add(miem1);
            miembros.Add(miem2);
            miembros.Add(miem3);
            miembros.Add(miem4);
            miembros.Add(miem5);
            miembros.Add(miem6);
            miembros.Add(miem7);

            return miembros;
        }

    }
}
    

