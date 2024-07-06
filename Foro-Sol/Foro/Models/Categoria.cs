using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Foro.Models;

namespace Foro.Models
{
    public class Categoria
    {
        #region Propiedades
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "El nombre de la categoría es requerido.")]
        public string Nombre { get; set; }

        
        public List<Entrada>? Entradas { get; set; }

       

        #endregion
    }
}
