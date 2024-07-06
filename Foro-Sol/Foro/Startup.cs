using Foro.Controllers;
using Foro.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Foro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            #region Tipo de DB Provider a usar
            try
            {
                _dbInMemory = Configuration.GetValue<bool>("DbInMem");
            }
            catch
            {
                _dbInMemory = true;
            }
            #endregion


        }

        public IConfiguration Configuration { get; }

        public bool _dbInMemory = false;

        public void ConfigureServices(IServiceCollection services)
        {
            #region Tipo de DB provider a usar
            if (_dbInMemory)
            {
                services.AddDbContext<ForoContexto>(options => options.UseInMemoryDatabase("ForoDB-Landa-2A"));
            }
            else
            {
                services.AddDbContext<ForoContexto>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ForoDBCS"))
                );
            }
            #endregion

            services.AddScoped<PreCarga>();

            #region Identity
            services.AddIdentity<Usuario, Rol>().AddEntityFrameworkStores<ForoContexto>();

            services.Configure<IdentityOptions>(
                opciones =>
                {
                    opciones.Password.RequiredLength = 8;
                }
                );
            #endregion

            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme,
                opciones =>
                {
                    opciones.LoginPath = "/Account/IniciarSesion";
                    opciones.AccessDeniedPath = "/Account/AccesoDenegado";
                });

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ForoContexto foroContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var contexto = serviceScope.ServiceProvider.GetRequiredService<ForoContexto>();

                if (!_dbInMemory)
                {
                    foroContext.Database.Migrate();

                }
                serviceScope.ServiceProvider.GetService<PreCarga>().Seed().Wait();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            }
            );
        }
    }
}