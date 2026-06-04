namespace DQ8SaveEditor;

/// <summary>Dragon Quest VIII menu palette — aged parchment, dark wood frame, ink-brown serif text.</summary>
internal static class Theme
{
    public static readonly Color WoodDark   = Color.FromArgb(0x3A, 0x24, 0x18); // outer frame
    public static readonly Color WoodMid    = Color.FromArgb(0x5A, 0x38, 0x22); // buttons / borders
    public static readonly Color Parchment  = Color.FromArgb(0xE6, 0xD7, 0xB0); // panel background
    public static readonly Color ParchmentHi = Color.FromArgb(0xF2, 0xE7, 0xC8); // lighter fields
    public static readonly Color Ink         = Color.FromArgb(0x3B, 0x2A, 0x18); // body text
    public static readonly Color InkMuted    = Color.FromArgb(0x6E, 0x57, 0x3A); // secondary text
    public static readonly Color Gold        = Color.FromArgb(0xD7, 0xA8, 0x3C); // selection / accents
    public static readonly Color GoldDeep    = Color.FromArgb(0xB6, 0x86, 0x22);

    public static readonly string SerifFamily = "Palatino Linotype"; // falls back gracefully

    public static Font Body(float size, FontStyle style = FontStyle.Regular)
        => new(SerifFamily, size, style);
}

/// <summary>Themed colors for the menu strip (parchment popups, gold highlight).</summary>
internal sealed class DqMenuColors : System.Windows.Forms.ProfessionalColorTable
{
    public override Color MenuStripGradientBegin => Theme.WoodDark;
    public override Color MenuStripGradientEnd   => Theme.WoodDark;
    public override Color ToolStripDropDownBackground => Theme.Parchment;
    public override Color ImageMarginGradientBegin => Theme.Parchment;
    public override Color ImageMarginGradientMiddle => Theme.Parchment;
    public override Color ImageMarginGradientEnd => Theme.Parchment;
    public override Color MenuItemSelected => Theme.Gold;
    public override Color MenuItemSelectedGradientBegin => Theme.Gold;
    public override Color MenuItemSelectedGradientEnd => Theme.GoldDeep;
    public override Color MenuItemBorder => Theme.GoldDeep;
    public override Color MenuItemPressedGradientBegin => Theme.ParchmentHi;
    public override Color MenuItemPressedGradientEnd => Theme.Parchment;
    public override Color MenuBorder => Theme.WoodMid;
    public override Color SeparatorDark => Theme.WoodMid;
}
