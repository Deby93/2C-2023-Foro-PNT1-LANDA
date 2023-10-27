using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Respuesta
    {
        #region Propiedades
        public int RespuestaId { get; set; }

        public int PreguntaId { get; set; }

        public int MiembroId { get; set; }

        public string Descripcion { get; set; }

        public Pregunta Pregunta { get; set; }

        public Miembro Miembro { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public List<Reaccion> Reacciones { get; set; }
        #endregion
    }
}
