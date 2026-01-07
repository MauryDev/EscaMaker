namespace EscaMaker.Utils;

public static class DateTimeUtil
{
    static readonly string[] namesMonth = [
        "Janeiro",
        "Fevereiro",
        "Março",
        "Abril",
        "Maio",
        "Junho",
        "Julho",
        "Agosto",
        "Setembro",
        "Outubro",
        "Novembro",
        "Dezembro"
    ];
    public static string[] GetNamesMonth() => namesMonth;

    public static string? GetMonthName(int month)
    {
        if (month > 0 && month < 13)
        {
            return namesMonth[month - 1];
        }
        return null;
    }
    public static int FirstDayFromWeekDay(int month, int year, DayOfWeek dayOfWeek)
    {
        var firstDay = new DateTime(year, month, 1);
        if (firstDay.DayOfWeek == dayOfWeek)
            return 1;
        else if (firstDay.DayOfWeek < dayOfWeek)
        {
            return (int)dayOfWeek - (int)firstDay.DayOfWeek + 1;

        }
        else
        {
            return 7 - (int)firstDay.DayOfWeek + (int)dayOfWeek + 1;
        }
    }
    public static List<byte> Days(int month, int year, DayOfWeek dayOfWeek)
    {
        var totaldays = DateTime.DaysInMonth(year, month);

        var firstDay = FirstDayFromWeekDay(month, year, dayOfWeek);
        var ls = new List<byte>(5);
        for (int i = firstDay; i <= totaldays; i += 7)
        {
            ls.Add((byte)i);
        }
        return ls;
    }

    public static string GetDiaSemanaNome(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Sunday => "Domingo",
            DayOfWeek.Monday => "Segunda-feira",
            DayOfWeek.Tuesday => "Terça-feira",
            DayOfWeek.Wednesday => "Quarta-feira",
            DayOfWeek.Thursday => "Quinta-feira",
            DayOfWeek.Friday => "Sexta-feira",
            DayOfWeek.Saturday => "Sábado",
            _ => "",
        };
    }
    public static DateTime ToFirstDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }
}
