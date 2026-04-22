// Program.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared;
using AirTicketSystem.shared.UI;
using AirTicketSystem.modules.user.Application.UseCases;
using AirTicketSystem.UI.Auth;
using AirTicketSystem.UI;

// ── Configuración ────────────────────────────────────────────────────────────
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// ── Contenedor DI ────────────────────────────────────────────────────────────
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddAirTicketSystem(configuration);

var provider = services.BuildServiceProvider();

// ── Verificar conexión ───────────────────────────────────────────────────────
try
{
    await using var startScope = provider.CreateAsyncScope();
    var context = startScope.ServiceProvider
        .GetRequiredService<AirTicketSystem.shared.context.AppDbContext>();
    await context.Database.CanConnectAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"  ✗ Error de conexión a la base de datos: {ex.Message}");
    Console.ResetColor();
    return;
}

// ── Seed inicial ─────────────────────────────────────────────────────────────
try
{
    await using var seedScope = provider.CreateAsyncScope();
    var seed = seedScope.ServiceProvider.GetRequiredService<SeedAdminUseCase>();
    await seed.ExecuteAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  ⚠ Advertencia en seed inicial: {ex.Message}");
    Console.ResetColor();
}

// ── Bucle principal ──────────────────────────────────────────────────────────
var session = provider.GetRequiredService<SessionContext>();

while (true)
{
    var authMenu = new AuthMenu(provider, session);
    var autenticado = await authMenu.MostrarAsync();

    if (!autenticado)
    {
        Spectre.Console.AnsiConsole.MarkupLine(
            "\n[grey]  Hasta luego. ¡Buen vuelo![/]\n");
        break;
    }

    var mainMenu = new MainMenu(provider, session);
    await mainMenu.EnrutarAsync();
}
