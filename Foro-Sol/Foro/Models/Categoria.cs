using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foro
{
    public class Categoria
    {
        #region Propiedades
        public int CategoriaId { get; set; }
        [Required(ErrorMessage = "El nombre de la categoría es requerido.")]
       // [Remote("ValidateCategoriaNombre", "Categoria", HttpMethod = "POST", ErrorMessage = "Ya existe una categoría con este nombre.")]
        public string Nombre { get; set; }

        
        public List<Entrada>? Entradas { get; set; }

       

        #endregion
    }
}
