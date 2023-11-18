using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Foro
{
    public class Usuario
    {
        #region Propiedades
  
        public int id { get; set; }

        [Display(Name = Alias.Nombre)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxNom, MinimumLength = Restrictions.MinNom, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string Nombre { get; set; }

        [Display(Name = Alias.Apellido)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxApe, MinimumLength = Restrictions.MinApe, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string Apellido { get; set; }

        [Display(Name = Alias.FechaAlta)]
        [DataType(DataType.DateTime, ErrorMessage = ErrMsgs.ErrMsgNotValid)]
        public DateTime? FechaAlta { get; set; } = null;

        [Display(Name = Alias.NombreCompleto)]
        public string NombreCompleto
        {
            get
            {
                return $"{Apellido.ToUpper()}, {Nombre}";
            }
        }

        [Display(Name = "Correo electronico")]
        [DataType(DataType.EmailAddress, ErrorMessage = ErrMsgs.ErrMsgNotValid)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password, ErrorMessage = ErrMsgs.ErrMsgNotValid)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public string Password { get; set; }
        #endregion
    }
}
