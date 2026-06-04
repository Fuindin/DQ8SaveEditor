namespace DQ8SaveEditor;

public sealed partial class InstructionsForm : Form
{
    public InstructionsForm()
    {
        InitializeComponent();
        ApplyTheme();
        BuildContent();

        AcceptButton = _btnClose;
        _btnClose.Click += (_, _) => Close();
    }

    private void ApplyTheme()
    {
        BackColor = Theme.WoodDark;
        Font = Theme.Body(9.75f);

        _lblTitle.BackColor = Theme.Parchment;
        _lblTitle.ForeColor = Theme.GoldDeep;
        _lblTitle.Font = Theme.Body(14f, FontStyle.Bold);

        _txt.BackColor = Theme.Parchment;
        _txt.ForeColor = Theme.Ink;
        _txt.Font = Theme.Body(10.5f);

        _bottomPanel.BackColor = Theme.Parchment;

        _btnClose.FlatStyle = FlatStyle.Flat;
        _btnClose.BackColor = Theme.WoodMid;
        _btnClose.ForeColor = Theme.Parchment;
        _btnClose.Font = Theme.Body(10f, FontStyle.Bold);
        _btnClose.FlatAppearance.BorderColor = Theme.GoldDeep;
        _btnClose.FlatAppearance.BorderSize = 2;
        _btnClose.Cursor = Cursors.Hand;
    }

    private void BuildContent()
    {
        var head = Theme.Body(11.5f, FontStyle.Bold);
        var body = Theme.Body(10.5f);

        void H(string s) { _txt.SelectionFont = head; _txt.SelectionColor = Theme.GoldDeep; _txt.AppendText(s + "\n"); }
        void B(string s) { _txt.SelectionFont = body; _txt.SelectionColor = Theme.Ink; _txt.AppendText(s + "\n\n"); }

        B("This tool edits PCSX2 save states (.p2s) for Dragon Quest VIII: Journey of the Cursed King (NTSC-U, SLUS-212.07). It never changes your original file — edits are always written to a new copy.");

        H("1.  Open a save state");
        B("• File ▸ Open save state…  (Ctrl+O), or drag a .p2s file onto the program.\n"
        + "• The header shows the detected game, how many party members were found, and a warning if anything looks unexpected.");

        H("2.  Pick a character");
        B("The Characters list shows the members currently in your party. Click one to view and edit their details on the right.");

        H("3.  Attributes tab  (editable)");
        B("Edit Level, Experience, current/maximum HP and MP, and the four attributes (Strength, Agility, Resilience, Wisdom). Values are kept within safe ranges automatically. Attack and Defence aren't listed because the game derives them from your attributes plus equipment.");

        H("4.  Gold  (editable)");
        B("Use the Gold box at the bottom-left to set the party's gold.");

        H("5.  Items tab  (read-only)");
        B("Shows each character's equipped gear by slot (Weapon, Armour, Shield, Helmet, Accessory) and any other items they carry. The equipped weapon is read from the game's own data; the other slots show your best item for that slot.");

        H("6.  Party Bag tab  (read-only)");
        B("Lists the shared party bag with item names and quantities.");

        H("7.  Save your changes");
        B("Click “Save As (new copy)…” and choose a file name (it suggests *_edited.p2s). Your original save is left untouched.");

        H("8.  Load it in PCSX2");
        B("In PCSX2, use “Load State From File” and select the edited .p2s. Open the in-game menu to confirm your changes.");

        H("Good to know");
        B("• Nothing is written until you choose Save As.\n"
        + "• Edits are applied to every copy of the data the game keeps in memory, so they stick when you load the state.\n"
        + "• This editor is calibrated for the NTSC-U release (SLUS-212.07). A warning banner appears if a different version or an unusual save is detected.");

        // Tidy margins and scroll back to the top.
        _txt.SelectAll();
        _txt.SelectionIndent = 12;
        _txt.SelectionRightIndent = 12;
        _txt.DeselectAll();
        _txt.SelectionStart = 0;
        _txt.ScrollToCaret();
    }
}
