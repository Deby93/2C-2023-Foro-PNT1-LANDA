
using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Pregunta
    {
        #region Propiedades
        public int PreguntaId { get; set; }

        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int MiembroId { get; set; }

        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int EntradaId { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxDescPregunta, MinimumLength = Restrictions.MinDescPregunta, ErrorMessage = ErrMsgs.StrMaxMin)]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }



        [Display(Name = Alias.FechaDePublicacion)]
        [DataType(DataType.DateTime, ErrorMessage = ErrMsgs.ErrMsgNotValid)]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public Boolean Activa { get; set; } = true;

        public Entrada Entrada { get; set; }

        public Miembro Miembro { get; set; }

        public List<Respuesta> Respuestas { get; set; }

        public int CantidadDeRespuestas
        {
            get
            {
                int resultado = 0;
                if (Respuestas != null)
                {
                    resultado = Respuestas.Count;
                }
                return resultado;
            }
        }
        #endregion
    }
}
