using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foro.Helpers
{
    public class Restrictions
    {
        public const int MinNom = 2;
        public const int MaxNom = 15;
        public const int MinNomUser = 3;
        public const int MaxNomUser = 18;
        public const int MinApe = 3;
        public const int MaxApe = 15;
        public const int MinPassword = 8;
        public const int MaxPassword = 20;

        public const int MinNomCat = 3;
        public const int MaxNomCat = 20;

        public const int MinTituloEntrada = 4;
        public const int MaxTituloEntrada = 50;
        public const int MinDescEntrada = 4;
        public const int MaxDescEntrada = 500;

        public const int MinDescPregunta = 4;
        public const int MaxDescPregunta = 100;

        public const int MinDescRespuesta = 4;
        public const int MaxDescRespuesta = 100;
    }
}
