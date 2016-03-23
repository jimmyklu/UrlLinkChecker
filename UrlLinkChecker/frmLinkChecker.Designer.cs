namespace UrlLinkChecker
{
    partial class frmLinkChecker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnFileChooser = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.rtbSourceDoc = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAbort = new System.Windows.Forms.Button();
            this.listResults = new UrlLinkChecker.CustomListView();
            this.btnCheckLinks = new System.Windows.Forms.Button();
            this.progBar1 = new System.Windows.Forms.ProgressBar();
            this.lblChecking = new System.Windows.Forms.Label();
            this.lblCounter = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openThisUrlInDefaultBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMarkAsOK = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveDupes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.copySelectedItemsToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSelectedItemsInExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.copyAllToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllInExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeALLDuplicatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveResultsToSourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnFileChooser);
            this.splitContainer1.Panel1.Controls.Add(this.txtFile);
            this.splitContainer1.Panel1.Controls.Add(this.rtbSourceDoc);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnAbort);
            this.splitContainer1.Panel2.Controls.Add(this.listResults);
            this.splitContainer1.Panel2.Controls.Add(this.btnCheckLinks);
            this.splitContainer1.Panel2.Controls.Add(this.progBar1);
            this.splitContainer1.Panel2.Controls.Add(this.lblChecking);
            this.splitContainer1.Panel2.Controls.Add(this.lblCounter);
            this.splitContainer1.Size = new System.Drawing.Size(1320, 736);
            this.splitContainer1.SplitterDistance = 491;
            this.splitContainer1.SplitterWidth = 8;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnFileChooser
            // 
            this.btnFileChooser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFileChooser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileChooser.Location = new System.Drawing.Point(459, 16);
            this.btnFileChooser.Name = "btnFileChooser";
            this.btnFileChooser.Size = new System.Drawing.Size(28, 20);
            this.btnFileChooser.TabIndex = 7;
            this.btnFileChooser.Text = "...";
            this.btnFileChooser.UseVisualStyleBackColor = true;
            this.btnFileChooser.Click += new System.EventHandler(this.btnFileChooser_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(102, 16);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(359, 20);
            this.txtFile.TabIndex = 6;
            // 
            // rtbSourceDoc
            // 
            this.rtbSourceDoc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbSourceDoc.Location = new System.Drawing.Point(7, 42);
            this.rtbSourceDoc.Name = "rtbSourceDoc";
            this.rtbSourceDoc.Size = new System.Drawing.Size(483, 682);
            this.rtbSourceDoc.TabIndex = 1;
            this.rtbSourceDoc.Text = "";
            this.rtbSourceDoc.TextChanged += new System.EventHandler(this.rtbSourceDoc_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Document:";
            // 
            // btnAbort
            // 
            this.btnAbort.Enabled = false;
            this.btnAbort.Location = new System.Drawing.Point(6, 4);
            this.btnAbort.Name = "btnAbort";
            this.btnAbort.Size = new System.Drawing.Size(76, 26);
            this.btnAbort.TabIndex = 8;
            this.btnAbort.Text = "Cancel";
            this.btnAbort.UseVisualStyleBackColor = true;
            this.btnAbort.Visible = false;
            // 
            // listResults
            // 
            this.listResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listResults.FullRowSelect = true;
            this.listResults.Location = new System.Drawing.Point(6, 32);
            this.listResults.Name = "listResults";
            this.listResults.Size = new System.Drawing.Size(802, 692);
            this.listResults.TabIndex = 5;
            this.listResults.UseCompatibleStateImageBehavior = false;
            // 
            // btnCheckLinks
            // 
            this.btnCheckLinks.Location = new System.Drawing.Point(6, 4);
            this.btnCheckLinks.Name = "btnCheckLinks";
            this.btnCheckLinks.Size = new System.Drawing.Size(76, 26);
            this.btnCheckLinks.TabIndex = 2;
            this.btnCheckLinks.Text = "Check Links";
            this.btnCheckLinks.UseVisualStyleBackColor = true;
            this.btnCheckLinks.Click += new System.EventHandler(this.btnCheckLinks_Click);
            // 
            // progBar1
            // 
            this.progBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progBar1.Location = new System.Drawing.Point(271, 10);
            this.progBar1.Name = "progBar1";
            this.progBar1.Size = new System.Drawing.Size(534, 14);
            this.progBar1.TabIndex = 3;
            this.progBar1.Visible = false;
            // 
            // lblChecking
            // 
            this.lblChecking.AutoSize = true;
            this.lblChecking.Location = new System.Drawing.Point(91, 11);
            this.lblChecking.Name = "lblChecking";
            this.lblChecking.Size = new System.Drawing.Size(86, 13);
            this.lblChecking.TabIndex = 4;
            this.lblChecking.Text = "Checking Links: ";
            // 
            // lblCounter
            // 
            this.lblCounter.AutoSize = true;
            this.lblCounter.Location = new System.Drawing.Point(177, 12);
            this.lblCounter.Name = "lblCounter";
            this.lblCounter.Size = new System.Drawing.Size(13, 13);
            this.lblCounter.TabIndex = 5;
            this.lblCounter.Text = "- ";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openThisUrlInDefaultBrowserToolStripMenuItem,
            this.toolStripMenuItemMarkAsOK,
            this.toolStripMenuItemRemoveDupes,
            this.toolStripSeparator2,
            this.copySelectedItemsToClipboardToolStripMenuItem,
            this.showSelectedItemsInExcelToolStripMenuItem,
            this.toolStripSeparator1,
            this.copyAllToClipboardToolStripMenuItem,
            this.showAllInExcelToolStripMenuItem,
            this.removeALLDuplicatesToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveResultsToSourceFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(254, 242);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // openThisUrlInDefaultBrowserToolStripMenuItem
            // 
            this.openThisUrlInDefaultBrowserToolStripMenuItem.Name = "openThisUrlInDefaultBrowserToolStripMenuItem";
            this.openThisUrlInDefaultBrowserToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.openThisUrlInDefaultBrowserToolStripMenuItem.Text = "Open This Url in Default Browser";
            this.openThisUrlInDefaultBrowserToolStripMenuItem.Click += new System.EventHandler(this.openThisUrlInDefaultBrowserToolStripMenuItem_Click);
            // 
            // toolStripMenuItemMarkAsOK
            // 
            this.toolStripMenuItemMarkAsOK.Name = "toolStripMenuItemMarkAsOK";
            this.toolStripMenuItemMarkAsOK.Size = new System.Drawing.Size(253, 22);
            this.toolStripMenuItemMarkAsOK.Text = "OVERRIDE:  Mark Item as \"Ok\"";
            this.toolStripMenuItemMarkAsOK.Click += new System.EventHandler(this.toolStripMenuItemMarkAsOK_Click);
            // 
            // toolStripMenuItemRemoveDupes
            // 
            this.toolStripMenuItemRemoveDupes.Name = "toolStripMenuItemRemoveDupes";
            this.toolStripMenuItemRemoveDupes.Size = new System.Drawing.Size(253, 22);
            this.toolStripMenuItemRemoveDupes.Text = "Remove Other Duplicates";
            this.toolStripMenuItemRemoveDupes.Click += new System.EventHandler(this.toolStripMenuItemRemoveDupes_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(250, 6);
            // 
            // copySelectedItemsToClipboardToolStripMenuItem
            // 
            this.copySelectedItemsToClipboardToolStripMenuItem.Name = "copySelectedItemsToClipboardToolStripMenuItem";
            this.copySelectedItemsToClipboardToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.copySelectedItemsToClipboardToolStripMenuItem.Text = "Copy Selected Items To Clipboard";
            this.copySelectedItemsToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copySelectedItemsToClipboardToolStripMenuItem_Click);
            // 
            // showSelectedItemsInExcelToolStripMenuItem
            // 
            this.showSelectedItemsInExcelToolStripMenuItem.Name = "showSelectedItemsInExcelToolStripMenuItem";
            this.showSelectedItemsInExcelToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.showSelectedItemsInExcelToolStripMenuItem.Text = "Show Selected Items in Excel";
            this.showSelectedItemsInExcelToolStripMenuItem.Click += new System.EventHandler(this.showSelectedItemsInExcelToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(250, 6);
            // 
            // copyAllToClipboardToolStripMenuItem
            // 
            this.copyAllToClipboardToolStripMenuItem.Name = "copyAllToClipboardToolStripMenuItem";
            this.copyAllToClipboardToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.copyAllToClipboardToolStripMenuItem.Text = "Copy ALL To Clipboard";
            this.copyAllToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyAllToClipboardToolStripMenuItem_Click);
            // 
            // showAllInExcelToolStripMenuItem
            // 
            this.showAllInExcelToolStripMenuItem.Name = "showAllInExcelToolStripMenuItem";
            this.showAllInExcelToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.showAllInExcelToolStripMenuItem.Text = "Show ALL in Excel";
            this.showAllInExcelToolStripMenuItem.Click += new System.EventHandler(this.showAllInExcelToolStripMenuItem_Click);
            // 
            // removeALLDuplicatesToolStripMenuItem
            // 
            this.removeALLDuplicatesToolStripMenuItem.Name = "removeALLDuplicatesToolStripMenuItem";
            this.removeALLDuplicatesToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.removeALLDuplicatesToolStripMenuItem.Text = "Remove ALL Duplicates";
            this.removeALLDuplicatesToolStripMenuItem.Click += new System.EventHandler(this.removeALLDuplicatesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(250, 6);
            // 
            // saveResultsToSourceFileToolStripMenuItem
            // 
            this.saveResultsToSourceFileToolStripMenuItem.Name = "saveResultsToSourceFileToolStripMenuItem";
            this.saveResultsToSourceFileToolStripMenuItem.Size = new System.Drawing.Size(253, 22);
            this.saveResultsToSourceFileToolStripMenuItem.Text = "Save Results to Source File";
            this.saveResultsToSourceFileToolStripMenuItem.Click += new System.EventHandler(this.saveResultsToSourceFileToolStripMenuItem_Click);
            // 
            // frmLinkChecker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1320, 736);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmLinkChecker";
            this.Text = "UrlLinkTester";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCheckLinks;
        private System.Windows.Forms.RichTextBox rtbSourceDoc;
        private System.Windows.Forms.Label label1;
        private CustomListView listResults;
        private System.Windows.Forms.ProgressBar progBar1;
        private System.Windows.Forms.Label lblCounter;
        private System.Windows.Forms.Label lblChecking;
        private System.Windows.Forms.Button btnFileChooser;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copySelectedItemsToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyAllToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllInExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSelectedItemsInExcelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openThisUrlInDefaultBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMarkAsOK;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveDupes;
        private System.Windows.Forms.ToolStripMenuItem removeALLDuplicatesToolStripMenuItem;
        private System.Windows.Forms.Button btnAbort;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveResultsToSourceFileToolStripMenuItem;
    }
}

