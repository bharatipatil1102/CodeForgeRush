using System;

namespace CodeForgeRush.Config
{
    [Serializable]
    public sealed class LiveOpsConfig
    {
        public int version = 1;

        public int levelsPerStage = 100;
        public int bossEveryLevels = 100;
        public int tutorialLevelsCount = 20;

        public int baseCoinReward = 10;
        public int baseXpReward = 15;
        public int perfectBonusCoins = 6;

        public int rankBaseXp = 80;
        public int rankXpPerRank = 15;

        public int dailyBaseRewardCoins = 20;
        public int dailyStreakBonusPerDay = 2;
        public int hintCostCoins = 30;
        public int reviveCostCoins = 20;
        public int rewardedHintTokens = 1;
        public int bossClearBonusCoins = 75;

        public int missionATarget = 5;
        public int missionARewardCoins = 40;
        public int missionBTarget = 2;
        public int missionBRewardCoins = 50;

        public int seasonBaseXp = 12;
        public int seasonPerfectBonusXp = 6;
        public int seasonBossBonusXp = 15;
        public int seasonTierBaseXp = 60;
        public int seasonTierXpPerTier = 12;
        public int seasonTierCoinBase = 30;
        public int seasonTierCoinPerTier = 2;

        public int normalGridBase = 5;
        public int bossGridBase = 8;
        public int normalGridStageDivisor = 4;
        public int bossGridStageDivisor = 6;
        public int maxGridSize = 12;
        public int maxInstructionsCap = 40;
        public int parMovesCap = 36;
        public int hazardCap = 12;
        public int enemyCap = 5;

        public static LiveOpsConfig Default()
        {
            return new LiveOpsConfig();
        }

        public void Sanitize()
        {
            levelsPerStage = Math.Clamp(levelsPerStage, 20, 500);
            bossEveryLevels = Math.Clamp(bossEveryLevels, 20, 500);
            tutorialLevelsCount = Math.Clamp(tutorialLevelsCount, 0, 100);

            rankBaseXp = Math.Clamp(rankBaseXp, 20, 500);
            rankXpPerRank = Math.Clamp(rankXpPerRank, 1, 100);

            hintCostCoins = Math.Clamp(hintCostCoins, 0, 1000);
            reviveCostCoins = Math.Clamp(reviveCostCoins, 0, 1000);
            rewardedHintTokens = Math.Clamp(rewardedHintTokens, 1, 10);

            missionATarget = Math.Clamp(missionATarget, 1, 50);
            missionBTarget = Math.Clamp(missionBTarget, 1, 50);

            maxGridSize = Math.Clamp(maxGridSize, 5, 20);
            maxInstructionsCap = Math.Clamp(maxInstructionsCap, 8, 80);
            parMovesCap = Math.Clamp(parMovesCap, 8, 120);
            hazardCap = Math.Clamp(hazardCap, 0, 30);
            enemyCap = Math.Clamp(enemyCap, 0, 15);
        }
    }
}
