namespace CodingTracker.Models;
public class CodingSession
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan Duration { get; set; }

    public CodingSession()
    {

    }
    public CodingSession(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
        Duration = CalculateDuration();
    } 
    internal TimeSpan CalculateDuration()
    {
        if (EndTime < StartTime)
        {
            throw new ArgumentException("EndTime cannot be earlier than StartTime.");
        }
        return EndTime - StartTime;
    }
}
