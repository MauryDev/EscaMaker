namespace EscaMaker.View;

public record ScheduleInfoPDF(ScheduleInfo ScheduleInfo, IEnumerable<IEnumerable<byte>> PeriodDates, IEnumerable<IEnumerable<string>> PeriodNames);
