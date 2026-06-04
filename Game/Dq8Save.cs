using System.Buffers.Binary;
using DQ8SaveEditor.SaveFormat;

namespace DQ8SaveEditor.Game;

/// <summary>
/// High-level model: opens a .p2s, locates the DQ8 party structure copies,
/// exposes party members and gold for editing, and repacks to a new file.
/// </summary>
public sealed class Dq8Save
{
    private readonly P2SArchive _archive;
    private readonly byte[] _ee;
    private readonly Dq8Layout _layout;
    private readonly List<int> _validBases;     // copies whose header looks valid

    public string SourcePath { get; }
    public Dq8Layout Layout => _layout;
    public string? DetectedSerial { get; }
    public IReadOnlyList<Dq8Character> AllMembers { get; }

    /// <summary>Active party members for this save (Jessica/Angelo etc. are excluded until they join).</summary>
    public IReadOnlyList<Dq8Character> PartyMembers =>
        AllMembers.Take(PartyCount).Where(m => m.IsPresent).ToList();

    /// <summary>Detected active party size (clamped), or the configured override.</summary>
    public int PartyCount
    {
        get
        {
            if (_layout.PartyCountOverride > 0)
            {
                return Math.Min(_layout.PartyCountOverride, AllMembers.Count);
            }

            if (_validBases.Count == 0)
            {
                return 0;
            }

            int at = _validBases[0] + _layout.PartyCountOffset;
            int n = at + 2 <= _ee.Length ? BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(at)) : 0;

            return Math.Clamp(n, 0, AllMembers.Count);
        }
    }

    /// <summary>True if at least one valid party-structure copy was found.</summary>
    public bool StructureFound => _validBases.Count > 0;

    public int CopiesFound => _validBases.Count;

    private Dq8Save(string path, P2SArchive archive, byte[] ee, Dq8Layout layout)
    {
        SourcePath = path;
        _archive = archive;
        _ee = ee;
        _layout = layout;
        DetectedSerial = FindSerial(ee);
        _validBases = layout.HeroHeaderBases.Where(b => LooksLikeHeader(ee, layout, b)).ToList();
        AllMembers = layout.Members
            .Select(m => new Dq8Character(ee, layout, m, _validBases))
            .ToList();
    }

    public static Dq8Save Open(string path, Dq8Layout layout)
    {
        var archive = P2SArchive.Load(path);
        if (!archive.Has("eeMemory.bin"))
        {
            throw new InvalidDataException("This .p2s has no eeMemory.bin region.");
        }

        return new Dq8Save(path, archive, archive.GetData("eeMemory.bin"), layout);
    }

    public bool GoldCalibrated => _validBases.Count > 0;

    public long Gold
    {
        get
        {
            if (_validBases.Count == 0)
            {
                return 0;
            }

            int at = _validBases[0] + _layout.GoldOffset;
            return at + 4 <= _ee.Length ? BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(at)) : 0;
        }
        set
        {
            long v = Math.Clamp(value, 0, _layout.GoldMax);
            foreach (int b in _validBases)
            {
                int at = b + _layout.GoldOffset;
                if (at + 4 <= _ee.Length)
                {
                    BinaryPrimitives.WriteUInt32LittleEndian(_ee.AsSpan(at), (uint)v);
                }
            }
        }
    }

    public void SaveAs(string destPath)
    {
        _archive.SetData("eeMemory.bin", _ee);
        _archive.Save(destPath);
    }

    public byte[] EeMemory => _ee;

    public readonly record struct BagEntry(ItemInfo Info, int Quantity);

    /// <summary>The shared party bag contents (read-only).</summary>
    public IReadOnlyList<BagEntry> Bag()
    {
        var list = new List<BagEntry>();
        if (_validBases.Count == 0)
        {
            return list;
        }

        int b = _validBases[0] + _layout.BagOffset;
        for (int i = 0; i < _layout.BagSlots; i++)
        {
            int at = b + i * 4;
            if (at + 4 > _ee.Length)
            {
                break;
            }

            int id = BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(at));
            int qty = BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(at + 2));
            if (id != 0)
            { 
                list.Add(new BagEntry(Dq8Items.Lookup(id), qty));
            }
        }

        return list;
    }

    /// <summary>A header copy is valid if hero level and slot-0 max HP look sane.</summary>
    private static bool LooksLikeHeader(byte[] ee, Dq8Layout layout, int headerBase)
    {
        if (headerBase < 0)
        {
            return false;
        }

        int levelAt = headerBase + layout.HeroLevelOffset;
        int maxHpAt = headerBase + layout.StatBlockOffset + 0x04;
        if (levelAt + 4 > ee.Length || maxHpAt + 4 > ee.Length)
        {
            return false;
        }

        long level = BinaryPrimitives.ReadUInt32LittleEndian(ee.AsSpan(levelAt));
        long maxHp = BinaryPrimitives.ReadUInt32LittleEndian(ee.AsSpan(maxHpAt));

        return level is >= 1 and <= 99 && maxHp is >= 1 and <= 99999;
    }

    private static string? FindSerial(byte[] ee)
    {
        for (int i = 0; i < ee.Length - 11; i++)
        {
            if (ee[i] != (byte)'S' || ee[i + 1] != (byte)'L')
            {
                continue;
            }

            char c2 = (char)ee[i + 2], c3 = (char)ee[i + 3];
            if (c2 is 'U' or 'E' or 'P' or 'A' && c3 is 'S' or 'L' or 'M' or 'B'
                && ee[i + 4] == (byte)'_' && char.IsDigit((char)ee[i + 5]))
            {
                return System.Text.Encoding.ASCII.GetString(ee, i, 11);
            }
        }

        return null;
    }
}
