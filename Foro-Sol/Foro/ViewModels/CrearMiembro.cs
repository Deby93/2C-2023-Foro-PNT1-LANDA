
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Foro
{
    public class CrearMiembro
    {
        [Display(Name = Alias.Telefono)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[0-9]{2}[0-9]{4}[0-9]{4}", ErrorMessage = ErrorMsg.FormatoCelularInvalido)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = ErrMsgs.StrMaxMin)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrMsgs.NoValido)]
        public string? Telefono { get; set; }

        [Display(Name = Alias.Nombre)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxNom, MinimumLength = Restrictions.MinNom, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string? Nombre { get; set; }

        [Display(Name = Alias.Apellido)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxApe, MinimumLength = Restrictions.MinApe, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxNomUser, MinimumLength = Restrictions.MinNomUser, ErrorMessage = ErrMsgs.StrMaxMin)]
        [Remote(action: "UsuarioDisponible", controller: "Account")]
        [Display(Name = Alias.UserName)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [EmailAddress(ErrorMessage = ErrMsgs.NoValido)]
        [Remote(action: "EmailDisponible", controller: "Account")]
        [Display(Name = "Correo Electronico")]
        public string? Email { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña no coincide")]
        public string? ConfirmPassword { get; set; }


     

    }
}