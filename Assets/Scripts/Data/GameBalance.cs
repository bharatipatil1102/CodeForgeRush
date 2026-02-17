namespace CodeForgeRush.Data
{
    public static class GameBalance
    {
        public const int MaxLevel = 15000;
        public const int BaseCoinReward = 10;
        public const int BaseXpReward = 15;
        public const int PerfectBonusCoins = 6;
        public const int DailyStreakBonusPerDay = 2;
        public const int HintCostCoins = 30;
        public const int ReviveCostCoins = 20;

        public static int StageFromLevel(int level)
        {
            if (level < 1) return 1;
            return ((level - 1) / 100) + 1;
        }
    }
}
