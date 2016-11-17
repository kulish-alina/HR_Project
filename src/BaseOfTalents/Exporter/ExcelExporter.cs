using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using DAL.DTO.ReportDTO;

namespace Exporter
{
    public class ExcelExporter : IExporter<ExportDataSet>
    {
        private class DataContent
        {
            public string Date { get; set; }
            public string Comment { get; set; }
        }

        public Stream Export(ExportDataSet dataSet)
        {
            //Create a book
            var workbook = new XLWorkbook(XLEventTracking.Disabled);
            IXLWorksheet ws = workbook.Worksheets.Add("Report");
            int headerTop = 1, headerBottom = 3, contentTop = headerBottom + 1;

            //Create header of a table
            ws.Cell(headerTop, 1).Value = "Location";
            ws.Range(headerTop, 1, headerBottom, 1).Merge();

            ws.Cell(headerTop, 2).Value = "Candidate";
            ws.Range(headerTop, 2, headerBottom, 2).Merge();

            ws.Cell(headerTop, 3).Value = "Vacancy";
            ws.Range(headerTop, 3, headerBottom, 3).Merge();


            int column = 4;
            foreach (var stage in dataSet.Stages)
            {
                ws.Cell(2, column).Value = stage.Title;
                ws.Range(2, column, 2, column + 1).Merge();
                ws.Cell(headerBottom, column).Value = "Date";
                ws.Cell(headerBottom, column + 1).Value = "Comments";

                var content = new List<DataContent>();
                foreach (IEnumerable<StageInfoDTO> x in dataSet.StagesFromReport)
                {
                    if (!x.Any(y => y.StageId == stage.Id))
                    {
                        content.Add(new DataContent
                        {
                            Comment = "",
                            Date = ""
                        });
                    }
                    else
                    {
                        var st = x.First(y => y.StageId == stage.Id);
                        content.Add(new DataContent
                        {
                            Comment = st.Comment,
                            Date = st.PassDate.ToString()
                        });
                    }
                }

                ws.Cell(contentTop, column).InsertData(content);
                column += 2;
            }

            ws.Cell(headerTop, 4).Value = "Stages";
            ws.Range(headerTop, 4, headerTop, column - 1).Merge();

            //Fill it with content
            int current = contentTop;
            foreach (var location in dataSet.Locations)
            {
                ws.Cell(current, 1).Value = location.Item1.Title;
                ws.Range(current, 1, current + location.Item2 - 1, 1).Merge();
                current += location.Item2;
            }

            ws.Cell(contentTop, 2).InsertData(dataSet.CandidateVanancyData);

            //Worksheet settings
            ws.Columns().AdjustToContents();

            //Get excel file out to stream
            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
