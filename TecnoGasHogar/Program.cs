using Microsoft.EntityFrameworkCore;
using TecnoGasHogar.Data;
using TecnoGasHogar.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Aplica las migraciones pendientes y crea la base de datos SQLite si no existe.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Inserta datos de ejemplo si la base de datos está vacía.
    if (!db.SolicitudesServicio.Any())
    {
        var inicio = DateTime.Now;
        var ejemplo = new List<SolicitudServicio>
        {
            new SolicitudServicio
            {
                Cliente = "María Fernanda López",
                Telefono = "987 654 321",
                Distrito = "San Isidro",
                TipoServicio = "Instalación",
                Descripcion = "Instalación de cocina a gas de 4 hornillas. Incluye prueba de llama y fuga."
            },
            new SolicitudServicio
            {
                Cliente = "Jorge Luis Cárdenas",
                Telefono = "912 345 678",
                Distrito = "Miraflores",
                TipoServicio = "Mantenimiento",
                Descripcion = "Mantenimiento preventivo de terma a gas de 10 litros."
            },
            new SolicitudServicio
            {
                Cliente = "Carmen Rosa Quispe",
                Telefono = "998 111 222",
                Distrito = "Surco",
                TipoServicio = "Revisión",
                Descripcion = "Revisión general de cocina a gas por mal funcionamiento de los quemadores."
            },
            new SolicitudServicio
            {
                Cliente = "Luis Alberto Torres",
                Telefono = "955 888 777",
                Distrito = "La Molina",
                TipoServicio = "Fuga",
                Descripcion = "Detecta olor a gas cerca de la cocina. Requiere atención prioritaria."
            },
            new SolicitudServicio
            {
                Cliente = "Ana Lucía Salazar",
                Telefono = "944 333 222",
                Distrito = "San Miguel",
                TipoServicio = "Instalación",
                Descripcion = "Instalación de terma a gas de 12 litros en el departamento."
            },
            new SolicitudServicio
            {
                Cliente = "Pedro Enrique Valdez",
                Telefono = "933 444 555",
                Distrito = "Jesús María",
                TipoServicio = "Mantenimiento",
                Descripcion = "Limpieza y ajuste de válvulas del calentador de agua."
            },
            new SolicitudServicio
            {
                Cliente = "Rosa María Castillo",
                Telefono = "922 555 666",
                Distrito = "Lince",
                TipoServicio = "Revisión",
                Descripcion = "Revisión de seguridad de la instalación de gas del hogar."
            },
            new SolicitudServicio
            {
                Cliente = "Carlos Andrés Ramírez",
                Telefono = "911 222 333",
                Distrito = "Callao",
                TipoServicio = "Instalación",
                Descripcion = "Instalación de horno a gas empotrado y conexión a la red."
            },
            new SolicitudServicio
            {
                Cliente = "Patricia Delgado",
                Telefono = "900 111 000",
                Distrito = "Los Olivos",
                TipoServicio = "Fuga",
                Descripcion = "Posible fuga en la manguera de gas del balón. Solicita revisión urgente."
            },
            new SolicitudServicio
            {
                Cliente = "Diego Fernando Rojas",
                Telefono = "989 777 666",
                Distrito = "Pueblo Libre",
                TipoServicio = "Mantenimiento",
                Descripcion = "Mantenimiento anual de la terma a gas y revisión de la llama."
            },
            new SolicitudServicio
            {
                Cliente = "Verónica Mendoza",
                Telefono = "978 666 555",
                Distrito = "Barranco",
                TipoServicio = "Revisión",
                Descripcion = "Revisión preventiva de la cocina a gas antes de iniciar el servicio."
            },
            new SolicitudServicio
            {
                Cliente = "Marco Antonio Paredes",
                Telefono = "967 555 444",
                Distrito = "Surquillo",
                TipoServicio = "Instalación",
                Descripcion = "Instalación de cocina a gas de 3 hornillas en vivienda familiar."
            }
        };

        for (int i = 0; i < ejemplo.Count; i++)
        {
            ejemplo[i].FechaRegistro = inicio.AddMinutes(-15 * i);
        }

        db.SolicitudesServicio.AddRange(ejemplo);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseHttpsRedirection();
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SolicitudServicio}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
