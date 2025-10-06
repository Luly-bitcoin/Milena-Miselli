using Laboratorio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; // <- agregar esto
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InmobiliariaDbContext>(
    options => options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString)));

// --- Agregar autenticación por cookies ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

var app = builder.Build();

// --- Inicialización automática de la base de datos ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InmobiliariaDbContext>();
    try
    {
        // Verificar si la base de datos existe y tiene datos
        if (!context.Database.CanConnect() || !context.Usuarios.Any())
        {
            // Ejecutar el script SQL de inicialización
            await InicializarBaseDeDatos(connectionString);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al inicializar la base de datos: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// --- Habilitar autenticación ---
app.UseAuthentication();
app.UseAuthorization();

// Cambiar ruta por defecto a Usuarios/Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}");

app.Run();

// --- Método para inicializar la base de datos ---
static async Task InicializarBaseDeDatos(string connectionString)
{
    try
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        
        // Leer el archivo SQL
        var sqlScript = await File.ReadAllTextAsync("mydb.sql");
        
        // Dividir el script en comandos individuales
        var commands = sqlScript.Split(new[] { ";\r\n", ";\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Where(cmd => !string.IsNullOrWhiteSpace(cmd) && !cmd.StartsWith("--"))
            .Select(cmd => cmd.Trim())
            .Where(cmd => cmd.Length > 0);
        
        foreach (var command in commands)
        {
            if (!string.IsNullOrWhiteSpace(command))
            {
                using var cmd = new MySqlCommand(command, connection);
                await cmd.ExecuteNonQueryAsync();
            }
        }
        
        Console.WriteLine("Base de datos inicializada correctamente con datos de prueba.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al ejecutar el script SQL: {ex.Message}");
        throw;
    }
}
