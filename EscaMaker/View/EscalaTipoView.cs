namespace EscaMaker.View
{
    public class EscalaTipoView
    {
        public List<List<string>>? Names { get; set; }
        public string? Name { get; set; }
        public IEnumerable<string>? Headers { get; set; }

        public Func<IEnumerable<IEnumerable<byte>>> Datas { get; set; }

        IEnumerable<(byte,string)>? FindName(string nameGP)
        {
            
            var Names_datasSemanas = Datas().Zip(Names);
            var diasNomes = Names_datasSemanas.SelectMany((e) => e.First.Zip(e.Second));
            return diasNomes.Where((e) => e.Second.Equals(nameGP));
        }

        public static IEnumerable<( string? Name,byte,string)>? GetDias(IEnumerable<EscalaTipoView> escalaTipoViews, string nameGP)
        {
            var query = from escalaTipoView in escalaTipoViews
                        let nomesEncontrados = escalaTipoView.FindName(nameGP)
                        where nomesEncontrados != null
                        from nomeEncontrado in nomesEncontrados
                        select (escalaTipoView.Name, nomeEncontrado.Item1, nomeEncontrado.Item2);
            return query.OrderBy(e => e.Item2);
        }
    }
}
