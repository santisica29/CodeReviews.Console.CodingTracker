using CodingTracker.Controller;
using Spectre.Console;
using static CodingTracker.Enums;

namespace CodingTracker.View;
internal class UserInterface
{
    private readonly CodingController _codingController = new();

    internal void MainMenu()
    {
        bool flag = true;
        while (flag)
        {
            Console.Clear();

            var choice = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOption>()
            .Title("CODING TRACKER")
            .AddChoices(Enum.GetValues<MenuOption>()));

            switch (choice)
            {
                case MenuOption.AddCodingSession:
                    _codingController.AddSession();
                    break;
                case MenuOption.ViewCodingSession:
                    _codingController.ViewSessions(_codingController.GetSessions());
                    break;
                case MenuOption.DeleteCodingSession:
                    _codingController.DeleteSession();
                    break;
                case MenuOption.UpdateCodingSession:
                    _codingController.UpdateSession();
                    break;
                case MenuOption.StartSession:
                    _codingController.StartSession();
                    break;
                case MenuOption.ViewReportOfCodingSession:
                    _codingController.ViewReportOfCodingSession();
                    break;
                case MenuOption.Exit:
                    AnsiConsole.MarkupLine("Goodbye");
                    flag = false;
                    break;
                default:
                    AnsiConsole.MarkupLine("Invalid input");
                    break;
            }
        }
    }
}
