using Foro.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Reaccion
    {
        #region Propiedades

        [Key]
        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int RespuestaId { get; set; }

        [Key]
        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int MiembroId { get; set; }

        public Respuesta Respuesta { get; set; }

        public Miembro Miembro { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrorMsg.ErrMsgNotValid)]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public Boolean MeGusta { get; set; }
        #endregion
    }
}

