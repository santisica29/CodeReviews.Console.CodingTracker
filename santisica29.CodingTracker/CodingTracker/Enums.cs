using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CodingTracker;
internal class Enums
{
    internal enum MenuOption
    {
        [Description("Add Coding Session")]
        AddCodingSession,
        [Description("View Coding Session")]
        ViewCodingSession,
        [Description("Delete Coding Session")]
        DeleteCodingSession,
        [Description("Update Coding Session")]
        UpdateCodingSession,
        [Description("Start Session")]
        StartSession,
        [Description("View Report of Coding Session")]
        ViewReportOfCodingSession,
        [Description("Exit")]
        Exit
    }

    internal enum ReportOption
    {
        Days,
        Months,
        Years,
        Total
    }
}
