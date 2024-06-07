namespace Foro
{
    namespace Foro
    {
        public class PreguntasFalsoRepo
        {
            public static List<Pregunta> GetPreguntas()
            {
                //Entrada entrada1 = new() { Titulo = "Maquillaje de dia para ir a trabajar", Fecha = dateTime1, Privada = false, CategoriaId = 1, MiembroId = 6 };
                //Entrada entrada2 = new() { Titulo = "Wolfcut,Bob cortes que siguen en el 2024", Fecha = dateTime2, Privada = true, CategoriaId = 1, MiembroId = 6 };
                //Entrada entrada3 = new() { Titulo = "Bases recomendables para tu piel", Fecha = dateTime3, Privada = false, CategoriaId = 1, MiembroId = 7 };

                //Entrada entrada4 = new() { Titulo = "Lugares para ir a comer en Palermo", Fecha = dateTime4, Privada = false, CategoriaId = 2, MiembroId = 7 };
                //Entrada entrada5 = new() { Titulo = "Hoy recorremos Tigre, te contamos los imperdibles", Fecha = dateTime5, Privada = true, CategoriaId = 2, MiembroId = 7 };

                //Entrada entrada6 = new() { Titulo = "Boyhood", Fecha = dateTime6, Privada = true, CategoriaId = 3, MiembroId = 8 };
                //Entrada entrada7 = new() { Titulo = "Cinema Paradiso", Fecha = dateTime7, Privada = false, CategoriaId = 3, MiembroId = 8 };
                //Entrada entrada8 = new() { Titulo = "Top 5 de peliculas en Amazon", Fecha = dateTime8, Privada = true, CategoriaId = 3, MiembroId = 9 };

                //Entrada entrada9 = new() { Titulo = "Como llegar a mas publico en Instagram?", Fecha = dateTime9, Privada = false, CategoriaId = 4, MiembroId = 9 };
                //Entrada entrada10 = new() { Titulo = "A que publico debemos apuntar y porque", Fecha = dateTime10, Privada = true, CategoriaId = 4, MiembroId = 10 };
                //Entrada entrada11 = new() { Titulo = "Generacion alpha y Pandemials que publicidad consume", Fecha = dateTime11, Privada = false, CategoriaId = 4, MiembroId = 10 };

                //Entrada entrada12 = new() { Titulo = "Dolar hoy", Fecha = dateTime12, Privada = true, CategoriaId = 5, MiembroId = 11 };
                //Entrada entrada13 = new() { Titulo = "Resumen  semanal politico", Fecha = dateTime13, Privada = false, CategoriaId = 5, MiembroId = 11 };


                Pregunta pregunta1 = new() { MiembroId = 6, EntradaId = 12, Descripcion = "¿Va a seguir aumentando el dolar en marzo?", Fecha = new(2022, 02, 08, 06, 01, 50), Activa = true };
                Pregunta pregunta2 = new() { MiembroId = 6, EntradaId = 6, Descripcion = "¿Cuantos años tardaron para hacer la pelicula ?", Fecha = new(2022, 02, 08, 06, 02, 50), Activa = true };
                Pregunta pregunta3 = new() { MiembroId = 6, EntradaId = 9, Descripcion = "¿Es verdad que los reels dejaron de generar engagement?", Fecha = new(2022, 02, 08, 06, 03, 50), Activa = false };
                Pregunta pregunta4 = new() { MiembroId = 6, EntradaId = 5, Descripcion = "¿Podrias decir las direcciones exactas?", Fecha = new(2022, 02, 08, 06, 04, 50), Activa = true };
                Pregunta pregunta5 = new() { MiembroId = 6, EntradaId = 5, Descripcion = "¿Van a hacer parte 2?", Fecha = new(2022, 02, 08, 06, 05, 50), Activa = false };
                Pregunta pregunta6 = new() { MiembroId = 7, EntradaId = 13, Descripcion = "¿Que es lo que paso en la ultima semana?", Fecha = new(2022, 02, 08, 06, 06, 50), Activa = true };
                Pregunta pregunta7 = new() { MiembroId = 7, EntradaId = 7, Descripcion = "¿Van hacer la remake?", Fecha = new(2022, 02, 08, 06, 07, 50), Activa = true };
                Pregunta pregunta8 = new() { MiembroId = 7, EntradaId = 12, Descripcion = "¿Cuanta diferencia hay entre dolar oficial y el mep?", Fecha = new(2022, 02, 08, 06, 08, 50), Activa = false };
                Pregunta pregunta9 = new() { MiembroId = 7, EntradaId = 7, Descripcion = "¿Quién compuso la música de la película Cinema Paradiso?", Fecha = new(2022, 02, 08, 06, 09, 50), Activa = true };
                Pregunta pregunta10 = new() { MiembroId = 7, EntradaId = 1, Descripcion = "¿Como subir el maquillaje dia para que sea de noche?", Fecha = new(2022, 02, 08, 06, 10, 50), Activa = false };


                ;

                List<Pregunta> preguntas = new()
            {
                pregunta1,
                pregunta2,
                pregunta3,
                pregunta4,
                pregunta5,
                pregunta6,
                pregunta7,
                pregunta8,
                pregunta9,
                pregunta10,


            };
                return preguntas;
            }
        }
    }

}
