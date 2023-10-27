using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class Entrada
    {
        public int EntradaId { get; set; }

        public string Titulo { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;

        public int CategoriaId { get; set; }

        public int MiembroId { get; set; }

       
        public Boolean Privada { get; set; } = false;

        public Miembro Miembro { get; set; }

        public Categoria Categoria { get; set; }
        public List<MiembrosHabilitados> MiembrosHabilitados { get; set; }

        public List<Pregunta> Preguntas { get; set; }

        public int CantidadDePreguntasYRespuestas
        {
            get
            {
                int resultado = 0;
                if (Preguntas != null)
                {
                    resultado = Preguntas.Count();
                    foreach (Pregunta preg in Preguntas)
                    {
                        resultado += preg.CantidadDeRespuestas;
                    }
                }
                return resultado;
            }
        }
}
