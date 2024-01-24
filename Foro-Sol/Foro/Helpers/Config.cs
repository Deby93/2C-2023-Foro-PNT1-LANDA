namespace Foro
{
    public class Config
    {
        public const string LoginPath = "/Account/IniciarSesion";
        public const string AccessDeniedPath = "/Account/AccesoDenegado";
        public const string CoockieName = "ForoCookie";
        public const string Titulo = "Foro Azucar";
        public const string AdminEmail = "admin@ort.edu.ar";
        public const string AdminPass = "Password1!";
        public const string Dominio = "@ort.edu.ar.com";
        public const string MiembroEmail = "miembro@ort.edu.ar";
        public const string GenericPass = "Password1!";
        public const string AuthMiembroOrAdm = "Miembro,Administrador";
        public const string MiembroRolName = "Miembro";
        public const string UsuarioRolName = "Usuario";
        public const string AdminRolName = "Administrador";
        public const string NombreBase = "Miembro";
        public static readonly List<string> RolesParaAdmin = new List<string>() { AdminRolName,MiembroRolName, UsuarioRolName };
        public static readonly List<string> RolesParaMiembro= new List<string>() { MiembroRolName, UsuarioRolName };
    }
}
