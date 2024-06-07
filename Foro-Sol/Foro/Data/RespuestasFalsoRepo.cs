namespace Foro.Data
{
    public class RespuestasFalsoRepo
    {
        public static List<Respuesta> GetRespuestas()
        {
            //Pregunta pregunta1 = new() { MiembroId = 7, EntradaId = 12, Descripcion = "¿Va a seguir aumentando el dolar en marzo?", Fecha = new(2022, 02, 08, 06, 01, 50), Activa = true };
            //Pregunta pregunta3 = new() { MiembroId = 7, EntradaId = 9, Descripcion = "¿Es verdad que los reels dejaron de generar engagement?", Fecha = new(2022, 02, 08, 06, 03, 50), Activa = false };
            //Pregunta pregunta4 = new() { MiembroId = 7, EntradaId = 5, Descripcion = "¿Podrias decir las direcciones exactas?", Fecha = new(2022, 02, 08, 06, 04, 50), Activa = true };
            //Pregunta pregunta5 = new() { MiembroId = 6, EntradaId = 5, Descripcion = "¿Van a hacer parte 2?", Fecha = new(2022, 02, 08, 06, 05, 50), Activa = false };
            //Pregunta pregunta6 = new() { MiembroId = 6, EntradaId = 13, Descripcion = "¿Que es lo que paso en la ultima semana?", Fecha = new(2022, 02, 08, 06, 06, 50), Activa = true };
            //Pregunta pregunta7 = new() { MiembroId = 7, EntradaId = 7, Descripcion = "¿Van hacer la remake?", Fecha = new(2022, 02, 08, 06, 07, 50), Activa = true };
            //Pregunta pregunta8 = new() { MiembroId = 7, EntradaId = 12, Descripcion = "¿Cuanta diferencia hay entre dolar oficial y el mep?", Fecha = new(2022, 02, 08, 06, 08, 50), Activa = false };
            //Pregunta pregunta9 = new() { MiembroId = 7, EntradaId = 7, Descripcion = "¿Como se llama la cancion de la peli?", Fecha = new(2022, 02, 08, 06, 09, 50), Activa = true };
            //Pregunta pregunta10 = new() { MiembroId = 7, EntradaId = 1, Descripcion = "¿Como subir el maquillaje dia para que sea de noche?", Fecha = new(2022, 02, 08, 06, 10, 50), Activa = false };
            //Pregunta pregunta2 = new() { MiembroId = 7, EntradaId = 6, Descripcion = "¿Cuantos años tardaron para hacer la pelicula ?", Fecha = new(2022, 02, 08, 06, 02, 50), Activa = true };


            Respuesta respuesta1 = new() { PreguntaId = 10, MiembroId = 6, Descripcion = "Con un labial oscuro y delineado mas marcado", Fecha = new(2022, 02, 09, 06, 13, 50) };
            Respuesta respuesta2 = new() { PreguntaId = 8, MiembroId = 6, Descripcion = "20 pesos", Fecha = new(2022, 02, 09, 06, 14, 50) };
            Respuesta respuesta3 = new() { PreguntaId = 1, MiembroId = 6, Descripcion = "Si!", Fecha = new(2022, 02, 09, 06, 15, 50) };
            Respuesta respuesta4 = new() { PreguntaId = 2, MiembroId = 6, Descripcion = "Tardo 12 años para que creciera el personaje de Mason (Ellar Coltrane)", Fecha = new(2022, 02, 09, 06, 16, 50) };
            Respuesta respuesta5 = new() { PreguntaId = 3, MiembroId = 6, Descripcion = "Si, porque la gente pierde el interes en los primeros 5 segundos", Fecha = new(2022, 02, 09, 06, 17, 50) };
            Respuesta respuesta6 = new() { PreguntaId = 9, MiembroId = 6, Descripcion = " La música y banda sonora original de la película es de Ennio Morricone y su hijo Andrea Morricone, quien colaboró en su composición.", Fecha = new(2022, 02, 09, 06, 18, 50) };
            Respuesta respuesta7 = new() { PreguntaId = 7, MiembroId = 6, Descripcion = "No, por el momento", Fecha = new(2022, 02, 09, 06, 19, 50) };
            Respuesta respuesta8 = new() { PreguntaId = 4, MiembroId = 7, Descripcion = "Te la paso por mail", Fecha = new(2022, 02, 09, 06, 20, 50) };
            Respuesta respuesta9 = new() { PreguntaId = 5, MiembroId = 7, Descripcion = "Si, estamos buscando lugares nuevos en otro barrio pronto lo comunicaremos", Fecha = new(2022, 02, 09, 06, 21, 50) };


            List<Respuesta> respuestas = new()
            {
                respuesta1,
                respuesta2,
                respuesta3,
                respuesta4,
                respuesta5,
                respuesta6,
                respuesta7,
                respuesta8,
                respuesta9,
            };

            return respuestas;
        }
    }
}
    

