using System.ComponentModel.DataAnnotations;


namespace Foro
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

        public List<MiembrosHabilitados>MiembrosHabilitados { get; set; }

        //listas inicializadas
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

