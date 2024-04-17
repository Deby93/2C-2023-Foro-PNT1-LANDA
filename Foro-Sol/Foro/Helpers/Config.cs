namespace Foro
{
    public class Config
    {
        public const string LoginPath = "/Account/IniciarSesion";
        public const string AccessDeniedPath = "/Account/AccesoDenegado";
        public const string CookieName = "ForoCookie";
        public const string Titulo = "Foro Azucar";
        public const string AdministradorEmail = "administrador";
        public const string Dominio = "@ort.edu.ar";
        public const string MiembroEmail = "miembro";
        public const string GenericPass = "Password1!";
        public const string Administrador = "ADMINISTRADOR";
        public const string Miembro = "MIEMBRO";
        public const string AuthMiembroOrAdmistrador = "MIEMBRO,ADMINISTRADOR";
        public const string MiembroRolName = "Miembro";
        public const string AdministradorRolName = "Administrador";
        public const string NombreBaseMiembro = "Miembro";
        public static readonly List<string> RolesParaAdministrador = new() { AdministradorRolName };
        public static readonly List<string> RolesParaMiembro= new() { MiembroRolName };

        //public static readonly List<string> NombresParaMiembros  = new() { "Juan", "Maria", "Renata", "Felipe", "Facundo", "Gimena", "Gala" };
        //public static readonly List<string> ApellidosParaMiembros = new() { "Jimenez", "Matinez", "Nunez", "Marti", "Hernandez", "Barroso", "Ramirez" }; 
        //public static readonly List<string> TelefonosParaMiembros = new() { "1165789000", "1132665577", "1122334455", "1167889900", "1134567890", "1123458907", "1100987654" };




        public static readonly List<string> NombresParaCategorias = new() { "Tendencia", "Deporte", "Politica", "Moda" };

       
        public static readonly List<string>DescripcionesParaRta = new() { "Me interesa", "Exacto" };
        public static readonly List<string> DescripcionesParaPreg = new() { "Queres saber mas sobre el color Pantone?", "Es importante usar una paleta de colores para disenar?" };

    }
}
