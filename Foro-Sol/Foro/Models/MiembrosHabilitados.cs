using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foro
{
    public class MiembrosHabilitados
    {
        #region Propiedades
        [Key]
        public int MiembroId { get; set; }

        [Required]
        [Key]

        public int EntradaId { get; set; }
            public bool Habilitado { get; set; }

            public Miembro ?Miembro { get; set; }
            public Entrada ?Entrada { get; set; }
        }

    }
    #endregion

