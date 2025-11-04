namespace EscaMaker.Utils;

public static class ListExtra
{
    public static void Resize<T>(this List<T>? list, int sz, Func<T> c)
    {
        if (list == null)
            return;
        int cur = list.Count;
        if (sz < cur)
            list.RemoveRange(sz, cur - sz);
        else if (sz > cur)
        {
            if (sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
                list.Capacity = sz;
            var loopV = sz - cur;
           
            list.AddRange(Enumerable.Range(0, sz - cur).Select((_) => c()));
        }
    }
    public static void Resize<T>(this List<T>? list, int sz) where T : new()
    {
        Resize(list, sz, () => new T());
    }
}
