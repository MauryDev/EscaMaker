namespace EscaMaker.Utils
{
    public class EscalaInfo
    {
       public enum DiasType
        {
            SDQ,
            DQ,
            S
        }
        public string Name { get; set; }
        public DiasType diasType { get; set; }
        static List<string> Headers = ["Sábado", "Domingo", "Quarta"];
        
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
            return diasType switch
            {
                DiasType.DQ => Headers.Skip(1),
                DiasType.S => Headers.Take(1),
                _ => Headers,
            };
        }
        public IEnumerable<IEnumerable<byte>> GetDatas(IEnumerable<IEnumerable<byte>> datas)
        {
            return GetDatas(this.diasType, datas);
        }
        public static IEnumerable<IEnumerable<byte>> GetDatas(DiasType diasType, IEnumerable<IEnumerable<byte>> datas)
        {
            return diasType switch
            {
                DiasType.DQ => datas.Skip(1),
                DiasType.S => datas.Take(1),
                _ => datas,
            };
        }
    }
}
