using EscaMaker.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EscaMaker.View;

public class ScheduleInfo
{
    public enum DaysType
    {
        SDQ,
        DQ,
        S,
        DQSx
    }
    static string[] Headers = ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado",];
    static Dictionary<DaysType, byte[]> HeadersEscalaType = new()
    {
        {DaysType.S, [6] },
        {DaysType.DQ, [0,3] },
        {DaysType.SDQ, [6,0,3] },
        {DaysType.DQSx, [0, 3,5] }
    };

    public string Name { get; set; }
    public DaysType daysType { get; set; }
   
    public ScheduleInfo(string name, DaysType daysType)
    {
        
        this.Name = name;
        this.daysType = daysType;
    }

    public IEnumerable<string> GetHeaders()
    {
        return GetHeaders(this.daysType);
    }

    public static IEnumerable<string> GetHeaders(DaysType diasType)
    {
        if (HeadersEscalaType.TryGetValue(diasType, out var dias))
        {
            return dias.Select((e) => Headers[e]);
        }
        else
        {
            return [];
        }
    }

    public IEnumerable<IEnumerable<byte>> GetDates(int month, int year)
    {
        return GetDatas(this.daysType, month,year);
    }

   
    public static IEnumerable<IEnumerable<byte>> GetDatas(DaysType diasType,int month, int year)
    {
        var dateonly = new DateOnly(year, month, 1);
        if (HeadersEscalaType.TryGetValue(diasType, out var dias))
        {
            return dias.Select((e) => DateTimeUtil.Days(month,year, (DayOfWeek)e));
        }
        return [];
    }

    public static ScheduleInfo[]? LoadFromResource()
    {
        var escalaInfos = Resources.Resource.escalaInfos;
        return JsonSerializer.Deserialize<ScheduleInfo[]>(escalaInfos, JsonEnumString.GetOptions());

    }

}
