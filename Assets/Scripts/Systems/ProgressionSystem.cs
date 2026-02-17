using CodeForgeRush.Config;
using CodeForgeRush.Data;
using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public sealed class ProgressionSystem
    {
        private readonly LiveOpsConfig _config;

        public ProgressionSystem(LiveOpsConfig config)
        {
            _config = config;
        }

        public void CompleteLevel(PlayerProfile profile, int levelNumber, bool perfect, int collectedCoins)
        {
            int stage = StageFromLevel(levelNumber);
            int xpGain = _config.baseXpReward + (stage * 2);
            int coinGain = _config.baseCoinReward + stage + collectedCoins;

            if (perfect)
            {
                coinGain += _config.perfectBonusCoins;
                profile.TotalPerfectRuns++;
            }

            profile.Xp += xpGain;
            profile.Coins += coinGain;

            if (levelNumber > profile.HighestLevelCompleted)
                profile.HighestLevelCompleted = levelNumber;

            if (levelNumber >= profile.CurrentLevel)
                profile.CurrentLevel = levelNumber + 1;

            while (profile.Xp >= XpToNextRank(profile.Rank))
            {
                profile.Xp -= XpToNextRank(profile.Rank);
                profile.Rank++;
            }

            if (profile.CurrentLevel > GameBalance.MaxLevel)
                profile.CurrentLevel = GameBalance.MaxLevel;
        }

        public void RegisterFailure(PlayerProfile profile)
        {
            profile.TotalFailures++;
        }

        private int XpToNextRank(int rank)
        {
            return _config.rankBaseXp + (rank * _config.rankXpPerRank);
        }

        private int StageFromLevel(int level)
        {
            if (level < 1)
                return 1;

            return ((level - 1) / _config.levelsPerStage) + 1;
        }
    }
}
