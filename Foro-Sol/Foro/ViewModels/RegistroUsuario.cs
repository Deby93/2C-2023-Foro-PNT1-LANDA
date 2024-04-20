
using System.ComponentModel.DataAnnotations;

namespace Foro
{
    public class RegistroUsuario
    {
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrMsgs.NoValido)]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña no coincide")]
        public string ConfirmPassword { get; set; }

        public string Dominio { get; set; }
        public string Rol { get; set; }

        public RegistroUsuario()
        {
            Rol = Config.Miembro;
        }


    }
}