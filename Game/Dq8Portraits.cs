using System.Reflection;

namespace DQ8SaveEditor.Game;

/// <summary>
/// Character profile portraits, embedded in the assembly and keyed by party slot
/// (0 = hero, 1 = Yangus, 2 = Jessica, 3 = Angelo). Cropped from the in-game
/// status menu. Loaded lazily and cached; unknown slots return null.
/// </summary>
public static class Dq8Portraits
{
    private const string ResourcePrefix = "DQ8SaveEditor.Assets.Portraits.";

    private static readonly Dictionary<int, string> SlotToName = new()
    {
        [0] = "jack",      // the hero (player-named, but this is its canonical portrait)
        [1] = "yangus",
        [2] = "jessica",
        [3] = "angelo",
    };

    private static readonly Dictionary<int, Image?> Cache = new();

    /// <summary>Portrait for a party slot, or null if none is bundled for it.</summary>
    public static Image? ForSlot(int slot)
    {
        if (Cache.TryGetValue(slot, out var cached))
        {
            return cached;
        }

        Image? img = null;
        if (SlotToName.TryGetValue(slot, out var name))
        {
            var asm = Assembly.GetExecutingAssembly();
            using Stream? s = asm.GetManifestResourceStream(ResourcePrefix + name + ".png");
            if (s is not null)
            {
                // Copy into a MemoryStream so the Image isn't tied to the disposed
                // manifest stream (GDI+ keeps the backing stream open otherwise).
                using var ms = new MemoryStream();
                s.CopyTo(ms);
                img = Image.FromStream(ms);
            }
        }

        Cache[slot] = img;
        return img;
    }
}
