namespace Foro.Models
{
    public class Miembro
    {
        public string Telefono { get; set; }

        public List<Reaccion> PreguntasYRespuestasQueMeGustan { get; set; }

        public List<MiembrosHabilitados> Entradas { get; set; }

        public List<Pregunta> Preguntas { get; set; }

        public List<Respuesta> Respuestas { get; set; }

    }
}
