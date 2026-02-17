using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public sealed class ShopSystem
    {
        public bool TryBuySkin(PlayerProfile profile, string skinId, out string message)
        {
            var item = CosmeticCatalog.GetById(skinId);
            if (item == null)
            {
                message = "Skin not found.";
                return false;
            }

            if (profile.OwnedSkinIds.Contains(item.Id))
            {
                message = "Skin already owned.";
                return false;
            }

            if (profile.Coins < item.PriceCoins)
            {
                message = "Not enough coins.";
                return false;
            }

            profile.Coins -= item.PriceCoins;
            profile.OwnedSkinIds.Add(item.Id);
            message = $"Unlocked {item.Name}.";
            return true;
        }

        public bool EquipSkin(PlayerProfile profile, string skinId, out string message)
        {
            if (!profile.OwnedSkinIds.Contains(skinId))
            {
                message = "Skin not owned.";
                return false;
            }

            profile.EquippedSkinId = skinId;
            message = "Skin equipped.";
            return true;
        }
    }
}
