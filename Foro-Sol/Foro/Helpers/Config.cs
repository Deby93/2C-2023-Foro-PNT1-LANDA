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
        public const string AuthMiembroOrAdmistrador = "Miembro,Administrador";
        public const string MiembroRolName = "Miembro";
        public const string UsuarioRolName = "Usuario";
        public const string AdministradorRolName = "Administrador";
        public const string NombreBaseMiembro = "Miembro";
        public const string UsuarioEmail = "usuario";
        public static readonly List<string> RolesParaAdministrador = new List<string>() { AdministradorRolName,MiembroRolName, UsuarioRolName };
        public static readonly List<string> RolesParaMiembro= new List<string>() { MiembroRolName, UsuarioRolName };
        public static readonly List<string> RolesParaUsuario = new List<string>() {UsuarioRolName };
    }
}
