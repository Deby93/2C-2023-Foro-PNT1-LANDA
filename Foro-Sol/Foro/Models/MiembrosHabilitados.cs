using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class MiembrosHabilitados
    {
        [Key]
        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int EntradaId { get; set; }
        [Key]
        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int MiembroId { get; set; }

        public Entrada Entrada { get; set; }

        public Miembro Miembro { get; set; }

        public Boolean Habilitado { get; set; }
#endregion
    }
}
