using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Reaccion
    {
        #region Propiedades

        public int RespuestaId { get; set; }

        public int MiembroId { get; set; }

        public Respuesta Respuesta { get; set; }

        public Miembro Miembro { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public Boolean MeGusta { get; set; }
        #endregion
    }
}

