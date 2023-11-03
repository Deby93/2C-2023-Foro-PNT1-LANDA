using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Respuesta
    {
        #region Propiedades
        [Required(ErrorMessage = ErrMsgs.Requerido)]

        public int RespuestaId { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]

        public int PreguntaId { get; set; }
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        public int MiembroId { get; set; }

        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxDescRespuesta, MinimumLength = Restrictions.MinDescRespuesta, ErrorMessage = ErrMsgs.StrMaxMin)]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        public Pregunta Pregunta { get; set; }

        public Miembro Miembro { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrMsgs.ErrMsgNotValid)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public List<Reaccion> Reacciones { get; set; }
        #endregion
    }
}
