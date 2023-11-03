using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Categoria
    {
        #region
        public int CategoriaId { get; set; }

        [StringLength(Restrictions.MaxNomCat, MinimumLength = Restrictions.MinNomCat, ErrorMessage = ErrMsgs.StrMaxMin)]
        [Display(Name = Alias.NomCat)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [Remote(action: "nombreDeCategoriaDisponible", controller: "Categorias")]
        public string Nombre { get; set; }
        public List<Entrada> Entradas { get; set; }

        #endregion
    }
}
