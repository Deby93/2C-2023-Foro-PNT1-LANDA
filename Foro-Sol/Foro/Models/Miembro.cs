using Foro.Helpers;
using System.ComponentModel.DataAnnotations;


namespace Foro.Models
{
    public class Miembro : Usuario
    {
        #region Propiedades
        [Display(Name = Alias.Telefono)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [RegularExpression(@"[0-9]{2}[0-9]{4}[0-9]{4}", ErrorMessage = ErrorMsg.FormatoCelularInvalido)]
        [StringLength(15, MinimumLength = 8, ErrorMessage = ErrMsgs.StrMaxMin)]
        [DataType(DataType.PhoneNumber, ErrorMessage = ErrMsgs.NoValido)]
        public string Telefono { get; set; }

        public List<Reaccion>? PreguntasYRespuestasQueMeGustan { get; set; }

        public List<Entrada>? Entradas { get; set; }
       
        public List<Pregunta>? Preguntas { get; set; }

        public List<Respuesta>? Respuestas { get; set; }

        public List<MiembrosHabilitados>? MiembrosHabilitados { get; set; }

        public Miembro()
        {
            PreguntasYRespuestasQueMeGustan = new List<Reaccion>();
            Entradas = new List<Entrada>();
            Preguntas = new List<Pregunta>();
            Respuestas = new List<Respuesta>();
            MiembrosHabilitados = new List<MiembrosHabilitados>();
        }
    }
    #endregion
}

