namespace EscaMaker.View;

public class LocalScheduleData
{
    public required int Year { get; set; }
    public required byte Month { get; set; }
    public required List<List<string>>[] Names { get; set; }
    public required ScheduleInfo[] SchedulesInfo { get; set; }
}
