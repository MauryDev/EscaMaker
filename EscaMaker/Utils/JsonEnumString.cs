using System.Text.Json;
using System.Text.Json.Serialization;

namespace EscaMaker.Utils
{
    public static class JsonEnumString
    {
        static JsonSerializerOptions? EnumStr;
        public static JsonSerializerOptions GetOptions()
        {
            EnumStr ??= new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
            return EnumStr;
        }
    }
}
