using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Foro.Models;
using Foro.Helpers;

namespace Foro.ViewModels
{
    
    public class Login
    {
        [Display(Name = Alias.Email)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public string? Email { get; set; }

        [Display(Name = Alias.Contraseña)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

    }
}
