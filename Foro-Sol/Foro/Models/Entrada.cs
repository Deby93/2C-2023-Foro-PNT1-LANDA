﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;




namespace Foro
{
    public class Entrada
    {
        #region Propiedades
        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int Id { get; set; }

        [Display(Name = Alias.Titulo)]
        [Required(ErrorMessage = ErrMsgs.Requerido)]
        [StringLength(Restrictions.MaxTituloEntrada, MinimumLength = Restrictions.MinTituloEntrada, ErrorMessage = ErrMsgs.StrMaxMin)]
        public string Titulo { get; set; }

        [Display(Name = Alias.FechaDePublicacion)]
        [DataType(DataType.DateTime, ErrorMessage = ErrorMsg.ErrMsgNotValid)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = ErrorMsg.ErrMsgRequired)]
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
    #endregion
}

