// Program.cs
using Microsoft.Extensions.Configuration;
using AirTicketSystem.shared.helpers;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

using var context = DbContextFactory.Create(configuration);

Console.WriteLine("✈  Air Ticket System iniciando...");

// Verifica conexión a la base de datos
try
{
    await context.Database.CanConnectAsync();
    Console.WriteLine("✅ Conexión a MySQL establecida correctamente.");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error de conexión: {ex.Message}");
    return;
}

// TODO: Aquí va el menú principal
Console.WriteLine("Sistema listo.");
