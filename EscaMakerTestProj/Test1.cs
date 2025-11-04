
namespace EscaMakerTestProj
{
    [TestClass]
    public sealed class Test1
    {

        public static int FirstDayFromWeekDay(int mes, int ano, DayOfWeek dayOfWeek)
        {
            var totaldays = DateTime.DaysInMonth(ano, mes);

            var firstDay = new DateTime(ano, mes, 1);
            if (firstDay.DayOfWeek == dayOfWeek)
                return 1;
            else if (firstDay.DayOfWeek < dayOfWeek)
            {
                return (int)dayOfWeek - (int)firstDay.DayOfWeek  + 1;

            }
            else
            {
                return 7 - (int)firstDay.DayOfWeek + (int)dayOfWeek + 1;
            }
        }
        public static List<byte> Days(int mes, int ano, DayOfWeek dayOfWeek)
        {
            var totaldays = DateTime.DaysInMonth(ano, mes);

            var firstDay = FirstDayFromWeekDay(mes,ano,dayOfWeek);
            var ls = new List<byte>(5);
            for (int i = firstDay; i <= totaldays; i+= 7)
            {
                ls.Add((byte)i);
            }
            return ls;
        }

        void AssertList<T>(List<T> expected, List<T> actual)
        {
            var len = expected.Count;
            Assert.AreEqual(len, actual.Count);
            for (int i = 0; i < len; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }
        [TestMethod]
        public void TestMethod1()
        {
            int mes = 8;
            int ano = 2025;

            var firstSabado = Days(mes, ano, DayOfWeek.Saturday);
            var firstDomingo = Days(mes, ano, DayOfWeek.Sunday);
            var firstQuarta = Days(mes, ano, DayOfWeek.Wednesday);

            AssertList<byte>([2, 9, 16, 23, 30], firstSabado);
            AssertList<byte>([3, 10, 17, 24, 31], firstDomingo);
            AssertList<byte>([6, 13, 20, 27], firstQuarta);


        }
    }
}
