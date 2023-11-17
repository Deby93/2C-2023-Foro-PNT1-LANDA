using Foro.Helpers;
using FORO_D.Models;
using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Miembro : Usuario
    {
        #region Propiedades
        [Display(Name = Alias.Telefono)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = ErrMsgs.StrMaxMin)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrMsgs.NoValido)]
        [RegularExpression(@"[0-9]{2}[0-9]{4}[0-9]{4}", ErrorMessage = ErrorMsg.FormatoCelularInvalido)]
        public string Telefono { get; set; }

        public List<Reaccion> PreguntasYRespuestasQueMeGustan { get; set; }

        public List<Entrada> Entradas { get; set; }

        public List<Pregunta> Preguntas { get; set; }

        public List<Respuesta> Respuestas { get; set; }
    }
    #endregion
}

