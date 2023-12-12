﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Foro
{
    
    public class Login
    {
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public string Username { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [DataType(DataType.Password)]
        //[MinLength(9, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
