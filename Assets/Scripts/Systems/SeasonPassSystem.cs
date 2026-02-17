using CodeForgeRush.Config;
using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public sealed class SeasonPassSystem
    {
        private readonly LiveOpsConfig _config;

        public SeasonPassSystem(LiveOpsConfig config)
        {
            _config = config;
        }

        public void GrantProgressOnWin(PlayerProfile profile, bool perfect, bool bossClear)
        {
            int gain = _config.seasonBaseXp;
            if (perfect)
                gain += _config.seasonPerfectBonusXp;
            if (bossClear)
                gain += _config.seasonBossBonusXp;

            profile.SeasonXp += gain;
            while (profile.SeasonXp >= XpToNextTier(profile.SeasonTier))
            {
                profile.SeasonXp -= XpToNextTier(profile.SeasonTier);
                profile.SeasonTier++;
                profile.Coins += _config.seasonTierCoinBase + (profile.SeasonTier * _config.seasonTierCoinPerTier);
            }
        }

        private int XpToNextTier(int tier)
        {
            return _config.seasonTierBaseXp + (tier * _config.seasonTierXpPerTier);
        }
    }
}
