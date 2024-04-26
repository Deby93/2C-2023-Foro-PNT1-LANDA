﻿
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Foro
{
    public class CrearAdmin
    {

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
        public string? Password { get; set; } = Config.GenericPass;
        public string? Rol { get; set; } = Config.AdministradorRolName;

       

    }
}