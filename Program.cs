using Laboratorio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies; 
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<InmobiliariaDbContext>(
    options => options.UseMySql(connectionString,
        ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<InmobiliariaDbContext>();
    try
    {
        if (!context.Database.CanConnect() || !context.Usuarios.Any())
        {
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}");

app.Run();

static async Task InicializarBaseDeDatos(string connectionString)
{
    try
    {
        using var connection = new MySqlConnection(connectionString);
        await connection.OpenAsync();
        
        var sqlScript = await File.ReadAllTextAsync("mydb.sql");
        
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
