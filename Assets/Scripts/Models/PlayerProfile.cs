using System;
using System.Collections.Generic;

namespace CodeForgeRush.Models
{
    [Serializable]
    public sealed class PlayerProfile
    {
        public int CurrentLevel = 1;
        public int HighestLevelCompleted = 0;
        public int Coins = 100;
        public int Xp = 0;
        public int Rank = 1;
        public int DailyStreak = 0;
        public string LastDailyClaimDateIso = string.Empty;
        public int TotalPerfectRuns = 0;
        public int TotalFailures = 0;

        public int MissionAProgress = 0;
        public int MissionBProgress = 0;
        public string MissionDateIso = string.Empty;

        public int HintTokens = 1;
        public int SeasonXp = 0;
        public int SeasonTier = 1;
        public bool SeasonPremium = false;

        public string EquippedSkinId = "default";
        public List<string> OwnedSkinIds = new List<string> { "default" };

        public bool TutorialCompleted = false;
        public int TutorialStepIndex = 0;
    }
}
