using MudBlazor;

namespace EscaMaker.View
{
    public class ScheduleTypeView
    {
        public List<List<string>>? Names { get; set; }
        public string? Name { get; set; }
        public IEnumerable<string>? Headers { get; set; }

        public Func<IEnumerable<IEnumerable<byte>>>? Dates { get; set; }

        IEnumerable<(byte,string)>? FindName(string nameGP)
        {
            
            var Names_datasSemanas = Dates().Zip(Names!);
            var daysName = Names_datasSemanas.SelectMany((e) => e.First.Zip(e.Second));
            return daysName.Where((e) => e.Second.Equals(nameGP));
        }

        public static IEnumerable<( string? Name,byte,string)>? GetDays(IEnumerable<ScheduleTypeView> SchedulesTypeView, string nameGP)
        {
            var query = from scheduletypeview in SchedulesTypeView
                        let namesResult = scheduletypeview.FindName(nameGP)
                        where namesResult != null
                        from nameResult in namesResult
                        select (scheduletypeview.Name, nameResult.Item1, nameResult.Item2);
            return query.OrderBy(e => e.Item2);
        }

        public void Clear()
        {
            if (Names == null) return;
            for (int groupIndex = 0; groupIndex < this.Names.Count; groupIndex++)
            {
                var group = this.Names[groupIndex];
                for (int itemIndex = 0; itemIndex < group.Count; itemIndex++)
                {
                    group[itemIndex] = string.Empty;
                }
            }
        }
    }
}
