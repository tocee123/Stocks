using Spire.Xls;
using Stocks.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Stocks.Core.Excel
{
    public class ExcelSaver : IExcelSaver
    {
        public byte[] SaveToExcel(IEnumerable<StockDividend> stockDividends)
        {
            var stockDividendsForExcel = stockDividends.Select(StockDividendForExcel.Map);
            var workbook = new Workbook();
            var sheet = workbook.Worksheets[0];
            sheet.Name = "StockDividends";
            var row = 2;
            foreach (var sd in stockDividendsForExcel)
            {
                var column = 1;
                sheet.Range[row, column++].Text = sd.Name;
                sheet.Range[row, column++].Text = sd.Ticker;
                sheet.Range[row, column++].NumberValue = sd.Price;
                sheet.Range[row, column++].DateTimeValue = sd.ExDate;
                sheet.Range[row, column++].DateTimeValue = sd.RecordDate;
                sheet.Range[row, column++].DateTimeValue = sd.PayDate;
                sheet.Range[row, column++].DateTimeValue = sd.DeclarationDate;
                sheet.Range[row, column++].DateTimeValue = sd.WhenToBuy;
                sheet.Range[row, column++].DateTimeValue = sd.WhenToSell;
                sheet.Range[row, column].NumberValue = sd.Amount;
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
            var headerColumns = StockDividendForExcel.GetPropertyNames();
            FillHeaderCells(workbook, sheet, headerColumns);
            SetHeaderCellFormat(sheet, headerColumns.Length);
        }

        private static void SetHeaderCellFormat(Worksheet sheet, int headerColumnsCount)
        {
            sheet.AutoFilters.Range = sheet.Range[1, 1, 1, headerColumnsCount];
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
                if (false)
                {
                    //TODO fix the duplication in header
                    SetFontToRichText(1, i, sheet, fontBold);
                }
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
    }
}
