using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Foro
{
    public static class Startup


    {
        public static WebApplication InicializarApp(string[]args)
        {
            var builder= WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app= builder.Build();
            Configure(app);
            return app;
        }
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            // builder.Services.AddDbContext<ForoContexto>(options => options.UseInMemoryDatabase("ForoDb"));
            builder.Services.AddDbContext<ForoContexto>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ForoDBCS")));

            #region Identity

           builder.Services.AddIdentity<Usuario, Rol>().AddEntityFrameworkStores<ForoContexto>();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
            });

            #endregion

            builder.Services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
               opciones =>
               {
                   opciones.LoginPath = "/Account/IniciarSesion";
                   opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                   opciones.Cookie.Name = "IdentidadForoApp";
               });
            builder.Services.AddControllersWithViews();


        }
        

        private static void Configure(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                {
                    throw new NotImplementedException();
                }
            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
           }
}
