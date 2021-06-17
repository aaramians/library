using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics;
using System.IO;
using System.Data;

namespace Library.Converters.Excel
{
    public class OpenXMLWrapper
    {
        public void CreateWorkBook(FileInfo File)
        {

            using (var spDoc = SpreadsheetDocument.Create(File.FullName, SpreadsheetDocumentType.Workbook))
            {
                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spDoc.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                Trace.TraceInformation("OpenXML adding new workbook/workbookpart.");

                Stylesheet styleSheet = null;

                Fonts fonts = new Fonts(
                    new Font( // Index 0 - default
                        new FontSize() { Val = 10 }
                    ),
                    new Font( // Index 1 - header
                        new FontSize() { Val = 10 },
                        new Bold(),
                        new Color() { Rgb = "FFFFFF" }
                    )
                );

                Fills fills = new Fills(
                     new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                     new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                     new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
                 );

                Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                 );

                CellFormats cellFormats = new CellFormats(
                        new CellFormat(), // default
                        new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                        new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

                styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);
                Trace.TraceInformation("OpenXML adding new style sheet");

                WorkbookStylesPart stylePart = workbookpart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = styleSheet;

                Trace.TraceInformation("OpenXML linking workbook and style sheet");

                // Add Sheets to the Workbook.
                Sheets sheets = spDoc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                PopulateSheets(spDoc, workbookpart, sheets);

                workbookpart.Workbook.Save();

                // Close the document.
                spDoc.Close();
            }
        }

        public virtual void PopulateSheets(SpreadsheetDocument spDoc, WorkbookPart workbookpart, Sheets sheets)
        {
        }

    
    }
}
