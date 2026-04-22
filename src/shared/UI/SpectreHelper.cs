// src/shared/UI/SpectreHelper.cs
using Spectre.Console;

namespace AirTicketSystem.shared.UI;

public static class SpectreHelper
{
    // ── Títulos y secciones ─────────────────────────────────────────

    public static void MostrarTitulo(string titulo)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule($"[bold deepskyblue1]{titulo}[/]")
            .RuleStyle("deepskyblue1"));
        AnsiConsole.WriteLine();
    }

    public static void MostrarSubtitulo(string texto)
    {
        AnsiConsole.MarkupLine($"[bold yellow]  {texto}[/]");
        AnsiConsole.WriteLine();
    }

    public static void MostrarSeparador()
    {
        AnsiConsole.Write(new Rule().RuleStyle("grey"));
    }

    // ── Mensajes ─────────────────────────────────────────────────────

    public static void MostrarExito(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold green]  ✓ {Markup.Escape(mensaje)}[/]");
        AnsiConsole.WriteLine();
    }

    public static void MostrarError(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold red]  ✗ {Markup.Escape(mensaje)}[/]");
        AnsiConsole.WriteLine();
    }

    public static void MostrarAdvertencia(string mensaje)
    {
        AnsiConsole.MarkupLine($"[bold yellow]  ⚠ {Markup.Escape(mensaje)}[/]");
        AnsiConsole.WriteLine();
    }

    public static void MostrarInfo(string mensaje)
    {
        AnsiConsole.MarkupLine($"[deepskyblue1]  ℹ {Markup.Escape(mensaje)}[/]");
        AnsiConsole.WriteLine();
    }

    // ── Inputs ────────────────────────────────────────────────────────

    public static string PedirTexto(string etiqueta, bool obligatorio = true)
    {
        while (true)
        {
            var prompt = new TextPrompt<string>($"  [white]{etiqueta}:[/]")
                .AllowEmpty();

            var valor = AnsiConsole.Prompt(prompt).Trim();

            if (!obligatorio || !string.IsNullOrWhiteSpace(valor))
                return valor;

            MostrarAdvertencia($"El campo '{etiqueta}' es obligatorio.");
        }
    }

    public static string PedirPassword(string etiqueta = "Contraseña")
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>($"  [white]{etiqueta}:[/]")
                .Secret());
    }

    public static int PedirEntero(string etiqueta)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"  [white]{etiqueta}:[/]")
                .ValidationErrorMessage("[red]Ingrese un número entero válido.[/]"));
    }

    public static int PedirEnteroEnRango(string etiqueta, int min, int max)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>($"  [white]{etiqueta}:[/]")
                .ValidationErrorMessage($"[red]Ingrese un valor entre {min} y {max}.[/]")
                .Validate(n => n >= min && n <= max
                    ? ValidationResult.Success()
                    : ValidationResult.Error()));
    }

    public static decimal PedirDecimal(string etiqueta)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<decimal>($"  [white]{etiqueta}:[/]")
                .ValidationErrorMessage("[red]Ingrese un número decimal válido.[/]"));
    }

    public static bool Confirmar(string mensaje)
    {
        return AnsiConsole.Confirm($"  [yellow]{mensaje}[/]");
    }

    public static void EsperarTecla(string mensaje = "Presione Enter para continuar...")
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[grey]  {mensaje}[/]");
        Console.ReadLine();
    }

    // ── Menú de selección ─────────────────────────────────────────────

    public static T SeleccionarOpcion<T>(string titulo, IEnumerable<T> opciones,
        Func<T, string> etiqueta) where T : notnull
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<T>()
                .Title($"  [bold]{titulo}[/]")
                .PageSize(12)
                .AddChoices(opciones)
                .UseConverter(etiqueta));
    }

    public static string SeleccionarOpcionTexto(string titulo,
        IEnumerable<string> opciones)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"  [bold]{titulo}[/]")
                .PageSize(12)
                .AddChoices(opciones));
    }

    // ── Tablas ────────────────────────────────────────────────────────

    public static Table CrearTabla(params string[] columnas)
    {
        var tabla = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey);

        foreach (var col in columnas)
            tabla.AddColumn(new TableColumn($"[bold deepskyblue1]{col}[/]"));

        return tabla;
    }

    public static Table AgregarFila(Table tabla, params string[] celdas)
    {
        tabla.AddRow(celdas);
        return tabla;
    }

    public static void MostrarTabla(Table tabla)
    {
        AnsiConsole.Write(tabla);
        AnsiConsole.WriteLine();
    }

    // ── Panel ────────────────────────────────────────────────────────

    public static void MostrarPanel(string titulo, string contenido,
        Color? color = null)
    {
        var panel = new Panel(contenido)
        {
            Header = new PanelHeader($"[bold]{titulo}[/]"),
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(color ?? Color.DeepSkyBlue1)
        };
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    // ── Spinner ───────────────────────────────────────────────────────

    public static async Task ConSpinner(string mensaje, Func<Task> accion)
    {
        await AnsiConsole.Status()
            .StartAsync($"[deepskyblue1]{mensaje}[/]", async _ => await accion());
    }

    public static async Task<T> ConSpinner<T>(string mensaje, Func<Task<T>> accion)
    {
        return await AnsiConsole.Status()
            .StartAsync($"[deepskyblue1]{mensaje}[/]", async _ => await accion());
    }
}
