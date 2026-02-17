using System;
using CodeForgeRush.Config;
using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public sealed class DailyMissionSystem
    {
        private readonly LiveOpsConfig _config;

        public DailyMissionSystem(LiveOpsConfig config)
        {
            _config = config;
        }

        public void EnsureMissionsForToday(PlayerProfile profile, DateTime utcNow)
        {
            string today = utcNow.Date.ToString("yyyy-MM-dd");
            if (profile.MissionDateIso == today)
                return;

            profile.MissionDateIso = today;
            profile.MissionAProgress = 0;
            profile.MissionBProgress = 0;
        }

        public void RegisterLevelComplete(PlayerProfile profile, bool perfect)
        {
            profile.MissionAProgress += 1;
            if (perfect)
                profile.MissionBProgress += 1;
        }

        public int ClaimMissionRewards(PlayerProfile profile)
        {
            int reward = 0;
            if (profile.MissionAProgress >= _config.missionATarget)
            {
                reward += _config.missionARewardCoins;
                profile.MissionAProgress = -999;
            }
            if (profile.MissionBProgress >= _config.missionBTarget)
            {
                reward += _config.missionBRewardCoins;
                profile.MissionBProgress = -999;
            }

            if (reward > 0)
                profile.Coins += reward;

            return reward;
        }
    }
}
