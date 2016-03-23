namespace UrlLinkChecker
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using Excel = Microsoft.Office.Interop.Excel;
    using ExcelApp = Microsoft.Office.Interop.Excel.Application;

    using UrlLinkChecker.Internals;
    
    public partial class frmLinkChecker : Form
    {
        public frmLinkChecker()
        {
            InitializeComponent();

            btnCheckLinks.Enabled = false;

            listResults.View = View.Details;

            listResults.Columns.Add(Column1).Width = int.Parse(RscLiterals.ListView_Column1Width);
            listResults.Columns.Add(Column2).Width = int.Parse(RscLiterals.ListView_Column2Width);
            listResults.Columns.Add(Column3).Width = int.Parse(RscLiterals.ListView_Column3Width);

            listResults.MouseClick += listResults_MouseClick;

            listResults.ColumnClick += listResults_ColumnClick;

            openFileDialog1.Multiselect = false;
            openFileDialog1.FileName = RscLiterals.Parse_BadLinksFileName;
            openFileDialog1.FileOk += openFileDialog1_FileOk;

            listResults.ContextMenuStrip = contextMenuStrip1;
        }

        private int sortCol = -1;
        private bool sortAsc = false;

        void listResults_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == sortCol)
            {

                if (!sortAsc)
                {
                    listResults.Sorting = SortOrder.Ascending;
                    sortAsc = true;
                }
                else
                {
                    listResults.Sorting = SortOrder.Descending;
                    sortAsc = false;
                }
            }
            else
            {
                listResults.Sorting = SortOrder.Ascending;
                sortAsc = true;
                sortCol = e.Column;
            }

            listResults.ListViewItemSorter = new ListViewItemComparer(e.Column, listResults.Sorting);

            listResults.Sort();

            listResults.Refresh();

        }

        private ListViewItem rightClickedItem = null;

        void listResults_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point mousePos = listResults.PointToClient(Control.MousePosition);
                ListViewHitTestInfo hitTest = listResults.HitTest(mousePos);
                rightClickedItem = hitTest.Item;
            }
            else
            {
                rightClickedItem = null;
            }
        }


        void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string fileName = openFileDialog1.FileName;
            txtFile.Text = fileName;

            if (!string.IsNullOrEmpty(fileName))
            {
                badLinksFile = fileName;
                string[] links = ParseBadLinksUsingExcel();
                rtbSourceDoc.Lines = links;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += " - v" + version;
            listResults.Items.Clear();

            int groupCount = Properties.Settings.Default.LinkSplitCount;

            SplitGroupCount = (groupCount > 0) ? groupCount : DefaultSplitGroupCount;
        }

        private Color fgDefault = Color.Black;
        private Color fgOk = Color.Green;
        private Color fgFail = Color.Red;

        private Color bgNormal = Color.White;
        private Color bgDupe = Color.LightGray;

        private const int newItemsBatchSize = 50;

        private static readonly object lockObj = new object();

        private static List<ListViewItem> runningItems = new List<ListViewItem>();

        private void AddResult(string url, UrlResult result)
        {
            listResults.Invoke((MethodInvoker)delegate
            {
                Color fontColor = result.Success ? fgOk : fgFail;
                Color backColor = result.IsDuplicate ? bgDupe : bgNormal;

                string resultText = result.Success ? UrlResult.ResultOk : UrlResult.ResultFail;
                var item = new ListViewItem(new string[] { url, resultText, result.Error });
                item.ForeColor = fontColor;
                item.BackColor = backColor;

                lock (lockObj)
                {
                    runningItems.Add(item);
                }

                if (runningItems.Count >= newItemsBatchSize)
                {
                    lock (lockObj)
                    {
                        listResults.Items.AddRange(runningItems.ToArray());
                        runningItems.Clear();
                        listResults.Refresh();
                    }
                }

                UpdateProgbar();
            });
        }

        private const int MaxLinks = 100;

        private static readonly string Column1 = RscLiterals.ListView_Column1Label;
        private static readonly  string Column2 = RscLiterals.ListView_Column2Label;
        private static readonly  string Column3 = RscLiterals.ListView_Column3Label;

        static int counter = 0;
        static int linksFound = 0;

        private static readonly string DefaultTextSeparator =  RscLiterals.Parse_SourceFileLineSplitter;

        private string _textSeparator; 
        private string TextSeparator
        {
            get { return !string.IsNullOrEmpty(_textSeparator) ? _textSeparator : DefaultTextSeparator; }
            set { _textSeparator = value; }
        }

        private void btnCheckLinks_Click(object sender, EventArgs e)
        {
            _urlResults.Clear();
            ParseText_Delimited();
        }

        private void Alert_NoLinks()
        {
            MessageBox.Show(RscLiterals.Parse_NoLinksWarning, RscLiterals.Parse_NoLinksLabel, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private const int DefaultSplitGroupCount = 200;

        private int SplitGroupCount { get; set; }

        private static int threadCount = 0;

        private static IComparer resultSorter = null;

        private void CheckPlainTextLinks(string[] urlArray)
        {
            int i = 0;

            cancelRequested = false;
            threadCount = 0;

            linksFound = urlArray.Length;
            counter = 0;
            progBar1.Maximum = linksFound;

            listResults.DelayRefresh();

            if (linksFound < SplitGroupCount)
            {
                SplitGroupCount = (int)(linksFound / 4) + 1;
            }

            var splits = from item in urlArray
                         group item by i++ % SplitGroupCount into part
                         select part.AsEnumerable();

            resultSorter = listResults.ListViewItemSorter;
            listResults.ListViewItemSorter = null;

            foreach (var eaSplit in splits)
            {
                threadCount++;
                Task.Factory.StartNew(() => CheckLinks_Async(eaSplit));
            }

        }

        private static bool cancelRequested = false;

        private void CheckLinks_Async(IEnumerable<string> urlArray)
        {
            using (var client = new CustomWebClient())
            {
                client.HeadOnly = true;
                client.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);

                bool aborted = false;

                foreach (string eaUrl in urlArray)
                {
                    if (!cancelRequested)
                    {
                        if (!string.IsNullOrEmpty(eaUrl))
                        {
                            UrlResult linkResult = CheckLink(client, eaUrl);
                            counter++;
                            UpdateCounter();
                            AddResult(eaUrl, linkResult);
                            Thread.Sleep(10);
                        }
                    }
                    else
                    {
                        aborted = true;
                        threadCount--;
                        break;
                    }
                }

                if (!aborted)
                {
                    threadCount--;
                }
            }
        }


        private string badLinksFile = string.Empty;

        private string[] ParseBadLinksUsingExcel(int targetColumn = 1, bool hasHeaderRow = true)
        {
            string[] links = null;

            using(var helper = new ExcelHelper())
            {
                links = helper.GetRows(badLinksFile, Excel.XlTextParsingType.xlDelimited, targetColumn, hasHeaderRow, false);
            }

            return links;
        }

        private void ParseText_Delimited()
        {
            try
            {
                string[] urlLinks = rtbSourceDoc.Lines.SelectMany(ln => ln.Split("\t".ToCharArray()).Take(1)).ToArray();
                linksFound = urlLinks.Length;

                if (linksFound > 0)
                {
                    DialogResult conf = (linksFound > MaxLinks)
                        ? MessageBox.Show(string.Format(RscLiterals.Parse_LinkVolumeWarning, linksFound), RscLiterals.Parse_LinkVolumeLabel, MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        : DialogResult.Yes;

                    if (conf == DialogResult.Yes)
                    {
                        InitUIElements_Start();

                        CheckPlainTextLinks(urlLinks);
                    }
                }
                else
                {
                    Alert_NoLinks();
                }
            }
            catch
            {
                MessageBox.Show(RscLiterals.Error_BadHtml);
            }
        }


        private void InitUIElements_Start()
        {
            progBar1.Invoke((MethodInvoker)delegate
            {
                rtbSourceDoc.Enabled = false;
                listResults.Items.Clear();
                
                btnCheckLinks.Enabled = false;
                btnCheckLinks.Visible = false;

                this.Cursor = Cursors.WaitCursor;
                counter = 0;

                btnAbort.Enabled = true;
                btnAbort.Visible = true;

                progBar1.Visible = true;
                progBar1.Value = 0;
                progBar1.Maximum = linksFound;
            });
        }

        private void InitUIElements_End()
        {
            progBar1.Invoke((MethodInvoker)delegate
            {
                progBar1.Visible = false;
                rtbSourceDoc.Enabled = true;

                btnCheckLinks.Enabled = true;
                btnCheckLinks.Visible = true;

                this.Cursor = Cursors.Default;
                lblCounter.Text = RscLiterals.CounterLabel_Default;

                btnAbort.Enabled = false;
                btnAbort.Visible = false;

                if (runningItems.Count > 0)
                {
                    lock (lockObj)
                    {
                        listResults.Items.AddRange(runningItems.ToArray());
                        listResults.Refresh();
                    }
                }

                listResults.ListViewItemSorter = resultSorter;

                listResults.Refresh();

                runningItems.Clear();
            });
        }

        private void UpdateCounter()
        {
            lblCounter.Invoke((MethodInvoker)delegate
            {
                lblCounter.Text = string.Format(RscLiterals.CounterLabel_RunningFormat, counter, linksFound);
            });
        }

        private void UpdateProgbar()
        {
            progBar1.Invoke((MethodInvoker)delegate
            {
                progBar1.Increment(1);
                if (progBar1.Value == progBar1.Maximum)
                {
                    InitUIElements_End();
                }
            });
        }

        private UrlResult CheckLink(CustomWebClient client, string url)
        {
            if (!url.StartsWith(RscLiterals.Link_ProtocolPrefix))
            {
                url = RscLiterals.Link_ProtocolPrefix + "://" + url;
            }

            if (_urlResults.ContainsKey(url))
            {
                var dupResult = _urlResults[url].Clone();
                dupResult.IsDuplicate = true;
                return dupResult;
            }
            else
            {
                int redirCount = 0;

                string clientResult = string.Empty;
                try
                {

                    if (Properties.Settings.Default.WebClientTestHeadersOnly)
                    {
                        clientResult = client.DownloadString(url);
                    }
                    else
                    {
                        clientResult = client.GetUrl(url, out redirCount);
                    }
                }
                catch (Exception ex)
                {
                    clientResult = ex.Message;
                }

                var result = new UrlResult(string.IsNullOrEmpty(clientResult), clientResult, redirCount);

                if (!_urlResults.ContainsKey(url))
                {
                    _urlResults.Add(url, result);
                }

                return result;
            }
        }

        private void rtbSourceDoc_TextChanged(object sender, EventArgs e)
        {
            btnCheckLinks.Enabled = !string.IsNullOrEmpty(rtbSourceDoc.Text);
        }

        private void btnFileChooser_Click(object sender, EventArgs e)
        {
            badLinksFile = string.Empty;
            openFileDialog1.ShowDialog();
        }

        private void CopyAll()
        {
            CopyItemsToClipboard(listResults.Items);
        }

        private void CopySelected()
        {
            CopyItemsToClipboard(listResults.SelectedItems);
        }

        private void CopyItemsToClipboard(IEnumerable listViewItems)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Column1 + TextSeparator + Column2 + TextSeparator + Column3 + Environment.NewLine);
            foreach (object eaItem in listViewItems)
            {
                sb.Append(FormatListItem((ListViewItem)eaItem) + Environment.NewLine);
            }
            Clipboard.SetText(sb.ToString());
        }


        private string FormatListItem(ListViewItem item)
        {
            return item.SubItems[0].Text + TextSeparator + item.SubItems[1].Text + TextSeparator + item.SubItems[2].Text;
        }

        private void MarKItemAsOK(ListViewItem item)
        {
            item.ForeColor = fgOk;
            item.SubItems[1].Text = UrlResult.ResultOk;
            item.SubItems[2].Text = string.Empty;
        }

        private void RemoveDuplicates(ListViewItem item)
        {
            string url = item.SubItems[0].Text;

            int thisIndex = item.Index;

            var indices = new List<int>();

            for (int i = 0; i < listResults.Items.Count; i++ )
            {
                var dupeItem = listResults.FindItemWithText(url, true, i, false);

                if (null != dupeItem && dupeItem.Index != thisIndex && !indices.Contains(dupeItem.Index))
                {
                    indices.Add(dupeItem.Index);
                }
            }

            if (indices.Count > 0)
            {
                indices.Sort();
                indices.Reverse();
                foreach(int x in indices)
                {
                    listResults.Items[x].Remove();
                }
            }

            item.BackColor = bgNormal;
        }
        
        
        private void OpenSelectedInExcel()
        {
            CopySelected();

            OpenClipboardInExcel();
        }

        private void OpenAllInExcel()
        {
            CopyAll();

            OpenClipboardInExcel();
        }


        private void OpenClipboardInExcel()
        {
            using (var helper = new ExcelHelper())
            {
                try
                {
                    var columns = new ExcelColumnCollection()
                        .Add(new ExcelColumn(1, 100, true))
                        .Add(new ExcelColumn(2, 20, true))
                        .Add(new ExcelColumn(3, 100, true));

                    helper.OpenFromClipboard(columns);
                }
                catch
                {
                    helper.CleanUp();
                    MessageBox.Show(RscLiterals.Error_ExcelClipboad);
                }
            }
        }

       
        private void copySelectedItemsToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopySelected();
        }

        private void copyAllToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyAll();
        }

        private void showAllInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenAllInExcel();
        }

        private void showSelectedItemsInExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSelectedInExcel();
        }

        private Dictionary<string, UrlResult> _urlResults = new Dictionary<string, UrlResult>();

        private static readonly CellStyle errStyle = new CellStyle() { Pattern = Excel.XlPattern.xlPatternSolid, ForeColor = Color.Red, FontBold = true, Name = "ErrorStyle" };

        private void SaveResultsToFile()
        {
            int rowCount = listResults.Items.Count;

            if (!string.IsNullOrEmpty(badLinksFile) && rowCount > 0)
            {
                bool saveComplete = false;
                try
                {
                    string dateVal = DateTime.Now.ToString("yyyyMMddhhmm");
                    string newFileName = badLinksFile.Replace(".txt", "_results_" + dateVal + ".xls");

                    List<CellMatch> updates = new List<CellMatch>();

                    var errorItems = listResults.Items.Cast<ListViewItem>().Where(i => !string.IsNullOrEmpty(i.SubItems[2].Text)); 
                    foreach (ListViewItem eaItem in errorItems)
                    {
                        var url = eaItem.SubItems[0].Text.Trim();
                        var err = eaItem.SubItems[1].Text.ToUpper() + " : " + eaItem.SubItems[2].Text;

                        if (!string.IsNullOrEmpty(url))
                        {
                            updates.Add(new CellMatch(url, err, 1, 0) { Style = errStyle });
                        }
                    }

                    if (updates.Count > 0)
                    {
                        using (var helper = new ExcelHelper())
                        {
                            helper.UpdateExistingFile(badLinksFile, 1, Excel.XlTextParsingType.xlDelimited, updates.ToArray(), newFileName, Excel.XlFileFormat.xlExcel8);
                            saveComplete = true;
                            helper.CleanUp();
                        }
                    }
                }
                catch
                {
                    MessageBox.Show(RscLiterals.Error_ExcelClipboad);
                }

                if (saveComplete)
                {
                    string fileLocation = Directory.GetParent(badLinksFile).FullName;
                    Process.Start(fileLocation);
                }
            }
        }

        private void btnSaveResults_Click(object sender, EventArgs e)
        {
            SaveResultsToFile();
        }

        void frmLinkChecker_Disposed(object sender, System.EventArgs e)
        {
            KillTasks();
            _urlResults.Clear();
        }

        void frmLinkChecker_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            KillTasks();
            _urlResults.Clear();
        }

        private void openThisUrlInDefaultBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (null != rightClickedItem)
            {
                string urlString = rightClickedItem.SubItems[0].Text;
                if (!string.IsNullOrEmpty(urlString))
                {
                    if (Uri.IsWellFormedUriString(urlString, UriKind.Absolute))
                    {
                        Process.Start(urlString);
                    }
                    else
                    {
                        MessageBox.Show("Invalid url selected");
                    }
                }
            }
        }

        private void KillTasks()
        {
            cancelRequested = true;
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            KillTasks();

            while (threadCount > 0)
            {
                Thread.Sleep(1000);
            }

            this.Invoke((MethodInvoker)delegate
            {
                InitUIElements_End();
            });
        }

        private void toolStripMenuItemMarkAsOK_Click(object sender, EventArgs e)
        {
            MarKItemAsOK(listResults.SelectedItems[0]);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            openThisUrlInDefaultBrowserToolStripMenuItem.Enabled = (listResults.SelectedItems.Count == 1);
            toolStripMenuItemMarkAsOK.Enabled = (listResults.SelectedItems.Count == 1 && listResults.SelectedItems[0].ForeColor == fgFail);
            toolStripMenuItemRemoveDupes.Enabled = (listResults.SelectedItems.Count == 1 && listResults.SelectedItems[0].BackColor == bgDupe);
        }

        private void toolStripMenuItemRemoveDupes_Click(object sender, EventArgs e)
        {
            RemoveDuplicates(listResults.SelectedItems[0]);
        }

        private void removeALLDuplicatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dupeItems = listResults.Items.Cast<ListViewItem>().Where(i => i.BackColor == bgDupe);

            dupeItems = dupeItems.Distinct(new UrlComparer());

            foreach(ListViewItem eaDupe in dupeItems)
            {
                RemoveDuplicates(eaDupe);
            }
        }

        private void saveResultsToSourceFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveResultsToFile();
        }

        public void CleanUp()
        {
            KillTasks();
        }
    }

}
