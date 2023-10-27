﻿using System.ComponentModel.DataAnnotations;

namespace Foro.Models
{
    public class MiembrosHabilitados
    {
        public int EntradaId { get; set; }
        public int MiembroId { get; set; }

        public Entrada Entrada { get; set; }

        public Miembro Miembro { get; set; }

        public Boolean Habilitado { get; set; }
#endregion
    }
}