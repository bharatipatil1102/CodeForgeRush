using System.Collections.Generic;
using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public static class CosmeticCatalog
    {
        public static readonly IReadOnlyList<CosmeticItem> Items = new List<CosmeticItem>
        {
            new CosmeticItem { Id = "default", Name = "Default Bot", PriceCoins = 0 },
            new CosmeticItem { Id = "neon_blade", Name = "Neon Blade", PriceCoins = 250 },
            new CosmeticItem { Id = "pixel_fox", Name = "Pixel Fox", PriceCoins = 400 },
            new CosmeticItem { Id = "quantum_ace", Name = "Quantum Ace", PriceCoins = 650 },
            new CosmeticItem { Id = "boss_hunter", Name = "Boss Hunter", PriceCoins = 900 }
        };

        public static CosmeticItem GetById(string id)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Id == id)
                    return Items[i];
            }

            return null;
        }
    }
}
