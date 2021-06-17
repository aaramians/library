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
    public class DataSetpenXMLWrapper : OpenXMLWrapper
    {
        public DataSet sqlDS { get; set; }

        public void CreateWorkBook(FileInfo File, DataSet ds)
        {
            this.sqlDS = ds;

            base.CreateWorkBook(File);
        }

        public override void PopulateSheets(SpreadsheetDocument spDoc, WorkbookPart workbookpart, Sheets sheets)
        {
            for (int i = 0; i < sqlDS.Tables.Count; i++)
            {
                Trace.TraceInformation("OpenXML adding sheet: {0}", sqlDS.Tables[i].TableName);

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();


                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                // Append a new worksheet and associate it with the workbook.
                Sheet sheet = new Sheet()
                {
                    Id = spDoc.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = (UInt32)i + 1,
                    Name = sqlDS.Tables[i].TableName
                };

                ApplyColumns(workbookpart, worksheetPart, sheet, sqlDS.Tables[i]);

                Row HeaderRow = new Row() { };
                for (int j = 0; j < sqlDS.Tables[i].Columns.Count; j++)
                {
                    Cell header = new Cell()
                    {

                        CellValue = new CellValue(sqlDS.Tables[i].Columns[j].ColumnName),
                        DataType = CellValues.String,
                        StyleIndex = 2
                    };
                    HeaderRow.Append(header);
                }
                sheetData.Append(HeaderRow);

                Trace.TraceInformation("OpenXML adding rows");

                for (int j = 0; j < sqlDS.Tables[i].Rows.Count; j++)
                {
                    Row dataRow = new Row() { };
                    dataRow.Hidden = HideRow(sqlDS.Tables[i].Rows[j]);
                    for (int k = 0; k < sqlDS.Tables[i].Columns.Count; k++)
                    {
                        Cell c = new Cell()
                        {
                            DataType = CellValues.InlineString,
                            StyleIndex = 1
                        };

                        var t = new Text() { Text = sqlDS.Tables[i].Rows[j][k].ToString() };

                        var q = new InlineString();
                        q.AppendChild(t);

                        c.AppendChild(q);

                        dataRow.Append(c);
                    }
                    sheetData.Append(dataRow);
                }

                sheets.Append(sheet);

                // testing filter

                ApplyFilter(workbookpart, worksheetPart, sheet, sqlDS.Tables[i]);

            }
        }

        public virtual void ApplyColumns(WorkbookPart workbookpart, WorksheetPart worksheetPart, Sheet sheet, object datasource)
        {

        }

        public virtual bool HideRow(DataRow dataRow)
        {
            return false;
        }

        public virtual void ApplyFilter(WorkbookPart workbookpart, WorksheetPart worksheetPart, Sheet sheet, object datasource)
        {

        }

    }
}
