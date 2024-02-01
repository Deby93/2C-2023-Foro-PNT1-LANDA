using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foro
{
    
    public class Login
    {
        [Display(Name = Alias.Email)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public string Email { get; set; }

        [Display(Name = Alias.Contraseña)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        //[MinLength(9, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string Password { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }

    }
}
