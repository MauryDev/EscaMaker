using EscaMaker.Utils;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EscaMaker.View;

public class EscalaInfo
{
    public enum DiasType
    {
        SDQ,
        DQ,
        S,
        DQSx
    }
    static string[] Headers = ["Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado",];
    static Dictionary<DiasType, byte[]> HeadersEscalaType = new()
    {
        {DiasType.S, [6] },
        {DiasType.DQ, [0,3] },
        {DiasType.SDQ, [6,0,3] },
        {DiasType.DQSx, [0, 3,5] }
    };

    public string Name { get; set; }
    public DiasType diasType { get; set; }
   
    public EscalaInfo(string name, DiasType diasType)
    {
        
        this.Name = name;
        this.diasType = diasType;
    }

    public IEnumerable<string> GetHeaders()
    {
        return GetHeaders(this.diasType);
    }

    public static IEnumerable<string> GetHeaders(DiasType diasType)
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

    public IEnumerable<IEnumerable<byte>> GetDatas(int month, int year)
    {
        return GetDatas(this.diasType, month,year);
    }

   
    public static IEnumerable<IEnumerable<byte>> GetDatas(DiasType diasType,int month, int year)
    {
        var dateonly = new DateOnly(year, month, 1);
        if (HeadersEscalaType.TryGetValue(diasType, out var dias))
        {
            return dias.Select((e) => DateTimeUtil.Days(month,year, (DayOfWeek)e));
        }
        return [];
    }

    public static EscalaInfo[]? LoadFromResource()
    {
        var escalaInfos = Resources.Resource.escalaInfos;
        
        return JsonSerializer.Deserialize<EscalaInfo[]>(escalaInfos, JsonEnumString.GetOptions());

    }

}
