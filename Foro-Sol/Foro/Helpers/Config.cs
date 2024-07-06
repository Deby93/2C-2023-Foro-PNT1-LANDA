namespace Foro.Helpers
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
        public const string MiembroRolName = "MIEMBRO";
        public const string AdministradorRolName = "ADMINISTRADOR";
        public const string NombreBaseMiembro = "MIEMBRO";
        public static readonly List<string> RolesParaAdministrador = new() { AdministradorRolName };
        public static readonly List<string> RolesParaMiembro= new() { MiembroRolName };
        public static readonly int UMBRAL_DISLIKES = 5;
    }
}
