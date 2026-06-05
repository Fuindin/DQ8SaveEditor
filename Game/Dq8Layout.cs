using System.Text.Json;
using System.Text.Json.Serialization;

namespace DQ8SaveEditor.Game;

/// <summary>One editable numeric field within a character's stat block.</summary>
public sealed class FieldDef
{
    public string Key { get; set; } = "";
    public string Label { get; set; } = "";
    /// <summary>Byte offset relative to the member's stat-block base.</summary>
    public int Offset { get; set; } = -1;
    /// <summary>"u8", "u16", or "u32" (little-endian).</summary>
    public string Type { get; set; } = "u32";
    /// <summary>Min/Max are expressed in *display* units.</summary>
    public long Min { get; set; }
    public long Max { get; set; } = 9999;
    /// <summary>If true, the stored value is (display - 1). DQ8 stores level 0-indexed.</summary>
    public bool DisplayPlusOne { get; set; }

    public int Size => Type switch { "u8" => 1, "u16" => 2, "u32" => 4, _ => 4 };
}

/// <summary>A party slot and (for non-hero members) the game's fixed name.</summary>
public sealed class MemberDef
{
    public int Slot { get; set; }
    /// <summary>Null/empty = the player-named hero (read the stored name).</summary>
    public string? FixedName { get; set; }
}

/// <summary>
/// Memory map for Dragon Quest VIII (SLUS-212.07) party data, verified against a
/// real save. The hero record begins with the player-entered name, then party
/// gold and hero level, then a 104-byte-stride array of per-member stat blocks.
/// The whole structure exists as multiple synchronized copies in RAM; edits are
/// written to every valid copy. Offsets are JSON-overridable via dq8_layout.json.
/// </summary>
public sealed class Dq8Layout
{
    public string GameSerial { get; set; } = "SLUS_212.07";

    /// <summary>Absolute offsets of each copy's hero-record header in eeMemory.bin.</summary>
    public List<int> HeroHeaderBases { get; set; } = new() { 0x409830, 0x1BEF5C0 };

    /// <summary>Header base -> first member's stat block.</summary>
    public int StatBlockOffset { get; set; } = 0x40;
    public int PartyStride { get; set; } = 104;     // 0x68

    public int HeroNameOffset { get; set; } = 0x00; // relative to header base
    public int HeroNameLen { get; set; } = 16;

    public int GoldOffset { get; set; } = 0x10;     // relative to header base, u32
    public long GoldMax { get; set; } = 9_999_999;

    /// <summary>
    /// Relative to header base: party-membership BITMASK (u16). Bit i set = roster
    /// member i (0=hero, 1=Yangus, 2=Jessica, 3=Angelo, …) is in the party. Verified
    /// 0b011 (Jack+Yangus) -> 0b111 when Jessica joined. (Character level is NOT here —
    /// it lives in each stat block at +0x10; this offset only coincidentally equalled
    /// the hero's level early game.)
    /// </summary>
    public int PartyBitmaskOffset { get; set; } = 0x14;

    // --- Inventory (read-only) ---
    /// <summary>Header base -> first character's personal item block.</summary>
    public int ItemsBlockOffset { get; set; } = 0x1E0;
    /// <summary>Bytes between characters' personal item blocks.</summary>
    public int CharItemStride { get; set; } = 0x30;
    /// <summary>4-byte slots per character ([id u16][pad/qty u16]).</summary>
    public int ItemSlotsPerChar { get; set; } = 12;
    /// <summary>Header base -> shared party bag.</summary>
    public int BagOffset { get; set; } = 0x2A0;
    /// <summary>4-byte slots in the bag ([id u16][qty u16]).</summary>
    public int BagSlots { get; set; } = 384;

    /// <summary>
    /// ABSOLUTE eeMemory offset of the equipped-weapon-id array (u16 per party slot),
    /// found via a controlled 3-save experiment. Slot 0 (hero) lives here; later
    /// members follow at +EquippedWeaponStride. Used only when the value sanity-checks
    /// against the character's carried weapons; otherwise the tier heuristic applies.
    /// -1 disables it.
    /// </summary>
    public int EquippedWeaponBase { get; set; } = 0x44DA70;
    public int EquippedWeaponStride { get; set; } = 0x0A;

    /// <summary>If set (>0), forces the party size instead of reading the membership bitmask.</summary>
    public int PartyCountOverride { get; set; }

    public List<FieldDef> Fields { get; set; } = new();
    public List<MemberDef> Members { get; set; } = new();

    [JsonIgnore] public int StatBlockSpan => PartyStride;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    public static Dq8Layout Default() => new()
    {
        Fields =
        {
            new() { Key = "level",      Label = "Level",        Type = "u32", Offset = 0x10, Min = 1, Max = 99,        DisplayPlusOne = true },
            new() { Key = "experience", Label = "Experience",   Type = "u32", Offset = 0x18, Min = 0, Max = 9_999_999 },
            new() { Key = "curHp",      Label = "HP (current)", Type = "u32", Offset = 0x00, Min = 0, Max = 9999 },
            new() { Key = "maxHp",      Label = "HP (max)",     Type = "u32", Offset = 0x04, Min = 1, Max = 9999 },
            new() { Key = "curMp",      Label = "MP (current)", Type = "u32", Offset = 0x08, Min = 0, Max = 9999 },
            new() { Key = "maxMp",      Label = "MP (max)",     Type = "u32", Offset = 0x0C, Min = 0, Max = 9999 },
            new() { Key = "strength",   Label = "Strength",     Type = "u16", Offset = 0x30, Min = 0, Max = 999 },
            new() { Key = "agility",    Label = "Agility",      Type = "u16", Offset = 0x32, Min = 0, Max = 999 },
            new() { Key = "resilience", Label = "Resilience",   Type = "u16", Offset = 0x34, Min = 0, Max = 999 },
            new() { Key = "wisdom",     Label = "Wisdom",       Type = "u16", Offset = 0x36, Min = 0, Max = 999 },
        },
        Members =
        {
            new() { Slot = 0, FixedName = null },        // hero (stored name)
            new() { Slot = 1, FixedName = "Yangus" },
            new() { Slot = 2, FixedName = "Jessica" },
            new() { Slot = 3, FixedName = "Angelo" },
        }
    };

    public static Dq8Layout Load()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "dq8_layout.json");
        if (File.Exists(path))
        {
            try
            {
                var loaded = JsonSerializer.Deserialize<Dq8Layout>(File.ReadAllText(path), JsonOpts);
                if (loaded is { Fields.Count: > 0, Members.Count: > 0 })
                {
                    return loaded;
                }
            }
            catch { /* fall back to baked-in default */ }
        }

        return Default();
    }

    public void Save()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "dq8_layout.json");
        File.WriteAllText(path, JsonSerializer.Serialize(this, JsonOpts));
    }
}
