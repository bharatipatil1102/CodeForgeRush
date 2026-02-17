using System;
using CodeForgeRush.Config;
using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public sealed class RewardSystem
    {
        private readonly LiveOpsConfig _config;

        public RewardSystem(LiveOpsConfig config)
        {
            _config = config;
        }

        public int ClaimDailyStreak(PlayerProfile profile, DateTime utcNow)
        {
            string today = utcNow.Date.ToString("yyyy-MM-dd");
            if (profile.LastDailyClaimDateIso == today)
                return 0;

            DateTime yesterday = utcNow.Date.AddDays(-1);
            if (profile.LastDailyClaimDateIso == yesterday.ToString("yyyy-MM-dd"))
                profile.DailyStreak++;
            else
                profile.DailyStreak = 1;

            profile.LastDailyClaimDateIso = today;

            int reward = _config.dailyBaseRewardCoins + (profile.DailyStreak * _config.dailyStreakBonusPerDay);
            profile.Coins += reward;
            return reward;
        }

        public bool TrySpendHints(PlayerProfile profile)
        {
            if (profile.HintTokens > 0)
            {
                profile.HintTokens--;
                return true;
            }

            if (profile.Coins < _config.hintCostCoins)
                return false;
            profile.Coins -= _config.hintCostCoins;
            return true;
        }

        public bool TryRevive(PlayerProfile profile)
        {
            if (profile.Coins < _config.reviveCostCoins)
                return false;
            profile.Coins -= _config.reviveCostCoins;
            return true;
        }

        public void GrantRewardedHint(PlayerProfile profile)
        {
            profile.HintTokens += _config.rewardedHintTokens;
        }
    }
}
