using Spire.Xls;
using Stocks.Core.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Stocks.Core.Excel
{
    public class ExcelSaver : IExcelSaver
    {
        public byte[] SaveToExcel(IEnumerable<StockDividend> stockDividends)
        {
            var workbook = new Workbook();
            var sheet = workbook.Worksheets[0];
            sheet.Name = "StockDividends";
            var row = 2;
            foreach (var sd in stockDividends)
            {
                var column = 1;
                sheet.Range[row, column++].Text = sd.Name;
                sheet.Range[row, column++].Text = sd.ShortName;
                sheet.Range[row, column++].NumberValue = sd.Price;
                sheet.Range[row, column++].DateTimeValue = sd.LatestDividendHistory.ExDate;
                sheet.Range[row, column++].DateTimeValue = sd.LatestDividendHistory.RecordDate;
                sheet.Range[row, column++].DateTimeValue = sd.LatestDividendHistory.PayDate;
                sheet.Range[row, column++].DateTimeValue = sd.LatestDividendHistory.DeclarationDate;
                sheet.Range[row, column++].DateTimeValue = sd.LatestDividendHistory.WhenToBuy;
                sheet.Range[row, column].NumberValue = sd.LatestDividendHistory.Amount;
                sheet.Range[row, column++].NumberFormat = "0.00";
                sheet.Range[row, column].NumberValue = sd.DividendToPrice;
                sheet.Range[row, column].NumberFormat = "0.00%";
                sheet.Range[row, column].Style.Color = sd.DividendToPrice.GetBackgroundColorByDividendToPrice();
                sheet.Range[row, column].Style.Font.Color = sd.DividendToPrice.GetFontColorByDividendToPrice();

                row++;
            }

            SetHeader(workbook, sheet);
            using var ms = new MemoryStream();
            workbook.SaveToStream(ms, FileFormat.Version2016);
            return ms.ToArray();
        }

        private static void SetHeader(Workbook workbook, Worksheet sheet)
        {
            var headerColumns = GetHeaderColumns();
            FillHeaderCells(workbook, sheet, headerColumns);
            SetHeaderCellFormat(sheet, headerColumns.Length);
        }

        private static void SetHeaderCellFormat(Worksheet sheet, int headerColumnsCouont)
        {
            sheet.AutoFilters.Range = sheet.Range[1, 1, 1, headerColumnsCouont];
            sheet.SetRowHeight(1, 50);
            sheet.FreezePanes(2, 1);
        }

        private static void FillHeaderCells(Workbook workbook, Worksheet sheet, string[] headerColumns)
        {
            var fontBold = CreateBoldFont(workbook);

            for (int i = 1; i <= headerColumns.Length; i++)
            {
                sheet.Range[1, i].Text = headerColumns[i - 1];
                sheet.Range[1, i].Style.VerticalAlignment = VerticalAlignType.Top;
                SetFontToRichText(1, i, sheet, fontBold);
                sheet.AutoFitColumn(i);
            }
        }

        private static void SetFontToRichText(int row, int column, Worksheet sheet, ExcelFont font)
        {
            var richText = sheet.Range[row, column].RichText;
            richText.SetFont(0, richText.Text.Length - 1, font);
        }

        private static ExcelFont CreateBoldFont(Workbook workbook)
        {
            var fontBold = workbook.CreateFont();
            fontBold.IsBold = true;
            return fontBold;
        }
        private static ExcelFont CreateWhiteFont(Workbook workbook)
        {
            var fontWhite = workbook.CreateFont();
            fontWhite.Color = Color.White;
            return fontWhite;
        }

        private static string[] GetHeaderColumns()
            => new[] { nameof(StockDividend.Name), nameof(StockDividend.ShortName), nameof(StockDividend.Price), nameof(DividendHistory.ExDate), nameof(DividendHistory.RecordDate), nameof(DividendHistory.PayDate), nameof(DividendHistory.DeclarationDate), nameof(DividendHistory.WhenToBuy), nameof(DividendHistory.Amount), nameof(StockDividend.DividendToPrice) };
    }
}
