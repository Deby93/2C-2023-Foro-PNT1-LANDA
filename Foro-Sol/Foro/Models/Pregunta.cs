
namespace Foro.Models
{
    public class Pregunta
    {
        #region Propiedades
        public int PreguntaId { get; set; }

        public int MiembroId { get; set; }

        public int EntradaId { get; set; }

        public string Descripcion { get; set; }

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
