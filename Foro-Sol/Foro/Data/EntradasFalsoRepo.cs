namespace Foro.Data
{

    public class EntradasFalsoRepo
    {
        public static List<Entrada> GetEntradas()
        {
            DateTime dateTime1 = new(2024, 07, 02, 06, 52, 14);
            DateTime dateTime2 = new(2024, 07, 01, 07, 51, 16);
            DateTime dateTime3 = new(2024, 06, 28, 12, 50, 50);
            DateTime dateTime4 = new(2024, 07, 02, 18, 18, 18);
            DateTime dateTime5 = new(2024, 06, 17, 22, 59, 30);
            DateTime dateTime6 = new(2024, 07, 08, 14, 12, 44);
            DateTime dateTime7 = new(2021, 07, 07, 18, 50, 33);
            DateTime dateTime8 = new(2022, 03, 28, 03, 50, 50);
            DateTime dateTime9 = new(2022, 01, 30, 02, 50, 50);
            DateTime dateTime10 = new(2022, 02, 12, 06, 52, 14);
            DateTime dateTime11 = new(2021, 02, 05, 07, 51, 16);
            DateTime dateTime12 = new(2021, 11, 15, 12, 50, 50);
            DateTime dateTime13 = new(2021, 02, 01, 06, 51, 10);


            Entrada entrada1 = new() { Titulo = "Maquillaje de dia para ir a trabajar", Fecha = dateTime1, Privada = false, CategoriaId = 1, MiembroId = 6 };
            Entrada entrada2 = new() { Titulo = "Wolfcut,Bob cortes que siguen en el 2024", Fecha = dateTime2, Privada = true, CategoriaId = 1, MiembroId = 6 };
            Entrada entrada3 = new() { Titulo = "Bases recomendables para tu piel", Fecha = dateTime3, Privada = false, CategoriaId = 1, MiembroId = 7 };

            Entrada entrada4 = new() { Titulo = "Lugares para ir a comer en Palermo", Fecha = dateTime4, Privada = false, CategoriaId = 2, MiembroId = 7 };
            Entrada entrada5 = new() { Titulo = "Hoy recorremos Tigre, te contamos los imperdibles", Fecha = dateTime5, Privada = true, CategoriaId = 2, MiembroId = 7 };

            Entrada entrada6 = new() { Titulo = "Boyhood", Fecha = dateTime6, Privada = true, CategoriaId = 3, MiembroId = 8 };
            Entrada entrada7 = new() { Titulo = "Cinema Paradiso", Fecha = dateTime7, Privada = false, CategoriaId = 3, MiembroId = 8 };
            Entrada entrada8 = new() { Titulo = "Top 5 de peliculas en Amazon", Fecha = dateTime8, Privada = true, CategoriaId = 3, MiembroId = 9 };

            Entrada entrada9 = new() { Titulo = "Como llegar a mas publico en Instagram?", Fecha = dateTime9, Privada = false, CategoriaId = 4, MiembroId = 9 };
            Entrada entrada10 = new() { Titulo = "A que publico debemos apuntar y porque", Fecha = dateTime10, Privada = true, CategoriaId = 4, MiembroId = 10 };
            Entrada entrada11 = new() { Titulo = "Generacion alpha y Pandemials que publicidad consume", Fecha = dateTime11, Privada = false, CategoriaId = 4, MiembroId = 10 };

            Entrada entrada12 = new() { Titulo = "Dolar hoy", Fecha = dateTime12, Privada = true, CategoriaId = 5, MiembroId = 11 };
            Entrada entrada13 = new() { Titulo = "Resumen  semanal politico", Fecha = dateTime13, Privada = false, CategoriaId = 5, MiembroId = 11 };



            List<Entrada> entradas = new()
            {
                entrada1,
                entrada2,
                entrada3,
                entrada4,
                entrada5,
                entrada6,
                entrada7,
                entrada8,
                entrada9,
                entrada10,
                entrada11,
                entrada12,
                entrada13
            };


            return entradas;
        }

    }
}
