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
            string? mesNome = DateTimeUtil.GetMesNome(mes);
            if (mesNome != null)
            {
                var txt = new Text($"Mês de {mesNome}");
                var font = GetPdfFont();

                txt.SetFont(font)
                    .SetFontSize(SizeFont1)
                    .SimulateBold();
                doc.Add(new Paragraph(txt)
                    .SetTextAlignment(TextAlignment.CENTER));
            }
        }
        public static void CreateTableEscala2(this Document doc, (EscalaInfoPDF escalaInfo1, EscalaInfoPDF escalaInfo2) escalasPDF, int mes)
        {
            var (escalaInfo, escalaInfo2) = escalasPDF;
            var (periodosDatas, periodosDatas2) = (escalaInfo.periodoData, escalaInfo2.periodoData);
            var (periodosNomes, periodosNomes2) = (escalaInfo.periodosNomes, escalaInfo2.periodosNomes);
            var (headersEscala, headersEscala2) = (escalaInfo.EscalaInfo.GetHeaders(), escalaInfo2.EscalaInfo.GetHeaders());
            var (datasEscala, datasEscala2) = (escalaInfo.periodoData, escalaInfo2.periodoData);
            var headersLen = 2;

            var table = new Table(UnitValue.CreatePercentArray(headersLen))
                .UseAllAvailableWidth();

            table.AddCell(new Cell(1, 1)
                                .Add(escalaInfo.EscalaInfo.Name.CreateHeaderText())
                                .SetTextAlignment(TextAlignment.CENTER));

            table.AddCell(new Cell(1, 1)
                                .Add(escalaInfo2.EscalaInfo.Name.CreateHeaderText())
                                .SetTextAlignment(TextAlignment.CENTER));

            foreach (var header in headersEscala.Concat(headersEscala2))
            {
                table.AddCell(new Cell()
                    .Add(header.CreateHeaderText()));
            }
            var rowslen = datasEscala.Concat(datasEscala2).Max(x => x.Count()) * headersLen;
            for (int i = 0; i < rowslen; i++)
                table.AddCell(new Cell());

            foreach (var (diasHeader, nameHeader) in datasEscala.Zip(periodosNomes))
            {
                var i3 = 2;
                foreach (var (dia, name) in diasHeader.Zip(nameHeader))
                {
                    var txt = new Text($"{dia:D2}/{mes:D2} - {name}");
                    txt.SetFontSize(SizeFont2);
                    table.GetCell(i3, 0)
                        .Add(new Paragraph(txt));
                    i3++;
                }
            }
            foreach (var (diasHeader, nameHeader) in datasEscala2.Zip(periodosNomes2))
            {
                var i3 = 2;
                foreach (var (dia, name) in diasHeader.Zip(nameHeader))
                {
                    var txt = new Text($"{dia:D2}/{mes:D2} - {name}");
                    txt.SetFontSize(SizeFont2);
                    table.GetCell(i3, 1)
                        .Add(new Paragraph(txt));
                    i3++;
                }
            }
            doc.Add(table);
            doc.NewLine();
        }
        public static void CreateTableEscala(this Document doc, EscalaInfoPDF escalaInfoPDF, int mes)
        {
            var escalaInfo = escalaInfoPDF.EscalaInfo;
            var periodosDatas = escalaInfoPDF.periodoData;
            var periodosNomes = escalaInfoPDF.periodosNomes;
            var headersEscala = escalaInfo.GetHeaders();
            var headersLen = headersEscala.Count();

            var table = new Table(UnitValue.CreatePercentArray(headersLen))
                .UseAllAvailableWidth();
            table.AddCell(new Cell(1, headersLen)
                .Add(escalaInfo.Name.CreateHeaderText())
                .SetTextAlignment(TextAlignment.CENTER));
            foreach (var header in headersEscala)
            {
                table.AddCell(new Cell()
                    .Add(header.CreateHeaderText()));
            }
            var rowslen = periodosDatas.Max(x => x.Count()) * headersLen;
            for (int i = 0; i < rowslen; i++)
                table.AddCell(new Cell());

            var i2 = 0;
            foreach (var (diasHeader, nameHeader) in periodosDatas.Zip(periodosNomes))
            {
                var i3 = 2;
                foreach (var (dia, name) in diasHeader.Zip(nameHeader))
                {
                    var txt = new Text($"{dia:D2}/{mes:D2} - {name}");
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
        public static MemoryStream? GeneratePDFDocument(int mes,int ano, IEnumerable<IEnumerable<IEnumerable<string>>> periodosNomes, IEnumerable<EscalaInfo> escalaInfos)
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
                var escalasInfoPeriodo = escalaInfos.Zip(periodosNomes).ToArray();
                var lenEscalas = escalasInfoPeriodo.Length;
                for (int i = 0; i < lenEscalas; i++)
                {
                    var (escala, curnomes) = escalasInfoPeriodo[i];
                    if (escala.GetHeaders().Count() == 1)
                    {
                        if (i + 1 < lenEscalas)
                        {
                            var (nextEscala, nextCurnomes) = escalasInfoPeriodo[i + 1];
                            if (nextEscala.GetHeaders().Count() > 1)
                            {
                                doc.CreateTableEscala(new(escala, escala.GetDatas(mes,ano), curnomes), mes);
                            }
                            else
                            {
                                doc.CreateTableEscala2((new(escala, escala.GetDatas(mes, ano), curnomes), new(nextEscala, nextEscala.GetDatas(mes,ano), nextCurnomes)), mes);
                                i++;
                            }

                        }
                        else
                        {
                            doc.CreateTableEscala(new(escala, escala.GetDatas(mes, ano), curnomes), mes);
                        }
                    }
                    else
                    {
                        doc.CreateTableEscala(new(escala, escala.GetDatas(mes, ano), curnomes), mes);

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
