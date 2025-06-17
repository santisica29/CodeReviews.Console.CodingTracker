using Spectre.Console;

namespace CodingTracker.Controller;

internal abstract class BaseController
{
    protected void DisplayMessage(string message, string color = "blue")
    {
        AnsiConsole.MarkupLine($"[{color}]{message}[/]");
    }

    protected bool ConfirmDeletion(string itemName)
    {
        var confrim = AnsiConsole.Confirm($"Are you sure you want to delete {itemName}?");

        return confrim;
    }
}
