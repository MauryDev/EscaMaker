

using iText.IO.Image;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

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

        public static void CreateSimplePdf(string sourcePath, string outputPath, DateOnly dateOnly, List<(string, string)> values)
        {
            using var memoryStream = new FileStream(outputPath,FileMode.Create);
            using var writer = new PdfWriter(memoryStream);
            writer.SetCloseStream(false);
            using PdfDocument pdfDoc = new(writer);
            using Document doc = new(pdfDoc);

            doc.SetMargins(30, 30, 30, 30);

            ImageData imageData = ImageDataFactory.Create(sourcePath);
            Image image = new(imageData);
            var size = pdfDoc.GetDefaultPageSize();
            var test = image.GetImageScaledHeight();
            image.SetFixedPosition(0, 0);
            image.ScaleAbsolute(size.GetWidth(),size.GetHeight());
            doc.Add(image);

            var paragraph = new Paragraph();
            paragraph.Add(new Text("PROGRAMAÇÃO DE CULTO").SetFontSize(25f));
            paragraph.SetMarginLeft(80f);
            paragraph.SetTextAlignment(TextAlignment.CENTER);
            doc.Add(paragraph);

            var paragraph2 = new Paragraph();
            paragraph2.Add(new Text($"{dateOnly.Day:D2}/{dateOnly.Month:D2} - {dateOnly:dddd}").SetFontSize(25f));
            paragraph2.SetMarginLeft(80f);
            paragraph2.SetMarginBottom(100f);

            paragraph2.SetTextAlignment(TextAlignment.CENTER);
            doc.Add(paragraph2);


            foreach (var value in values)
            {
                var paragraphText = new Paragraph();
                paragraphText.Add(new Text($"{value.Item1} - {value.Item2}").SetFontSize(25f));
                paragraphText.SetMarginLeft(80f);
                paragraphText.SetMarginBottom(20f);

                doc.Add(paragraphText);

            }


            doc.Close();
            memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning



        }

        [TestMethod]
        public void CreatePDF()
        {
            string outputPath = "doc.pdf";
            CreateSimplePdf("C:\\dev\\Visual Studio\\EscaMaker\\EscaMakerTestProj\\2151904440.jpg", outputPath, new DateOnly(2025, 12, 10), [
                ("Ana Célia","Pregação"),
                ("Manoel Santos", "Louvor Especial"),
                ("Manoel Santos", "Recepção"),
                ("Maria Da Lapa", "Sonoplastia"),
                ("Maria José", "Momento Prévios"),
                ("Maria José", "Programação"),
                ("Teste", "Teste")
            ]);
        }
    }
}
