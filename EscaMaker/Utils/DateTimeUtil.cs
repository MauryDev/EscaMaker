namespace EscaMaker.Utils;

public static class DateTimeUtil
{
    static readonly string[] mesNomes = [
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
    public static string[] GetMesesNome() => mesNomes;

    public static string? GetMesNome(int mes)
    {
        if (mes > 0 && mes < 13)
        {
            return mesNomes[mes - 1];
        }
        return null;
    }
    public static int FirstDayFromWeekDay(int mes, int ano, DayOfWeek dayOfWeek)
    {
        var firstDay = new DateTime(ano, mes, 1);
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
    public static List<byte> Days(int mes, int ano, DayOfWeek dayOfWeek)
    {
        var totaldays = DateTime.DaysInMonth(ano, mes);

        var firstDay = FirstDayFromWeekDay(mes, ano, dayOfWeek);
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
