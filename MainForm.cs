using DQ8SaveEditor.Game;

namespace DQ8SaveEditor;

public sealed partial class MainForm : Form
{
    private readonly Dq8Layout _layout = Dq8Layout.Load();
    private Dq8Save? _save;
    private readonly List<Dq8Character> _party = new();   // present members, in list order
    private readonly Dictionary<string, NumericUpDown> _fieldEditors = new();
    private bool _dirty;
    private bool _loading;   // suppress write-back while populating controls
    private string? _pendingOpen;

    private static readonly (ItemSlot slot, string label)[] EquipSlots =
    {
        (ItemSlot.Weapon, "Weapon"), (ItemSlot.Armour, "Armour"), (ItemSlot.Shield, "Shield"),
        (ItemSlot.Helmet, "Helmet"), (ItemSlot.Accessory, "Accessory"),
    };

    public MainForm() : this(null) { }

    public MainForm(string? openPath)
    {
        InitializeComponent();   // visual layout (MainForm.Designer.cs)
        ApplyTheme();            // colours, fonts, owner-draw renderer
        WireEvents();
        BuildFieldEditors();     // generate the stat rows from the offset config
        UpdateState();
        _pendingOpen = openPath;
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if (_pendingOpen is not null)
        {
            string path = _pendingOpen;
            _pendingOpen = null;
            LoadPath(path);
        }
    }

    // ---- Theme & wiring (kept out of the designer file so it stays editable) ----

    private void ApplyTheme()
    {
        BackColor = Theme.WoodDark;
        ForeColor = Theme.Ink;
        Font = Theme.Body(9.75f);

        _menu.Renderer = new ToolStripProfessionalRenderer(new DqMenuColors()) { RoundedEdges = false };
        _menu.BackColor = Theme.WoodDark;
        _menu.ForeColor = Theme.Parchment;
        _menu.Font = Theme.Body(9.75f);

        _header.BackColor = Theme.Parchment;
        _lblFile.ForeColor = Theme.Ink;
        _lblFile.BackColor = Color.Transparent;
        _lblSerial.ForeColor = Theme.InkMuted;
        _lblSerial.BackColor = Color.Transparent;

        _goldPanel.BackColor = Theme.Parchment;
        _lblGold.ForeColor = Theme.Ink;
        _lblGold.BackColor = Color.Transparent;
        _lblGold.Font = Theme.Body(11f, FontStyle.Bold);
        _numGold.Maximum = _layout.GoldMax;
        _numGold.Font = Theme.Body(11f, FontStyle.Bold);

        StyleNumeric(_numGold);
        StyleButton(_btnSave);

        _leftPanel.BackColor = Theme.Parchment;
        _lblCharacters.ForeColor = Theme.Ink;
        _lblCharacters.BackColor = Color.Transparent;
        _lblCharacters.Font = Theme.Body(11f, FontStyle.Bold);
        _lstChars.BackColor = Theme.Parchment;
        _lstChars.ForeColor = Theme.Ink;
        _lstChars.Font = Theme.Body(12f, FontStyle.Bold);

        _divider.BackColor = Theme.WoodMid;

        _rightPanel.BackColor = Theme.Parchment;
        _txtName.BackColor = Theme.Parchment;
        _txtName.ForeColor = Theme.GoldDeep;
        _txtName.Font = Theme.Body(13f, FontStyle.Bold);

        _tabs.Font = Theme.Body(10f, FontStyle.Bold);
        foreach (TabPage page in _tabs.TabPages)
        {
            page.BackColor = Theme.Parchment;
        }

        _fieldTable.BackColor = Theme.Parchment;
        _equipTable.BackColor = Theme.Parchment;
        _lblCarried.ForeColor = Theme.InkMuted;
        _lblCarried.BackColor = Color.Transparent;
        _lblCarried.Font = Theme.Body(9.5f, FontStyle.Italic);

        ThemeReadOnlyList(_carriedList);
        ThemeReadOnlyList(_bagList);
    }

    private void WireEvents()
    {
        _mnuOpen.Click += (_, _) => OpenFile();
        _mnuSaveAs.Click += (_, _) => SaveAs();
        _mnuExit.Click += (_, _) => Close();
        _mnuInstructions.Click += (_, _) => { using var f = new InstructionsForm(); f.ShowDialog(this); };
        _mnuAbout.Click += (_, _) => MessageBox.Show(this,
            "DQ8 Save State Editor\n\nEdits PCSX2 (.p2s) save states for\nDragon Quest VIII (SLUS-212.07).\n\nAlways writes to a new copy; your original is never modified.",
            "About", MessageBoxButtons.OK, MessageBoxIcon.Information);

        _numGold.ValueChanged += (_, _) => { if (!_loading && _save is not null) { _save.Gold = (long)_numGold.Value; MarkDirty(); } };
        _btnSave.Click += (_, _) => SaveAs();

        _lstChars.DrawItem += DrawCharacterItem;
        _lstChars.SelectedIndexChanged += (_, _) => LoadSelectedCharacter();
        _tabs.DrawItem += DrawTab;
        _carriedList.DrawItem += DrawReadOnlyRow;
        _bagList.DrawItem += DrawReadOnlyRow;
    }

    private static void ThemeReadOnlyList(ListBox lb)
    {
        lb.BackColor = Theme.Parchment;
        lb.ForeColor = Theme.Ink;
        lb.Font = Theme.Body(11f);
    }

    private static void StyleNumeric(NumericUpDown n)
    {
        n.BorderStyle = BorderStyle.FixedSingle;
        n.BackColor = Theme.ParchmentHi;
        n.ForeColor = Theme.Ink;
    }

    private static void StyleButton(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.BackColor = Theme.WoodMid;
        b.ForeColor = Theme.Parchment;
        b.Font = Theme.Body(10f, FontStyle.Bold);
        b.FlatAppearance.BorderColor = Theme.GoldDeep;
        b.FlatAppearance.BorderSize = 2;
        b.FlatAppearance.MouseOverBackColor = Theme.WoodDark;
        b.Cursor = Cursors.Hand;
    }

    // ---- Owner drawing ---------------------------------------------------

    private static void DrawReadOnlyRow(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ListBox lb || e.Index < 0)
        {
            return;
        }

        e.Graphics.FillRectangle(new SolidBrush(Theme.Parchment), e.Bounds);
        string text = lb.Items[e.Index].ToString() ?? "";
        TextRenderer.DrawText(e.Graphics, text, lb.Font, Rectangle.Inflate(e.Bounds, -6, 0),
            Theme.Ink, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }

    private void DrawTab(object? sender, DrawItemEventArgs e)
    {
        var rect = _tabs.GetTabRect(e.Index);
        bool selected = e.Index == _tabs.SelectedIndex;
        using var bg = new SolidBrush(selected ? Theme.Gold : Theme.ParchmentHi);
        e.Graphics.FillRectangle(bg, rect);
        using var border = new Pen(Theme.WoodMid);
        e.Graphics.DrawRectangle(border, rect);
        TextRenderer.DrawText(e.Graphics, _tabs.TabPages[e.Index].Text, _tabs.Font, rect,
            selected ? Theme.WoodDark : Theme.Ink,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
    }

    /// <summary>Parchment rows with a gold highlight for the selected character.</summary>
    private void DrawCharacterItem(object? sender, DrawItemEventArgs e)
    {
        if (e.Index < 0)
        {
            return;
        }

        var g = e.Graphics;
        var rect = e.Bounds;
        g.FillRectangle(new SolidBrush(Theme.Parchment), rect);

        if ((e.State & DrawItemState.Selected) != 0)
        {
            var hi = Rectangle.Inflate(rect, -3, -3);
            using var fill = new SolidBrush(Color.FromArgb(70, Theme.Gold));
            using var pen = new Pen(Theme.GoldDeep, 2);
            g.FillRectangle(fill, hi);
            g.DrawRectangle(pen, hi);
        }

        string text = _lstChars.Items[e.Index].ToString() ?? "";
        TextRenderer.DrawText(g, text, _lstChars.Font, Rectangle.Inflate(rect, -10, 0),
            Theme.Ink, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
    }

    // ---- Dynamic stat rows (generated from the offset config) ------------

    private void BuildFieldEditors()
    {
        _fieldTable.Controls.Clear();
        _fieldEditors.Clear();
        _fieldTable.RowStyles.Clear();
        _fieldTable.RowCount = _layout.Fields.Count;
        for (int i = 0; i < _layout.Fields.Count; i++)
        {
            _fieldTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        }

        int row = 0;
        foreach (var f in _layout.Fields)
        {
            var lbl = new Label
            {
                Text = f.Label, AutoSize = true, Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 9, 0, 0), ForeColor = Theme.Ink,
                BackColor = Color.Transparent, Font = Theme.Body(11f),
            };
            var num = new NumericUpDown
            {
                Minimum = f.Min,
                Maximum = f.Max,
                Width = 130,
                ThousandsSeparator = f.Max > 9999,
                Anchor = AnchorStyles.Left,
                Tag = f.Key,
                Font = Theme.Body(11f),
            };
            StyleNumeric(num);
            num.ValueChanged += (_, _) =>
            {
                if (_loading)
                {
                    return;
                }

                int idx = _lstChars.SelectedIndex;
                if (idx < 0 || idx >= _party.Count)
                {
                    return;
                }

                _party[idx].Set(f.Key, (long)num.Value);
                MarkDirty();
            };
            _fieldEditors[f.Key] = num;
            _fieldTable.Controls.Add(lbl, 0, row);
            _fieldTable.Controls.Add(num, 1, row);
            row++;
        }
    }

    // ---- Actions ---------------------------------------------------------

    private void OpenFile()
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "PCSX2 save state (*.p2s)|*.p2s|All files (*.*)|*.*",
            Title = "Open PCSX2 save state",
        };

        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        LoadPath(dlg.FileName);
    }

    private void LoadPath(string path)
    {
        try
        {
            _save = Dq8Save.Open(path, _layout);
            _dirty = false;
            PopulateCharacters();
            UpdateState();
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Could not open save state:\n\n{ex.Message}", "Open failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SaveAs()
    {
        if (_save is null)
        {
            return;
        }

        string suggested = Path.GetFileNameWithoutExtension(_save.SourcePath) + "_edited.p2s";
        using var dlg = new SaveFileDialog
        {
            Filter = "PCSX2 save state (*.p2s)|*.p2s",
            Title = "Save edited copy",
            FileName = suggested,
            InitialDirectory = Path.GetDirectoryName(_save.SourcePath),
        };

        if (dlg.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        if (string.Equals(Path.GetFullPath(dlg.FileName), Path.GetFullPath(_save.SourcePath),
                StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Please choose a different file name — the editor never overwrites the original.",
                "Choose a new file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            _save.SaveAs(dlg.FileName);
            _dirty = false;
            UpdateTitle();
            MessageBox.Show(this, $"Saved:\n{dlg.FileName}\n\nLoad this state in PCSX2 to use it.",
                "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"Could not save:\n\n{ex.Message}", "Save failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ---- Population / state ---------------------------------------------

    private void PopulateCharacters()
    {
        _lstChars.Items.Clear();
        _party.Clear();
        if (_save is null)
        {
            return;
        }

        foreach (var c in _save.PartyMembers)
        {
            _party.Add(c);
            _lstChars.Items.Add(c.DisplayName);
        }

        RenderBag();

        if (_lstChars.Items.Count > 0)
        {
            _lstChars.SelectedIndex = 0;
        }
    }

    private void LoadSelectedCharacter()
    {
        int idx = _lstChars.SelectedIndex;
        if (idx < 0 || idx >= _party.Count)
        {
            return;
        }

        var c = _party[idx];

        _loading = true;
        _txtName.Text = c.DisplayName;
        foreach (var (key, num) in _fieldEditors)
        {
            if (c.TryGet(key, out long v))
            {
                num.Value = Math.Clamp(v, num.Minimum, num.Maximum);
                num.Enabled = true;
            }
            else
            {
                num.Enabled = false;
            }
        }

        _loading = false;
        RenderItems(c);
    }

    /// <summary>Fills the Items tab: equipment by slot + remaining carried items.</summary>
    private void RenderItems(Dq8Character c)
    {
        var items = c.CarriedItems();

        _equipTable.Controls.Clear();
        _equipTable.RowCount = EquipSlots.Length;
        int row = 0;
        // The real equipped-weapon pointer (runtime array) — trusted only if it names a
        // weapon this character actually carries; otherwise we fall back to the heuristic.
        int? eqWeaponId = c.EquippedWeaponId();

        var usedAsEquip = new List<ItemInfo>();
        foreach (var (slot, label) in EquipSlots)
        {
            ItemInfo worn = default;

            if (slot == ItemSlot.Weapon && eqWeaponId is int wid && wid != 0)
            {
                worn = items.FirstOrDefault(x => x.Id == wid && x.Slot == ItemSlot.Weapon);
            }

            // Fallback (and all non-weapon slots): no per-item flag in the save, so show
            // the higher-tier item of the slot as equipped (DQ8 IDs are tier-ordered).
            if (worn.Name is null)
            {
                worn = items.Where(x => x.Slot == slot && !usedAsEquip.Contains(x))
                            .OrderByDescending(x => x.Id)
                            .FirstOrDefault();
            }

            if (worn.Name is not null)
            {
                usedAsEquip.Add(worn);
            }

            var lblSlot = new Label { Text = label, AutoSize = true, ForeColor = Theme.InkMuted, BackColor = Color.Transparent, Font = Theme.Body(10.5f), Margin = new Padding(0, 4, 0, 4), Anchor = AnchorStyles.Left };
            var lblItem = new Label
            {
                Text = worn.Name ?? "— (none) —",
                AutoSize = true, BackColor = Color.Transparent,
                ForeColor = worn.Name is null ? Theme.InkMuted : Theme.Ink,
                Font = Theme.Body(10.5f, worn.Name is null ? FontStyle.Italic : FontStyle.Bold),
                Margin = new Padding(0, 4, 0, 4), Anchor = AnchorStyles.Left,
            };
            _equipTable.Controls.Add(lblSlot, 0, row);
            _equipTable.Controls.Add(lblItem, 1, row);
            row++;
        }

        // Anything not counted as an equipped piece is a carried/consumable item.
        _carriedList.BeginUpdate();
        _carriedList.Items.Clear();
        var counts = new Dictionary<string, int>();
        foreach (var it in items)
        {
            if (usedAsEquip.Remove(it))
            {
                continue;   // skip the equipped pieces
            }

            counts[it.Name] = counts.GetValueOrDefault(it.Name) + 1;
        }

        foreach (var (name, n) in counts)
        {
            _carriedList.Items.Add(n > 1 ? $"{name}  ×{n}" : name);
        }

        if (_carriedList.Items.Count == 0)
        {
            _carriedList.Items.Add("— none —");
        }

        _carriedList.EndUpdate();
    }

    private void RenderBag()
    {
        _bagList.BeginUpdate();
        _bagList.Items.Clear();

        if (_save is not null)
        {
            foreach (var e in _save.Bag())
            {
                _bagList.Items.Add(e.Quantity > 1 ? $"{e.Info.Name}  ×{e.Quantity}" : e.Info.Name);
            }
        }

        if (_bagList.Items.Count == 0)
        {
            _bagList.Items.Add("— empty —");
        }

        _bagList.EndUpdate();
    }

    private void UpdateState()
    {
        bool loaded = _save is not null;
        _btnSave.Enabled = loaded;
        _numGold.Enabled = loaded && _save!.GoldCalibrated;

        if (!loaded)
        {
            _lblFile.Text = "No save state loaded.  File ▸ Open…";
            _lblSerial.Text = "";
            _lblBanner.Visible = false;
            UpdateTitle();
            return;
        }

        _lblFile.Text = _save!.SourcePath;
        string serial = _save.DetectedSerial ?? "unknown";
        _lblSerial.Text = $"Detected game: {serial}   •   {_party.Count} party member(s)   •   {_save.CopiesFound} data cop{(_save.CopiesFound == 1 ? "y" : "ies")}";
        _lblSerial.ForeColor = serial.StartsWith("SLUS_212") ? Theme.InkMuted : Color.Firebrick;

        _loading = true;

        if (_save.GoldCalibrated)
        {
            _numGold.Value = Math.Clamp(_save.Gold, _numGold.Minimum, _numGold.Maximum);
        }

        _loading = false;

        if (!_save.StructureFound)
        {
            _lblBanner.Visible = true;
            _lblBanner.BackColor = Color.FromArgb(255, 235, 235);
            _lblBanner.ForeColor = Color.Firebrick;
            _lblBanner.Text = "⚠  DQ8 party data not found at the expected offsets. This may be a different game/version, or an unusual save point.";
        }
        else if (!serial.StartsWith("SLUS_212"))
        {
            _lblBanner.Visible = true;
            _lblBanner.BackColor = Color.FromArgb(255, 248, 196);
            _lblBanner.ForeColor = Color.FromArgb(120, 90, 0);
            _lblBanner.Text = $"⚠  This editor is calibrated for SLUS-212.07 (NTSC-U). Detected: {serial}. Edits may be incorrect.";
        }
        else
        {
            _lblBanner.Visible = false;
        }
        UpdateTitle();
    }

    private void MarkDirty()
    {
        if (_dirty)
        {
            return;
        }

        _dirty = true;
        UpdateTitle();
    }

    private void UpdateTitle()
    {
        const string baseTitle = "DQ8 Save State Editor";
        if (_save is null) 
        { 
            Text = baseTitle; return; 
        }

        Text = $"{baseTitle} — {Path.GetFileName(_save.SourcePath)}{(_dirty ? " *" : "")}";
    }
}
