using RestaurantesReservas.Web.Service;

namespace RestaurantesReservas.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache(); // Agregar soporte para caché en memoria distribuida
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Configurar el tiempo de espera de la sesión
                options.Cookie.HttpOnly = true; // Configurar la cookie de sesión como HttpOnly
                options.Cookie.IsEssential = true; // Hacer que la cookie de sesión sea esencial
            });

            // Registrar IReservationService
            builder.Services.AddScoped<IReservationService, ReservationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession(); // Agregar el middleware de sesión

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
