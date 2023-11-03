using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foro.Helpers
{
    public static class ErrorMsg
    {

        public const string ErrMsgStrLen = "El campo {0} deberia tener como minimo {2} y  como maximo {1} caracteres.";
        public const string ErrMsgNotValid = "El ingreso en el campo {0} no es valido.";
        public const string ErrMsgRequired = "Este campo es requerido.";
        public const string ErrMsgNotNumeric = "El valor ingresado en el campo {0} debe ser númerico.";
        public const string FormatoCelularInvalido = "El numero de celular debe tener un formato 11-1111-1111.";
    }
}
