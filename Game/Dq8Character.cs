using System.Buffers.Binary;
using System.Text;

namespace DQ8SaveEditor.Game;

/// <summary>
/// A live view over one party member, spanning every valid copy of the party
/// structure. Reads come from the first valid copy; writes go to all of them.
/// </summary>
public sealed class Dq8Character
{
    private readonly byte[] _ee;
    private readonly Dq8Layout _layout;
    private readonly MemberDef _member;
    private readonly IReadOnlyList<int> _headerBases;   // valid copies only

    public Dq8Character(byte[] ee, Dq8Layout layout, MemberDef member, IReadOnlyList<int> headerBases)
    {
        _ee = ee;
        _layout = layout;
        _member = member;
        _headerBases = headerBases;
    }

    public int Slot => _member.Slot;
    public bool IsHero => string.IsNullOrEmpty(_member.FixedName);

    private int StatBase(int headerBase) =>
        headerBase + _layout.StatBlockOffset + _member.Slot * _layout.PartyStride;

    private int ItemBlockBase(int headerBase) =>
        headerBase + _layout.ItemsBlockOffset + _member.Slot * _layout.CharItemStride;

    /// <summary>
    /// The real equipped-weapon item id from the runtime equip array, or null if that
    /// array is disabled / out of range. Caller should sanity-check it against carried items.
    /// </summary>
    public int? EquippedWeaponId()
    {
        if (_layout.EquippedWeaponBase < 0)
        {
            return null;
        }

        int at = _layout.EquippedWeaponBase + _member.Slot * _layout.EquippedWeaponStride;
        if (at < 0 || at + 2 > _ee.Length)
        {
            return null;
        }

        return BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(at));
    }

    /// <summary>This character's personal items (read-only), in slot order.</summary>
    public IReadOnlyList<ItemInfo> CarriedItems()
    {
        var list = new List<ItemInfo>();
        if (_headerBases.Count == 0)
        {
            return list;
        }

        int b = ItemBlockBase(_headerBases[0]);
        for (int i = 0; i < _layout.ItemSlotsPerChar; i++)
        {
            int at = b + i * 4;
            if (at + 2 > _ee.Length)
            {
                break;
            }

            int id = BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(at));
            if (id != 0)
            {
                list.Add(Dq8Items.Lookup(id));
            }
        }

        return list;
    }

    public string DisplayName
    {
        get
        {
            if (!IsHero)
            {
                return _member.FixedName!;
            }

            // Read the stored hero name from the first copy.
            if (_headerBases.Count > 0)
            {
                int at = _headerBases[0] + _layout.HeroNameOffset;
                if (at >= 0 && at + _layout.HeroNameLen <= _ee.Length)
                {
                    var span = _ee.AsSpan(at, _layout.HeroNameLen);
                    int len = span.IndexOf((byte)0);
                    if (len < 0)
                    {
                        len = span.Length;
                    }

                    string n = Encoding.ASCII.GetString(span[..len]).Trim();
                    if (n.Length > 0)
                    {
                        return n;
                    }
                }
            }

            return "Hero";
        }
    }

    /// <summary>A slot is occupied if its stat block holds plausible data.</summary>
    public bool IsPresent
    {
        get
        {
            if (_headerBases.Count == 0)
            {
                return false;
            }

            int b = StatBase(_headerBases[0]);
            if (b < 0 || b + _layout.PartyStride > _ee.Length)
            {
                return false;
            }

            long maxHp = ReadU32(b + 0x04);
            long level0 = ReadU32(b + 0x10);

            return maxHp is > 0 and < 100000 && level0 < 100;
        }
    }

    public bool TryGet(string key, out long display)
    {
        display = 0;
        var f = _layout.Fields.FirstOrDefault(x => x.Key == key);
        if (f is null || _headerBases.Count == 0)
        {
            return false;
        }

        int at = StatBase(_headerBases[0]) + f.Offset;
        if (at < 0 || at + f.Size > _ee.Length)
        {
            return false;
        }

        long stored = ReadValue(at, f.Size);
        display = f.DisplayPlusOne ? stored + 1 : stored;

        return true;
    }

    public void Set(string key, long display)
    {
        var f = _layout.Fields.FirstOrDefault(x => x.Key == key);
        if (f is null)
        {
            return;
        }

        display = Math.Clamp(display, f.Min, f.Max);
        long stored = f.DisplayPlusOne ? display - 1 : display;

        foreach (int headerBase in _headerBases)
        {
            int at = StatBase(headerBase) + f.Offset;
            if (at >= 0 && at + f.Size <= _ee.Length)
            {
                WriteValue(at, f.Size, stored);
            }

            // The hero's level is mirrored in the header (1-indexed) too.
            if (key == "level" && IsHero)
            {
                int la = headerBase + _layout.HeroLevelOffset;
                if (la >= 0 && la + 4 <= _ee.Length)
                {
                    BinaryPrimitives.WriteUInt32LittleEndian(_ee.AsSpan(la), (uint)display);
                }
            }
        }
    }

    private long ReadValue(int at, int size) => size switch
    {
        1 => _ee[at],
        2 => BinaryPrimitives.ReadUInt16LittleEndian(_ee.AsSpan(at)),
        4 => BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(at)),
        _ => 0
    };

    private long ReadU32(int at) =>
        at >= 0 && at + 4 <= _ee.Length ? BinaryPrimitives.ReadUInt32LittleEndian(_ee.AsSpan(at)) : -1;

    private void WriteValue(int at, int size, long value)
    {
        switch (size)
        {
            case 1: _ee[at] = (byte)value; break;
            case 2: BinaryPrimitives.WriteUInt16LittleEndian(_ee.AsSpan(at), (ushort)value); break;
            case 4: BinaryPrimitives.WriteUInt32LittleEndian(_ee.AsSpan(at), (uint)value); break;
        }
    }
}
