namespace DQ8SaveEditor.Game;

public enum ItemSlot { Weapon, Armour, Shield, Helmet, Accessory, Consumable, Key, Alchemy, Unknown }

public readonly record struct ItemInfo(int Id, string Name, ItemSlot Slot)
{
    public bool IsEquippable => Slot is ItemSlot.Weapon or ItemSlot.Armour or ItemSlot.Shield
        or ItemSlot.Helmet or ItemSlot.Accessory;
}

/// <summary>
/// Dragon Quest VIII (PS2, SLUS-212.07) item database: maps a 2-byte item ID to a
/// name and equipment slot. IDs are stored little-endian in the save; the codes
/// below are written as the raw byte pair (e.g. "0501" = bytes 05 01 = id 0x0105).
/// </summary>
public static class Dq8Items
{
    private static readonly Dictionary<int, ItemInfo> _byId = Parse();

    public static ItemInfo Lookup(int id) =>
        _byId.TryGetValue(id, out var info) ? info
        : new ItemInfo(id, $"Item #{id} (0x{id:X3})", ItemSlot.Unknown);

    public static bool IsKnown(int id) => _byId.ContainsKey(id);

    private static Dictionary<int, ItemInfo> Parse()
    {
        var map = new Dictionary<int, ItemInfo>();
        foreach (var raw in Table.Split('\n'))
        {
            var line = raw.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            // Format: CODE|SLOT|Name
            int p1 = line.IndexOf('|');
            int p2 = line.IndexOf('|', p1 + 1);
            string code = line[..p1];
            char slotCh = line[p1 + 1];
            string name = line[(p2 + 1)..];
            int b0 = Convert.ToInt32(code.Substring(0, 2), 16);
            int b1 = Convert.ToInt32(code.Substring(2, 2), 16);
            int id = b1 * 256 + b0;   // little-endian
            ItemSlot slot = slotCh switch
            {
                'W' => ItemSlot.Weapon, 'A' => ItemSlot.Armour, 'S' => ItemSlot.Shield,
                'H' => ItemSlot.Helmet, 'R' => ItemSlot.Accessory, 'C' => ItemSlot.Consumable,
                'K' => ItemSlot.Key, 'L' => ItemSlot.Alchemy, _ => ItemSlot.Unknown
            };
            map[id] = new ItemInfo(id, name, slot);
        }

        return map;
    }

    // CODE|SLOT|Name   (SLOT: W weapon, A armour, S shield, H helmet, R accessory,
    //                          C consumable, K key item, L alchemy)
    private const string Table = """
        0100|W|Cypress Stick
        0200|W|Soldier's Sword
        0300|W|Copper Sword
        0400|W|Steel Broadsword
        0500|W|Platinum Sword
        0600|W|Dream Blade
        0700|W|Falcon Blade
        0800|W|Uber Falcon Blade
        0900|W|Zombiesbane
        0A00|W|Zombie Slayer
        0B00|W|Dragonsbane
        0C00|W|Dragon Slayer
        0D00|W|Blizzard Blade
        0E00|W|Miracle Sword
        0F00|W|Uber Miracle Sword
        1000|W|Bastard Sword
        1100|W|Liquid Metal Sword
        1200|W|Double-Edged Sword
        1300|W|Uber Double-Edge
        1400|W|Rusty Old Sword
        1500|W|Dragovian Sword
        1600|W|Dragovian King Sword
        1700|W|Rapier
        1800|W|Templar's Sword
        1900|W|Hell Sabre
        1A00|W|Holy Silver Rapier
        1B00|W|Fallen Angel Rapier
        1C00|W|Shamshir Of Light
        1D00|W|Mercury's Rapier
        6101|W|Stone Sword
        1E00|W|Bronze Knife
        1F00|W|Dagger
        2000|W|Poison Moth Knife
        2100|W|Assassin's Dagger
        2200|W|Icicle Dirk
        2300|W|Poison Needle
        2400|W|Falcon Knife
        2500|W|Imp Knife
        2600|W|Sword Breaker
        2700|W|Eagle Dagger
        2800|W|Iron Lance
        2900|W|Holy Lance
        2A00|W|Long Spear
        2B00|W|Partisan
        2C00|W|Battle Fork
        2D00|W|Sandstorm Spear
        2E00|W|Demon Spear
        2F00|W|Hero Spear
        3000|W|Metal King Spear
        3100|W|Boomerang
        3200|W|Reinforced Boomerang
        3300|W|Edged Boomerang
        3400|W|Metal Wing Boomerang
        3500|W|Razor Wing Boomerang
        3600|W|Swallowtail
        3700|W|Flametang Boomerang
        3800|W|Stone Axe
        3900|W|Iron Axe
        3A00|W|King Axe
        3B00|W|Moon Axe
        3C00|W|Bandit Axe
        3D00|W|Golden Axe
        3E00|W|Battle Axe
        3F00|W|Conquerer's Axe
        4000|W|Oaken Club
        4100|W|Giant Mallet
        4200|W|Sledgehammer
        4300|W|War Hammer
        4400|W|Uber War Hammer
        4600|W|Megaton Hammer
        4700|W|Flail Of Fury
        4800|W|Flail Of Destruction
        4900|W|Thorn Whip
        4A00|W|Leather Whip
        4B00|W|Snakeskin Whip
        4C00|W|Chain Whip
        4D00|W|Dragontail Whip
        4E00|W|Spiked Steel Whip
        4F00|W|Demon Whip
        5000|W|Scourge Whip
        5100|W|Gringham Whip
        5200|W|Farmer's Scythe
        5400|W|Heavy Hatchet
        5500|W|Bardiche Of Binding
        5600|W|Hell Scythe
        5700|W|Steel Scythe
        5800|W|Wizard's Staff
        5900|W|Lightning Staff
        5A00|W|Staff Of Divine Wrath
        5B00|W|Magma Staff
        5D00|W|Staff Of Antimagic
        5E00|W|Rune Staff
        5F00|W|Staff Of Resurrection
        6000|W|Magical Mace
        6200|W|Hunter's Bow
        6300|W|Great Bow
        6400|W|Short Bow
        6500|W|Odin's Bow
        6600|W|Cheiron's Bow
        6700|W|Eros' Bow
        6800|A|Plain Clothes
        6900|A|Bandit's Grass Skirt
        6A00|A|Wayfarer's Clothes
        6B00|A|Boxer Shorts
        6C00|A|Dancer's Costume
        6D00|A|Leather Kilt
        6E00|A|Leather Dress
        6F00|A|Templar's Uniform
        7000|A|Fur Poncho
        7100|A|Cloak Of Evasion
        7200|A|Bunny Suit
        7300|A|Silk Bustier
        7400|A|Magic Bikini
        7500|A|Spangled Dress
        7600|A|Posh Waistcoat
        7700|A|Magical Skirt
        7800|A|Shimmering Dress
        7900|A|Dark Robe
        7B00|A|Dangerous Bustier
        7C00|A|Divine Bustier
        7D00|A|Jessica's Outfit
        7E00|A|Leather Armour
        7F00|A|Scale Armour
        8000|A|Chain Mail
        8100|A|Bronze Armour
        8200|A|Iron Cuirass
        8300|A|Iron Armour
        8400|A|Turtleshell
        8500|A|Full Plate Armour
        8600|A|Silver Cuirass
        8700|A|Dancer's Mail
        8800|A|Silver Mail
        8900|A|Dragon Mail
        8A00|A|Magic Armour
        8B00|A|Zombie Mail
        8C00|A|Heavy Armour
        8D00|A|Spiked Armour
        8E00|A|Platinum Mail
        8F00|A|Bandit Mail
        9000|A|Sacred Armour
        9200|A|Gigant Armour
        9300|A|Mirror Armour
        9400|A|Liquid Metal Armour
        9500|A|Dargovian Armour
        9600|A|Metal King Armour
        9700|A|Silk Robe
        9800|A|Leather Cape
        9900|A|Robe Of Serenity
        9A00|A|Magic Vestment
        9B00|A|Flowing Dress
        9C00|A|Sage's Robe
        9D00|A|Angel's Robe
        9E00|A|Velvet Cape
        9F00|A|Crimson Robe
        A000|A|Princess' Robe
        A100|A|Dragon Robe
        A200|S|Pot Lid
        A300|S|Leather Shield
        A400|S|Scale Shield
        A500|S|Silver Platter
        A600|S|Kitty Shield
        A700|S|Bronze Shield
        A800|S|Templar's Shield
        A900|S|Iron Shield
        AA00|S|Light Shield
        AB00|S|White Shield
        AC00|S|Steel Shield
        AD00|S|Magic Shield
        AE00|S|Dragon Shield
        B000|S|Power Shield
        B100|S|Saintess Shield
        B200|S|Bone Shield
        B300|S|Mirror Shield
        B400|S|Ogre Shield
        B500|S|Silver Shield
        B600|S|Big Boss Shield
        B700|S|Ruinous Shield
        B800|S|Thanatos' Shield
        B900|S|Goddess Shield
        BA00|S|Dragovian Shield
        BB00|S|Metal King Shield
        BC00|H|Bandana
        BD00|H|Leather Hat
        BE00|H|Hairband
        BF00|H|Pointy Hat
        C000|H|Feathered Cap
        C100|H|Turban
        C200|H|Silver Tiara
        C300|H|Bunny Ears
        C400|H|Stone Hardhat
        C500|H|Iron Helmet
        C600|H|Fur Hood
        C700|H|Bronze Helmet
        C800|H|Coral Hairpin
        C900|H|Pirate's Hat
        CA00|H|Iron Mask
        CB00|H|Hermes' Hat
        CC00|H|Platinum Headgear
        CD00|H|Magical Hat
        CE00|H|Happy Hat
        CF00|H|Hades' Helm
        D000|H|Slime Crown
        D100|H|Mythril Helm
        D200|H|Mercury's Bandana
        D300|H|Scholar's Cap
        D500|H|Thinking Cap
        D600|H|Iron Headgear
        D700|H|Raging Bull Helm
        D800|H|Golden Tiara
        D900|H|Great Helm
        DA00|H|Phantom Mask
        DB00|H|Skull Helm
        DC00|H|Dragovian Helm
        DD00|H|Sun Crown
        DE00|H|Metal King Helm
        E000|R|Agility Ring
        E100|R|Meteorite Bracer
        E200|R|Strength Ring
        E300|R|Gold Ring
        E400|R|Prayer Ring
        E500|R|Recovery Ring
        E600|R|Goddess Ring
        E700|R|Gospel Ring
        E800|R|Life Bracer
        E900|R|Mighty Armlet
        EA00|R|Gold Bracer
        ED00|R|Ruby Of Protection
        EE00|R|Titan Belt
        EF00|R|Scholar's Specs
        F000|R|Tough Guy Tattoo
        F100|R|Slime Earrings
        F200|R|Devil's Tail
        F300|R|Fishnet Stockings
        F400|R|Garter
        F500|R|Lady's Ring
        F600|R|Gold Rosary
        F800|R|Sorcerer's Ring
        F900|R|Skull Ring
        FA00|R|Holy Talisman
        FB00|R|Ring Of Awakening
        FC00|R|Full Moon Ring
        FD00|R|Ring Of Clarity
        FE00|R|Ring Of Truth
        FF00|R|Ring Of Immunity
        0001|R|Catholicon Ring
        0301|R|Dragon Scale
        0401|R|Elevating Shoes
        2901|R|Bunny Tail
        5601|R|Templar's Ring
        6901|R|Templar Captain's Ring
        6B01|R|Argon Ring
        0501|C|Medicinal Herb
        0601|C|Antidotal Herb
        0701|C|Moonwort Bulb
        0801|C|Strong Medicine
        0901|C|Special Medicine
        0A01|C|Lesser Panacea
        0B01|C|Greater Panacea
        0C01|C|Rose-Root
        0D01|C|Rose-Wort
        0E01|C|Strong Antidote
        0F01|C|Special Antidote
        1001|C|Moon's Mercy
        1101|C|Mystifying Mixture
        1201|C|Yggdrasil Leaf
        1301|C|Yggdrasil Dew
        1401|C|Amor Seco Essence
        1501|C|Chimaera Wing
        1601|C|Holy Water
        1701|C|Magic Water
        1801|C|Elfin Elixer
        1901|C|Rockbomb Shard
        1A01|C|Sage's Stone
        1C01|C|Seed Of Strength
        1D01|C|Seed Of Agility
        1E01|C|Seed Of Defense
        1F01|C|Seed Of Wisdom
        2001|C|Seed Of Skill
        2101|C|Seed Of Life
        2201|C|Seed Of Magic
        2A01|C|Timbrel Of Tension
        3C01|C|Plain Cheese
        3E01|C|Spicy Cheese
        3F01|C|Super Spicy Cheese
        4001|C|Scorching Cheese
        4201|C|Cool Cheese
        4301|C|Chilly Cheese
        4401|C|Cold Cheese
        4501|C|C-C-Cold Cheese
        4B01|C|Mild Cheese
        4C01|C|Cured Cheese
        4D01|C|Angel Cheese
        4E01|C|Hard Cheese
        4F01|C|Soft Cheese
        5001|C|Chunky Cheese
        5101|C|Highly Strung Cheese
        2301|K|Mini Medal
        2501|K|Thief's Key
        2601|K|Magic Key
        2701|K|Ultimate Key
        2D01|K|Baumren's Bell
        5301|K|Crystal Ball
        5401|K|Tool Bag
        5501|K|Jessica's Letter
        5701|K|World Map
        5801|K|Venus' Tear
        5901|K|Moonshadow Harp
        5A01|K|Sand Of Serenity
        5B01|K|Lizard Humour
        5D01|K|Argon Heart
        5E01|K|Great Big Argon Heart
        5F01|K|Magic Mirror
        6001|K|Sun Mirror
        6201|K|Kran Spinels
        6301|K|The Big Book Of Barriers
        6401|K|Marta's Bag
        6501|K|Illuminated Sea Chart
        6701|K|Godbird's Soulstone
        6801|K|Darktree Leaf
        6C01|K|Echo Flute
        6D01|K|Eros' Bow Recipe
        6E01|K|Imp Knife Recipe
        6F01|K|Dragon Slayer Recipe
        7001|K|Thief's Key Recipe
        7101|K|Morrie's Memo #1
        7201|K|Morrie's Memo #2
        7301|K|Morrie's Memo #3
        7401|K|Monster Arena Key
        7501|K|Godbird Sceptre
        7701|K|Gold Orb
        7801|K|Silver Orb
        7901|K|Red Orb
        7A01|K|Blue Orb
        7B01|K|Green Orb
        7C01|K|Yellow Orb
        7D01|K|Purple Orb
        7E01|K|Copper Monster Coin
        7F01|K|Silver Monster Coin
        8001|K|Gold Monster Coin
        2801|L|Orichalcum
        2E01|L|Gold Nugget
        2F01|L|Iron Nail
        3001|L|Cowpat
        3101|L|Dragon Dung
        3201|L|Rock Salt
        3401|L|Saint's Ashes
        3501|L|Wing Of Bat
        3601|L|Magic Beast Hide
        3701|L|Fresh Milk
        3801|L|Rennet Powder
        3901|L|Red Mould
        3A01|L|Waterweed Mould
        3B01|L|Premium Mould
        6601|L|Nook Grass
        """;
}
