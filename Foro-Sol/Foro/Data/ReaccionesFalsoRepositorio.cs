namespace Foro.Data
{
    public class ReaccionesFalsoRepositorio
    {
        public static List<Reaccion> GetReacciones()
        {
            Reaccion reaccion1 = new() { RespuestaId = 9, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion2 = new() { RespuestaId = 10, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = false };
            Reaccion reaccion3 = new() { RespuestaId = 12, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion4 = new() { RespuestaId = 14, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = false };
            Reaccion reaccion5 = new() { RespuestaId = 15, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = false };
            Reaccion reaccion6 = new() { RespuestaId = 16, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = false };
            Reaccion reaccion7 = new() { RespuestaId = 17, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = false };
            Reaccion reaccion8 = new() { RespuestaId = 20, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion9 = new() { RespuestaId = 21, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion10 = new() { RespuestaId = 22, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion11 = new() { RespuestaId = 23, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion12 = new() { RespuestaId = 24, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };
            Reaccion reaccion13 = new() { RespuestaId = 25, MiembroId = 6, Fecha = new(2022, 02, 09, 07, 53, 50), MeGusta = true };


            List<Reaccion> reacciones = new()
            {
                reaccion1,
                reaccion2,
                reaccion3,
                reaccion4,
                reaccion5,
                reaccion6,
                reaccion7,
                reaccion8,
                reaccion9,
                reaccion10,
                reaccion11,
                reaccion12,
                reaccion13
            };



            return reacciones;
        }
    }
}


