using Humanizer;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;



namespace Foro
{
    public class Usuario : IdentityUser<int>
    {
        #region Propiedades
  
    //    public int Id { get; set; }

        [Display(Name = Alias.Nombre)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxNom, MinimumLength = Restrictions.MinNom, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string? Nombre { get; set; }

        [Display(Name = Alias.Apellido)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxApe, MinimumLength = Restrictions.MinApe, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string? Apellido { get; set; }

        [Display(Name = Alias.FechaAlta)]
        [DataType(DataType.DateTime, ErrorMessage = ErrMsgs.ErrMsgNotValid)]
        public DateTime? FechaAlta { get; set; } =  DateTime.Now;

        [Display(Name = Alias.NombreCompleto)]
        public string NombreCompleto
        {
            get
            {
                if ((Nombre != null && Nombre.Length > 0) && (Apellido != null && Apellido.Length > 0))
                {
                    return $"{Nombre} {Apellido}";
                }
                else if(Nombre !=null && Nombre.Length >0)
                {
                    return $"{Nombre}";
                }
                return "error";
            }
        }

        #endregion
    }
}
