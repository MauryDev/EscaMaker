using EscaMaker.View;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace EscaMaker.Utils
{
    internal static class GeneratePDF
    {
        const int SizeFont1 = 15;
        const int SizeFont2 = 13;

        static PdfFont? FontStd { get; set; }
        static PdfFont GetPdfFont()
        {
            FontStd ??= PdfFontFactory.CreateFont(Resources.Resource.georgiaFont, "");
            return FontStd;
        }
        public static Paragraph CreateHeaderText(this string text)
        {
            var txt = new Text(text);
            var font = GetPdfFont();

            txt.SetFont(font)
                .SetFontSize(SizeFont2)
                .SimulateBold();
            return new Paragraph(txt)
                .SetHorizontalAlignment(HorizontalAlignment.LEFT);
        }
        public static void NewLine(this Document doc)
        { 
            doc.Add(new Paragraph());
        }
        public static void NextPageBreak(this Document doc)
        {
            doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
        }
        public static void CreateMesName(this Document doc, int mes)
        {
            string? namesMonth = DateTimeUtil.GetMonthName(mes);
            if (namesMonth != null)
            {
                var txt = new Text($"Mês de {namesMonth}");
                var font = GetPdfFont();

                txt.SetFont(font)
                    .SetFontSize(SizeFont1)
                    .SimulateBold();
                doc.Add(new Paragraph(txt)
                    .SetTextAlignment(TextAlignment.CENTER));
            }
        }
        public static void CreateTableEscala2(this Document doc, (ScheduleInfoPDF, ScheduleInfoPDF) schedulesPDF, int month)
        {
            var (scheduleInfo, scheduleInfo2) = schedulesPDF;
            var (periodDates, periodDates2) = (scheduleInfo.PeriodDates, scheduleInfo2.PeriodDates);
            var (periodNames, periodNames2) = (scheduleInfo.PeriodNames, scheduleInfo2.PeriodNames);
            var (headersSchedule, headersSchedule2) = (scheduleInfo.ScheduleInfo.GetHeaders(), scheduleInfo2.ScheduleInfo.GetHeaders());
            var (SchedulesData, datasEscala2) = (scheduleInfo.PeriodDates, scheduleInfo2.PeriodDates);
            var headersLen = 2;

            var table = new Table(UnitValue.CreatePercentArray(headersLen))
                .UseAllAvailableWidth();

            table.AddCell(new Cell(1, 1)
                                .Add(scheduleInfo.ScheduleInfo.Name.CreateHeaderText())
                                .SetTextAlignment(TextAlignment.CENTER));

            table.AddCell(new Cell(1, 1)
                                .Add(scheduleInfo2.ScheduleInfo.Name.CreateHeaderText())
                                .SetTextAlignment(TextAlignment.CENTER));

            foreach (var header in headersSchedule.Concat(headersSchedule2))
            {
                table.AddCell(new Cell()
                    .Add(header.CreateHeaderText()));
            }
            var rowslen = SchedulesData.Concat(datasEscala2).Max(x => x.Count()) * headersLen;
            for (int i = 0; i < rowslen; i++)
                table.AddCell(new Cell());

            foreach (var (diasHeader, nameHeader) in SchedulesData.Zip(periodNames))
            {
                var i3 = 2;
                foreach (var (dia, name) in diasHeader.Zip(nameHeader))
                {
                    var txt = new Text($"{dia:D2}/{month:D2} - {name}");
                    txt.SetFontSize(SizeFont2);
                    table.GetCell(i3, 0)
                        .Add(new Paragraph(txt));
                    i3++;
                }
            }
            foreach (var (diasHeader, nameHeader) in datasEscala2.Zip(periodNames2))
            {
                var i3 = 2;
                foreach (var (day, name) in diasHeader.Zip(nameHeader))
                {
                    var txt = new Text($"{day:D2}/{month:D2} - {name}");
                    txt.SetFontSize(SizeFont2);
                    table.GetCell(i3, 1)
                        .Add(new Paragraph(txt));
                    i3++;
                }
            }
            doc.Add(table);
            doc.NewLine();
        }
        public static void CreateTableEscala(this Document doc, ScheduleInfoPDF scheduleInfoPDF, int month)
        {
            var scheduleInfo = scheduleInfoPDF.ScheduleInfo;
            var periodDates = scheduleInfoPDF.PeriodDates;
            var periodNames = scheduleInfoPDF.PeriodNames;
            var headersSchedule = scheduleInfo.GetHeaders();
            var headersLen = headersSchedule.Count();

            var table = new Table(UnitValue.CreatePercentArray(headersLen))
                .UseAllAvailableWidth();
            table.AddCell(new Cell(1, headersLen)
                .Add(scheduleInfo.Name.CreateHeaderText())
                .SetTextAlignment(TextAlignment.CENTER));
            foreach (var header in headersSchedule)
            {
                table.AddCell(new Cell()
                    .Add(header.CreateHeaderText()));
            }
            var rowslen = periodDates.Max(x => x.Count()) * headersLen;
            for (int i = 0; i < rowslen; i++)
                table.AddCell(new Cell());

            var i2 = 0;
            foreach (var (daysHeader, nameHeader) in periodDates.Zip(periodNames))
            {
                var i3 = 2;
                foreach (var (day, name) in daysHeader.Zip(nameHeader))
                {
                    var txt = new Text($"{day:D2}/{month:D2} - {name}");
                    txt.SetFontSize(SizeFont2);
                    table.GetCell(i3, i2)
                        .Add(new Paragraph(txt));
                    i3++;
                }
                i2++;
            }
            doc.Add(table);
            doc.NewLine();
        }
        public static MemoryStream? GeneratePDFDocument(int mes,int ano, IEnumerable<IEnumerable<IEnumerable<string>>> periodosNomes, IEnumerable<ScheduleInfo> escalaInfos)
        {
            FontStd = null;
            try
            {
                MemoryStream memoryStream = new();
                using var writer = new PdfWriter(memoryStream);
                writer.SetCloseStream(false);
                using PdfDocument pdfDoc = new(writer);
                using Document doc = new(pdfDoc);

                doc.SetMargins(30, 30, 30, 30);

                doc.CreateMesName(mes);
                doc.NewLine();
                var tablesLen = 0;
                var schedulesInfoPeriodo = escalaInfos.Zip(periodosNomes).ToArray();
                var lenschedules = schedulesInfoPeriodo.Length;
                for (int i = 0; i < lenschedules; i++)
                {
                    var (schedule, name_cur) = schedulesInfoPeriodo[i];
                    if (schedule.GetHeaders().Count() == 1)
                    {
                        if (i + 1 < lenschedules)
                        {
                            var (nextSchedule, next_CurName) = schedulesInfoPeriodo[i + 1];
                            if (nextSchedule.GetHeaders().Count() > 1)
                            {
                                doc.CreateTableEscala(new(schedule, schedule.GetDates(mes,ano), name_cur), mes);
                            }
                            else
                            {
                                doc.CreateTableEscala2((new(schedule, schedule.GetDates(mes, ano), name_cur), new(nextSchedule, nextSchedule.GetDates(mes,ano), next_CurName)), mes);
                                i++;
                            }

                        }
                        else
                        {
                            doc.CreateTableEscala(new(schedule, schedule.GetDates(mes, ano), name_cur), mes);
                        }
                    }
                    else
                    {
                        doc.CreateTableEscala(new(schedule, schedule.GetDates(mes, ano), name_cur), mes);

                    }
                    tablesLen++;
                    if (tablesLen == 4)
                    {
                        doc.NextPageBreak();
                        tablesLen = 0;
                    }
                }


                var footer = new Paragraph();
                footer.SetFontSize(SizeFont2);
                footer.Add(new Text("OBSERVAÇÃO: ")
                    .SimulateBold());
                footer.Add("AQUELES QUE ESTÃO NAS ESCALAS, SE POR ALGUM MOTIVO NÃO PUDER COMPARECER, POR FAVOR, ENTRE EM CONTATO COM ELISA, RAQUEL OU ANA LÚCIA. DESDE JÁ, MUITO OBRIGADO. QUE O BOM SENHOR TE ABENÇOE!");

                doc.Add(footer);

                doc.Close();
                memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning
                return memoryStream;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return null;
            }

        }
    }
}
