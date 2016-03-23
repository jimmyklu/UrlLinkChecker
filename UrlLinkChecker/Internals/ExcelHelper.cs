namespace UrlLinkChecker.Internals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    using Excel = Microsoft.Office.Interop.Excel;
    using ExcelApp = Microsoft.Office.Interop.Excel.Application;

    internal class ExcelHelper : IDisposable
    {
        private bool disposed;

        private ExcelApp exApp = null;
        private Excel.Workbooks books = null;
        private Excel._Workbook book1 = null;
        private Excel.Worksheet xlWorkSheet = null;
        private Excel.Range cellsRange = null;


        public static void AddStyle(Excel.Range range, CellStyle style)
        {
            if (null != range && null != style)
            {
                Excel.Worksheet sheet = null;

                Excel._Workbook book = null;

                try
                {
                    sheet = (Excel.Worksheet)range.Parent;

                    book = ((Excel._Workbook)sheet.Parent);

                    if (!book.Styles.Cast<Excel.Style>().Any(s => s.Name == style.Name))
                    {
                        var newStyle = book.Styles.Add(style.Name);
                        newStyle.Font.Color = style.ForeColor;
                        newStyle.Font.Bold = style.FontBold;
                        if (null != style.BackColor)
                        {
                            newStyle.Interior.Color = System.Drawing.ColorTranslator.ToOle(style.BackColor.GetValueOrDefault());
                            newStyle.Interior.Pattern = style.Pattern;
                        }
                        else
                        {
                            newStyle.Interior.ColorIndex = 0;
                            newStyle.Interior.Pattern = Excel.XlPattern.xlPatternNone;
                        }
                    }
                }
                finally
                {
                    if (null != book)
                    {
                        Marshal.ReleaseComObject(book);
                        book = null;
                    }

                    if (null != sheet)
                    {
                        Marshal.ReleaseComObject(sheet);
                        sheet = null;
                    }
                }
            }        
        }

        public static void ApplyStyle(Excel.Range[] targetCells, CellStyle style)
        {
            if (null != targetCells && targetCells.Length > 0 && null != style)
            {
                AddStyle(targetCells[0], style);

                foreach (Excel.Range eaCell in targetCells)
                {
                    eaCell.Style = style.Name;
                }
            }
        }

        public void OpenFromClipboard(ExcelColumnCollection colDetails = null)
        {
            try
            {
                exApp = new ExcelApp() { Visible = false };
                books = (Excel.Workbooks)exApp.Workbooks;
                book1 = (Excel._Workbook)(books.Add(Missing.Value));

                xlWorkSheet = (Excel.Worksheet)book1.Worksheets.Item[1];

                cellsRange = (Excel.Range)xlWorkSheet.Cells[1, 1];

                cellsRange.Select();
                xlWorkSheet.PasteSpecial(cellsRange, NoHTMLFormatting: false);

                if (null != colDetails)
                {
                    foreach(ExcelColumn eaCol in colDetails.Columns)
                    {
                        if (eaCol.IsValid())
                        {
                            cellsRange = (Excel.Range)xlWorkSheet.Cells[eaCol.Index];
                            cellsRange.ColumnWidth = eaCol.Width;

                            if (eaCol.HeaderIsBold)
                            {
                                ((Excel.Range)xlWorkSheet.Cells[1, eaCol.Index]).Font.Bold = true;
                            }
                        }
                    }
                }

                exApp.Visible = true;
            }
            catch (Exception ex)
            {
                CleanUpExcel();
                throw;
            }
        }

        public void UpdateExistingFile(string sourceFileName, int startRow, Excel.XlTextParsingType sourceType, IExcelCellUpdate[] uodateItems, string targetFileName = null, Excel.XlFileFormat saveAsFormat = Excel.XlFileFormat.xlExcel8)
        {
            if (null != uodateItems && uodateItems.Length > 0)
            {
                try
                {
                    exApp = new ExcelApp() { Visible = false };
                    books = (Excel.Workbooks)exApp.Workbooks;
                    books.OpenText(Filename: sourceFileName, StartRow: startRow, DataType: sourceType, Tab: true);

                    book1 = (Excel._Workbook)(books[1]);

                    xlWorkSheet = (Excel.Worksheet)book1.Worksheets.Item[1];

                    try
                    {
                        cellsRange = (Excel.Range)xlWorkSheet.Columns;

                        foreach (IExcelCellUpdate eaItem in uodateItems)
                        {
                            try
                            {
                                eaItem.Update(cellsRange);
                            }
                            catch (Exception ex)
                            {
                                var msg = ex.Message;
                                continue;
                            }
                        }

                        string fileName = (!string.IsNullOrEmpty(targetFileName)) ? targetFileName : sourceFileName;

                        book1.SaveAs(fileName, saveAsFormat, Type.Missing, Type.Missing, false, false,
                            Excel.XlSaveAsAccessMode.xlExclusive, Excel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing);
                    }
                    finally
                    {
                        CleanUpExcel();
                    }
                }
                finally
                {
                    CleanUpExcel();
                }
            }
        }

        public string[] GetRows(string excelFileName, Excel.XlTextParsingType sourceType, int targetColumn = 0, bool hasHeaderRow = true, bool appVisible = false)
        {
            string[] rowContent = new string[0];

            exApp = new ExcelApp() { Visible = appVisible };
            books = (Excel.Workbooks)exApp.Workbooks;
            int startRow = hasHeaderRow ? 2 : 1;

            try
            {
                books.OpenText(Filename: excelFileName, StartRow: startRow, DataType: sourceType, Tab: true);

                book1 = (Excel._Workbook)(books[1]);

                xlWorkSheet = (Excel.Worksheet)book1.Worksheets.Item[1];

                cellsRange = (targetColumn > 0) 
                    ? (Excel.Range)xlWorkSheet.Columns[targetColumn]
                    : (Excel.Range)xlWorkSheet;

                System.Array myvalues = (System.Array)cellsRange.Cells.Value;
                rowContent = myvalues.OfType<object>().Select(o => o.ToString()).ToArray();
            }
            catch
            {
                if (null != books)
                {
                    books.Close();
                    Marshal.ReleaseComObject(books);
                    books = null;
                }
            }
            finally
            {
                CleanUpExcel();
            }

            return rowContent;
        }

        public void CleanUp()
        {
            CleanUpExcel();
        }

        private void CleanUpExcel()
        {
            if (null != cellsRange)
            {
                Marshal.ReleaseComObject(cellsRange);
                cellsRange = null;
            }

            if (null != xlWorkSheet)
            {
                Marshal.ReleaseComObject(xlWorkSheet);
                xlWorkSheet = null;
            }

            if (null != book1)
            {
                book1.Close(false, Type.Missing, Type.Missing);
                Marshal.ReleaseComObject(book1);
                book1 = null;
            }

            if (null != books)
            {
                books.Close();
                Marshal.ReleaseComObject(books);
                books = null;
            }

            if (null != exApp)
            {
                exApp.Quit();
                exApp = null;
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    CleanUpExcel();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }

    internal class ExcelColumn
    {
        public ExcelColumn()
        {
        }

        public ExcelColumn(int index, int width, bool headerIsBold)
        {
            this.Index = index;
            this.Width = width;
            this.HeaderIsBold = headerIsBold;
        }

        public int Index { get; set; }
        public int Width { get; set; }
        public bool HeaderIsBold { get; set; }

        public bool IsValid()
        {
            return this.Index > 0 && this.Width > 0;
        }
    }

    internal interface IExcelCellUpdate
    {
        int Col { get; set; }
        int Row { get; set; }
        string Text { get; set; }
        CellStyle Style { get; set; }

        void Update(Excel.Range range);
    }

    internal class CellTarget : IExcelCellUpdate
    {
        public CellTarget(string value,int col, int row)
        {
            this.Text = value;
            this.Col = col;
            this.Row = row;
        }

        public int Col { get; set; }
        public int Row { get; set; }
        public string Text { get; set; }
        public CellStyle Style { get; set; }

        public void Update(Excel.Range range)
        {
            var targetCell = range.Cells[this.Row, this.Col];

            if (null != targetCell)
            {
                targetCell.Value = this.Text;

                ExcelHelper.ApplyStyle(new Excel.Range[]{targetCell}, this.Style);
            }
        }
    }

    internal class CellMatch : IExcelCellUpdate
    {
        public CellMatch(string matchValue, string updateValue, int shiftColumn, int shiftRow)
        {
            this.Match = matchValue;
            this.Text = updateValue;
            this.Col = shiftColumn;
            this.Row = shiftRow;
        }

        public int Col { get; set; }
        public int Row { get; set; }
        public string Text { get; set; }
        public string Match { get; set; }
        public CellStyle Style { get; set; }

        public void Update(Excel.Range range)
        {
            var matchedCell = range.Cells.Find(this.Match);

            if (null != matchedCell)
            {
                var targetCell = range.Cells[matchedCell.Row + this.Row, matchedCell.Column + this.Col];

                if (null != targetCell)
                {
                    targetCell.Value = this.Text;

                    ExcelHelper.ApplyStyle(new Excel.Range[]{ matchedCell, targetCell}, this.Style);
                }
            }
        }
    }

    internal class CellStyle
    {
        public string Name { get; set; }
        public bool FontBold { get; set; }
        public System.Drawing.Color ForeColor { get; set; }
        public System.Drawing.Color? BackColor { get; set; }
        public Excel.XlPattern Pattern { get; set; }
    }


    internal class ExcelColumnCollection
    {
        private Dictionary<int, ExcelColumn> _colSet = new Dictionary<int, ExcelColumn>();

        public List<ExcelColumn> Columns { get { return _colSet.Values.ToList(); } }

        public ExcelColumnCollection Add(ExcelColumn col)
        {
            if (!_colSet.ContainsKey(col.Index))
            {
                _colSet.Add(col.Index, col);
            }
            return this;
        }

        public ExcelColumnCollection Remove(ExcelColumn col)
        {
            if (_colSet.ContainsKey(col.Index))
            {
                _colSet.Remove(col.Index);
            }
            return this;
        }
    }
}
