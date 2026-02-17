using System.Collections.Generic;
using CodeForgeRush.Config;
using CodeForgeRush.Models;
using CodeForgeRush.Systems;

namespace CodeForgeRush.Gameplay
{
    public sealed class GameplayController
    {
        private readonly PuzzleSimulator _simulator;
        private readonly ProgressionSystem _progression;
        private readonly DailyMissionSystem _missions;
        private readonly SeasonPassSystem _seasonPass;
        private readonly LiveOpsConfig _config;

        public GameplayController(
            PuzzleSimulator simulator,
            ProgressionSystem progression,
            DailyMissionSystem missions,
            SeasonPassSystem seasonPass,
            LiveOpsConfig config)
        {
            _simulator = simulator;
            _progression = progression;
            _missions = missions;
            _seasonPass = seasonPass;
            _config = config;
        }

        public SimulationResult SubmitRun(PlayerProfile profile, LevelDefinition level, IReadOnlyList<CodeInstruction> instructions)
        {
            var result = _simulator.Run(level, instructions);
            if (!result.Success)
            {
                _progression.RegisterFailure(profile);
                return result;
            }

            bool perfect = result.StepsUsed <= level.ParMoves;
            _progression.CompleteLevel(profile, level.LevelNumber, perfect, result.CoinsCollected);
            _missions.RegisterLevelComplete(profile, perfect);

            bool bossClear = level.IsBossLevel && result.BossDefeated;
            if (bossClear)
                profile.Coins += _config.bossClearBonusCoins;

            _seasonPass.GrantProgressOnWin(profile, perfect, bossClear);
            return result;
        }
    }
}
