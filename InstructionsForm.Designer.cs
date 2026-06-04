using System.ComponentModel;

namespace DQ8SaveEditor;

partial class InstructionsForm
{
    private IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        _lblTitle = new Label();
        _txt = new RichTextBox();
        _bottomPanel = new Panel();
        _btnClose = new Button();
        _bottomPanel.SuspendLayout();
        SuspendLayout();

        // _lblTitle
        _lblTitle.Dock = DockStyle.Top;
        _lblTitle.Name = "_lblTitle";
        _lblTitle.Padding = new Padding(4, 6, 4, 6);
        _lblTitle.Size = new Size(584, 38);
        _lblTitle.TabIndex = 0;
        _lblTitle.Text = "How to Use the DQ8 Save State Editor";
        _lblTitle.TextAlign = ContentAlignment.MiddleLeft;

        // _txt
        _txt.BorderStyle = BorderStyle.None;
        _txt.Dock = DockStyle.Fill;
        _txt.Name = "_txt";
        _txt.ReadOnly = true;
        _txt.ScrollBars = RichTextBoxScrollBars.Vertical;
        _txt.Size = new Size(584, 414);
        _txt.TabIndex = 1;
        _txt.Text = "";
        _txt.DetectUrls = false;

        // _bottomPanel
        _bottomPanel.Controls.Add(_btnClose);
        _bottomPanel.Dock = DockStyle.Bottom;
        _bottomPanel.Name = "_bottomPanel";
        _bottomPanel.Padding = new Padding(0, 8, 12, 8);
        _bottomPanel.Size = new Size(584, 48);
        _bottomPanel.TabIndex = 2;
        // _btnClose
        _btnClose.Anchor = AnchorStyles.Right;
        _btnClose.DialogResult = DialogResult.OK;
        _btnClose.Location = new Point(484, 8);
        _btnClose.Name = "_btnClose";
        _btnClose.Size = new Size(88, 32);
        _btnClose.TabIndex = 0;
        _btnClose.Text = "Close";

        // InstructionsForm
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 500);
        Controls.Add(_txt);
        Controls.Add(_bottomPanel);
        Controls.Add(_lblTitle);
        MinimumSize = new Size(520, 420);
        Name = "InstructionsForm";
        Padding = new Padding(10);
        StartPosition = FormStartPosition.CenterParent;
        Text = "How to Use — DQ8 Save State Editor";

        _bottomPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private Label _lblTitle;
    private RichTextBox _txt;
    private Panel _bottomPanel;
    private Button _btnClose;
}
