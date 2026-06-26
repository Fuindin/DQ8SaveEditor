using System.ComponentModel;

namespace DQ8SaveEditor;

partial class MainForm
{
    /// <summary>Required designer variable.</summary>
    private IContainer components = null;

    /// <summary>Clean up any resources being used.</summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support — the visual layout/structure of the form.
    /// Theme colours, fonts, owner-draw and the dynamically generated stat rows are
    /// applied in MainForm.cs (ApplyTheme/WireEvents/BuildFieldEditors) at run time.
    /// </summary>
    private void InitializeComponent()
    {
        _menu = new MenuStrip();
        _mnuFile = new ToolStripMenuItem();
        _mnuOpen = new ToolStripMenuItem();
        _mnuSaveAs = new ToolStripMenuItem();
        _mnuSep = new ToolStripSeparator();
        _mnuExit = new ToolStripMenuItem();
        _mnuHelp = new ToolStripMenuItem();
        _mnuInstructions = new ToolStripMenuItem();
        _mnuHelpSep = new ToolStripSeparator();
        _mnuAbout = new ToolStripMenuItem();
        _header = new TableLayoutPanel();
        _lblFile = new Label();
        _lblSerial = new Label();
        _lblBanner = new Label();
        _goldPanel = new FlowLayoutPanel();
        _lblGold = new Label();
        _numGold = new NumericUpDown();
        _btnSave = new Button();
        _leftPanel = new Panel();
        _lstChars = new ListBox();
        _lblCharacters = new Label();
        _divider = new Panel();
        _rightPanel = new Panel();
        _tabs = new TabControl();
        _pageAttr = new TabPage();
        _fieldTable = new TableLayoutPanel();
        _portraitFrame = new Panel();
        _portrait = new PictureBox();
        _pageItems = new TabPage();
        _carriedList = new ListBox();
        _lblCarried = new Label();
        _equipTable = new TableLayoutPanel();
        _pageBag = new TabPage();
        _bagList = new ListBox();
        _txtName = new TextBox();
        _menu.SuspendLayout();
        _header.SuspendLayout();
        _goldPanel.SuspendLayout();
        ((ISupportInitialize)_numGold).BeginInit();
        _leftPanel.SuspendLayout();
        _rightPanel.SuspendLayout();
        _tabs.SuspendLayout();
        _pageAttr.SuspendLayout();
        _pageItems.SuspendLayout();
        _pageBag.SuspendLayout();
        _portraitFrame.SuspendLayout();
        ((ISupportInitialize)_portrait).BeginInit();
        SuspendLayout();
        // 
        // _menu
        // 
        _menu.ImageScalingSize = new Size(24, 24);
        _menu.Items.AddRange(new ToolStripItem[] { _mnuFile, _mnuHelp });
        _menu.Location = new Point(10, 10);
        _menu.Name = "_menu";
        _menu.Size = new Size(1279, 33);
        _menu.TabIndex = 0;
        // 
        // _mnuFile
        // 
        _mnuFile.DropDownItems.AddRange(new ToolStripItem[] { _mnuOpen, _mnuSaveAs, _mnuSep, _mnuExit });
        _mnuFile.Name = "_mnuFile";
        _mnuFile.Size = new Size(54, 29);
        _mnuFile.Text = "&File";
        // 
        // _mnuOpen
        // 
        _mnuOpen.Name = "_mnuOpen";
        _mnuOpen.ShortcutKeys = Keys.Control | Keys.O;
        _mnuOpen.Size = new Size(341, 34);
        _mnuOpen.Text = "&Open save state…";
        // 
        // _mnuSaveAs
        // 
        _mnuSaveAs.Name = "_mnuSaveAs";
        _mnuSaveAs.ShortcutKeys = Keys.Control | Keys.S;
        _mnuSaveAs.Size = new Size(341, 34);
        _mnuSaveAs.Text = "Save &As (new copy)…";
        // 
        // _mnuSep
        // 
        _mnuSep.Name = "_mnuSep";
        _mnuSep.Size = new Size(338, 6);
        // 
        // _mnuExit
        // 
        _mnuExit.Name = "_mnuExit";
        _mnuExit.Size = new Size(341, 34);
        _mnuExit.Text = "E&xit";
        // 
        // _mnuHelp
        // 
        _mnuHelp.DropDownItems.AddRange(new ToolStripItem[] { _mnuInstructions, _mnuHelpSep, _mnuAbout });
        _mnuHelp.Name = "_mnuHelp";
        _mnuHelp.Size = new Size(65, 29);
        _mnuHelp.Text = "&Help";
        // 
        // _mnuInstructions
        // 
        _mnuInstructions.Name = "_mnuInstructions";
        _mnuInstructions.ShortcutKeys = Keys.F1;
        _mnuInstructions.Size = new Size(251, 34);
        _mnuInstructions.Text = "&How to Use…";
        // 
        // _mnuHelpSep
        // 
        _mnuHelpSep.Name = "_mnuHelpSep";
        _mnuHelpSep.Size = new Size(248, 6);
        // 
        // _mnuAbout
        // 
        _mnuAbout.Name = "_mnuAbout";
        _mnuAbout.Size = new Size(251, 34);
        _mnuAbout.Text = "&About";
        // 
        // _header
        // 
        _header.ColumnCount = 1;
        _header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        _header.Controls.Add(_lblFile, 0, 0);
        _header.Controls.Add(_lblSerial, 0, 1);
        _header.Controls.Add(_lblBanner, 0, 2);
        _header.Dock = DockStyle.Top;
        _header.Location = new Point(10, 43);
        _header.Name = "_header";
        _header.Padding = new Padding(10, 6, 10, 4);
        _header.RowCount = 3;
        _header.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
        _header.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
        _header.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        _header.Size = new Size(1279, 82);
        _header.TabIndex = 4;
        // 
        // _lblFile
        // 
        _lblFile.AutoEllipsis = true;
        _lblFile.Dock = DockStyle.Fill;
        _lblFile.Location = new Point(13, 6);
        _lblFile.Name = "_lblFile";
        _lblFile.Size = new Size(1253, 22);
        _lblFile.TabIndex = 0;
        _lblFile.Text = "No save state loaded.";
        // 
        // _lblSerial
        // 
        _lblSerial.Dock = DockStyle.Fill;
        _lblSerial.Location = new Point(13, 28);
        _lblSerial.Name = "_lblSerial";
        _lblSerial.Size = new Size(1253, 20);
        _lblSerial.TabIndex = 1;
        // 
        // _lblBanner
        // 
        _lblBanner.Dock = DockStyle.Fill;
        _lblBanner.Location = new Point(13, 48);
        _lblBanner.Name = "_lblBanner";
        _lblBanner.Padding = new Padding(8, 4, 8, 4);
        _lblBanner.Size = new Size(1253, 30);
        _lblBanner.TabIndex = 2;
        _lblBanner.TextAlign = ContentAlignment.MiddleLeft;
        _lblBanner.Visible = false;
        // 
        // _goldPanel
        // 
        _goldPanel.Controls.Add(_lblGold);
        _goldPanel.Controls.Add(_numGold);
        _goldPanel.Controls.Add(_btnSave);
        _goldPanel.Dock = DockStyle.Bottom;
        _goldPanel.Location = new Point(10, 977);
        _goldPanel.Name = "_goldPanel";
        _goldPanel.Padding = new Padding(12, 10, 12, 8);
        _goldPanel.Size = new Size(1279, 54);
        _goldPanel.TabIndex = 3;
        // 
        // _lblGold
        // 
        _lblGold.AutoSize = true;
        _lblGold.Location = new Point(12, 19);
        _lblGold.Margin = new Padding(0, 9, 6, 0);
        _lblGold.Name = "_lblGold";
        _lblGold.Size = new Size(50, 25);
        _lblGold.TabIndex = 0;
        _lblGold.Text = "Gold";
        // 
        // _numGold
        // 
        _numGold.Location = new Point(71, 13);
        _numGold.Maximum = new decimal(new int[] { 9999999, 0, 0, 0 });
        _numGold.Name = "_numGold";
        _numGold.Size = new Size(130, 31);
        _numGold.TabIndex = 1;
        _numGold.ThousandsSeparator = true;
        // 
        // _btnSave
        // 
        _btnSave.Location = new Point(252, 12);
        _btnSave.Margin = new Padding(48, 2, 0, 0);
        _btnSave.Name = "_btnSave";
        _btnSave.Size = new Size(170, 34);
        _btnSave.TabIndex = 2;
        _btnSave.Text = "Save As (new copy)…";
        // 
        // _leftPanel
        // 
        _leftPanel.Controls.Add(_lstChars);
        _leftPanel.Controls.Add(_lblCharacters);
        _leftPanel.Dock = DockStyle.Left;
        _leftPanel.Location = new Point(10, 125);
        _leftPanel.Name = "_leftPanel";
        _leftPanel.Padding = new Padding(12, 8, 6, 8);
        _leftPanel.Size = new Size(210, 852);
        _leftPanel.TabIndex = 2;
        // 
        // _lstChars
        // 
        _lstChars.BorderStyle = BorderStyle.None;
        _lstChars.Dock = DockStyle.Fill;
        _lstChars.DrawMode = DrawMode.OwnerDrawFixed;
        _lstChars.IntegralHeight = false;
        _lstChars.ItemHeight = 30;
        _lstChars.Location = new Point(12, 32);
        _lstChars.Name = "_lstChars";
        _lstChars.Size = new Size(192, 812);
        _lstChars.TabIndex = 1;
        // 
        // _lblCharacters
        // 
        _lblCharacters.Dock = DockStyle.Top;
        _lblCharacters.Location = new Point(12, 8);
        _lblCharacters.Name = "_lblCharacters";
        _lblCharacters.Size = new Size(192, 24);
        _lblCharacters.TabIndex = 0;
        _lblCharacters.Text = "Characters";
        // 
        // _divider
        // 
        _divider.Dock = DockStyle.Left;
        _divider.Location = new Point(220, 125);
        _divider.Name = "_divider";
        _divider.Size = new Size(2, 852);
        _divider.TabIndex = 1;
        // 
        // _rightPanel
        // 
        _rightPanel.Controls.Add(_tabs);
        _rightPanel.Controls.Add(_txtName);
        _rightPanel.Dock = DockStyle.Fill;
        _rightPanel.Location = new Point(222, 125);
        _rightPanel.Name = "_rightPanel";
        _rightPanel.Padding = new Padding(14, 8, 16, 8);
        _rightPanel.Size = new Size(1067, 852);
        _rightPanel.TabIndex = 0;
        // 
        // _tabs
        // 
        _tabs.Controls.Add(_pageAttr);
        _tabs.Controls.Add(_pageItems);
        _tabs.Controls.Add(_pageBag);
        _tabs.Dock = DockStyle.Fill;
        _tabs.DrawMode = TabDrawMode.OwnerDrawFixed;
        _tabs.ItemSize = new Size(120, 28);
        _tabs.Location = new Point(14, 32);
        _tabs.Name = "_tabs";
        _tabs.SelectedIndex = 0;
        _tabs.Size = new Size(1037, 812);
        _tabs.SizeMode = TabSizeMode.Fixed;
        _tabs.TabIndex = 1;
        // 
        // _pageAttr
        // 
        _pageAttr.AutoScroll = true;
        _pageAttr.Controls.Add(_portraitFrame);
        _pageAttr.Controls.Add(_fieldTable);
        _pageAttr.Location = new Point(4, 32);
        _pageAttr.Name = "_pageAttr";
        _pageAttr.Padding = new Padding(6);
        _pageAttr.Size = new Size(1029, 776);
        _pageAttr.TabIndex = 0;
        _pageAttr.Text = "Attributes";
        //
        // _portraitFrame  (framed character portrait, top-right of the Attributes page)
        //
        _portraitFrame.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _portraitFrame.Controls.Add(_portrait);
        _portraitFrame.Location = new Point(835, 16);
        _portraitFrame.Name = "_portraitFrame";
        _portraitFrame.Padding = new Padding(4);
        _portraitFrame.Size = new Size(176, 182);
        _portraitFrame.TabIndex = 1;
        //
        // _portrait
        //
        _portrait.Dock = DockStyle.Fill;
        _portrait.Name = "_portrait";
        _portrait.SizeMode = PictureBoxSizeMode.Zoom;
        _portrait.TabIndex = 0;
        _portrait.TabStop = false;
        // 
        // _fieldTable
        // 
        _fieldTable.AutoSize = true;
        _fieldTable.ColumnCount = 2;
        _fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
        _fieldTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
        _fieldTable.Dock = DockStyle.Top;
        _fieldTable.Location = new Point(6, 6);
        _fieldTable.Name = "_fieldTable";
        _fieldTable.Padding = new Padding(6, 10, 0, 0);
        _fieldTable.Size = new Size(1017, 10);
        _fieldTable.TabIndex = 0;
        // 
        // _pageItems
        // 
        _pageItems.Controls.Add(_carriedList);
        _pageItems.Controls.Add(_lblCarried);
        _pageItems.Controls.Add(_equipTable);
        _pageItems.Location = new Point(4, 32);
        _pageItems.Name = "_pageItems";
        _pageItems.Padding = new Padding(6);
        _pageItems.Size = new Size(1029, 776);
        _pageItems.TabIndex = 1;
        _pageItems.Text = "Items";
        // 
        // _carriedList
        // 
        _carriedList.BorderStyle = BorderStyle.None;
        _carriedList.Dock = DockStyle.Fill;
        _carriedList.DrawMode = DrawMode.OwnerDrawFixed;
        _carriedList.IntegralHeight = false;
        _carriedList.Location = new Point(6, 44);
        _carriedList.Name = "_carriedList";
        _carriedList.SelectionMode = SelectionMode.None;
        _carriedList.Size = new Size(1017, 726);
        _carriedList.TabIndex = 2;
        // 
        // _lblCarried
        // 
        _lblCarried.Dock = DockStyle.Top;
        _lblCarried.Location = new Point(6, 22);
        _lblCarried.Name = "_lblCarried";
        _lblCarried.Padding = new Padding(6, 2, 0, 0);
        _lblCarried.Size = new Size(1017, 22);
        _lblCarried.TabIndex = 1;
        _lblCarried.Text = "Other items carried";
        // 
        // _equipTable
        // 
        _equipTable.AutoSize = true;
        _equipTable.ColumnCount = 2;
        _equipTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        _equipTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 230F));
        _equipTable.Dock = DockStyle.Top;
        _equipTable.Location = new Point(6, 6);
        _equipTable.Name = "_equipTable";
        _equipTable.Padding = new Padding(6, 8, 0, 8);
        _equipTable.Size = new Size(1017, 16);
        _equipTable.TabIndex = 0;
        // 
        // _pageBag
        // 
        _pageBag.Controls.Add(_bagList);
        _pageBag.Location = new Point(4, 32);
        _pageBag.Name = "_pageBag";
        _pageBag.Padding = new Padding(6);
        _pageBag.Size = new Size(1029, 776);
        _pageBag.TabIndex = 2;
        _pageBag.Text = "Party Bag";
        // 
        // _bagList
        // 
        _bagList.BorderStyle = BorderStyle.None;
        _bagList.Dock = DockStyle.Fill;
        _bagList.DrawMode = DrawMode.OwnerDrawFixed;
        _bagList.IntegralHeight = false;
        _bagList.Location = new Point(6, 6);
        _bagList.Name = "_bagList";
        _bagList.SelectionMode = SelectionMode.None;
        _bagList.Size = new Size(1017, 764);
        _bagList.TabIndex = 0;
        // 
        // _txtName
        // 
        _txtName.BorderStyle = BorderStyle.None;
        _txtName.Dock = DockStyle.Top;
        _txtName.Location = new Point(14, 8);
        _txtName.Name = "_txtName";
        _txtName.ReadOnly = true;
        _txtName.Size = new Size(1037, 24);
        _txtName.TabIndex = 0;
        _txtName.TabStop = false;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1299, 1041);
        Controls.Add(_rightPanel);
        Controls.Add(_divider);
        Controls.Add(_leftPanel);
        Controls.Add(_goldPanel);
        Controls.Add(_header);
        Controls.Add(_menu);
        MainMenuStrip = _menu;
        MinimumSize = new Size(760, 700);
        Name = "MainForm";
        Padding = new Padding(10);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "DQ8 Save State Editor";
        _menu.ResumeLayout(false);
        _menu.PerformLayout();
        _header.ResumeLayout(false);
        _goldPanel.ResumeLayout(false);
        _goldPanel.PerformLayout();
        ((ISupportInitialize)_numGold).EndInit();
        _leftPanel.ResumeLayout(false);
        _rightPanel.ResumeLayout(false);
        _rightPanel.PerformLayout();
        _tabs.ResumeLayout(false);
        _pageAttr.ResumeLayout(false);
        _pageAttr.PerformLayout();
        _pageItems.ResumeLayout(false);
        _pageItems.PerformLayout();
        _pageBag.ResumeLayout(false);
        _portraitFrame.ResumeLayout(false);
        ((ISupportInitialize)_portrait).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip _menu;
    private ToolStripMenuItem _mnuFile;
    private ToolStripMenuItem _mnuOpen;
    private ToolStripMenuItem _mnuSaveAs;
    private ToolStripSeparator _mnuSep;
    private ToolStripMenuItem _mnuExit;
    private ToolStripMenuItem _mnuHelp;
    private ToolStripMenuItem _mnuInstructions;
    private ToolStripSeparator _mnuHelpSep;
    private ToolStripMenuItem _mnuAbout;
    private TableLayoutPanel _header;
    private Label _lblFile;
    private Label _lblSerial;
    private Label _lblBanner;
    private FlowLayoutPanel _goldPanel;
    private Label _lblGold;
    private NumericUpDown _numGold;
    private Button _btnSave;
    private Panel _leftPanel;
    private Label _lblCharacters;
    private ListBox _lstChars;
    private Panel _divider;
    private Panel _rightPanel;
    private TextBox _txtName;
    private TabControl _tabs;
    private TabPage _pageAttr;
    private TableLayoutPanel _fieldTable;
    private Panel _portraitFrame;
    private PictureBox _portrait;
    private TabPage _pageItems;
    private TableLayoutPanel _equipTable;
    private Label _lblCarried;
    private ListBox _carriedList;
    private TabPage _pageBag;
    private ListBox _bagList;
}
