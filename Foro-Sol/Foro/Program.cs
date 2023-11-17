namespace Foro
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var app= Startup.InicializarApp(args); 

            app.Run();
        }
    }
}