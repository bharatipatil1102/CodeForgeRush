using System;
using System.Collections.Generic;
using CodeForgeRush.Config;
using CodeForgeRush.Gameplay;
using CodeForgeRush.Models;
using CodeForgeRush.Systems;
using UnityEngine;

namespace CodeForgeRush.Core
{
    public sealed class GameManager : MonoBehaviour
    {
        private SaveSystem _saveSystem;
        private LiveOpsConfig _liveOpsConfig;
        private LevelGenerator _levelGenerator;
        private RewardSystem _rewardSystem;
        private DailyMissionSystem _dailyMissionSystem;
        private ShopSystem _shopSystem;
        private GameplayController _gameplayController;

        private PlayerProfile _profile;
        private LevelDefinition _currentLevel;

        public event Action<LevelDefinition> LevelLoaded;
        public event Action<PlayerProfile> ProfileChanged;
        public event Action<SimulationResult> RunResolved;

        private void Awake()
        {
            _saveSystem = new SaveSystem();
            _liveOpsConfig = new LiveOpsConfigService().Load();
            _levelGenerator = new LevelGenerator(_liveOpsConfig);
            _rewardSystem = new RewardSystem(_liveOpsConfig);
            _dailyMissionSystem = new DailyMissionSystem(_liveOpsConfig);
            _shopSystem = new ShopSystem();

            var progression = new ProgressionSystem(_liveOpsConfig);
            var seasonPass = new SeasonPassSystem(_liveOpsConfig);
            _gameplayController = new GameplayController(
                new PuzzleSimulator(),
                progression,
                _dailyMissionSystem,
                seasonPass,
                _liveOpsConfig);

            _profile = _saveSystem.Load();
            if (_profile.OwnedSkinIds == null || _profile.OwnedSkinIds.Count == 0)
                _profile.OwnedSkinIds = new List<string> { "default" };
            if (string.IsNullOrWhiteSpace(_profile.EquippedSkinId))
                _profile.EquippedSkinId = "default";

            _dailyMissionSystem.EnsureMissionsForToday(_profile, DateTime.UtcNow);
            _rewardSystem.ClaimDailyStreak(_profile, DateTime.UtcNow);

            LoadCurrentLevel();
            _saveSystem.Save(_profile);
            ProfileChanged?.Invoke(_profile);

            Debug.Log($"CodeForge Rush booted. Level {_profile.CurrentLevel}, Coins {_profile.Coins}, Rank {_profile.Rank}");
        }

        public LevelDefinition CurrentLevel => _currentLevel;
        public PlayerProfile Profile => _profile;
        public int MissionATarget => _liveOpsConfig != null ? _liveOpsConfig.missionATarget : 5;
        public int MissionBTarget => _liveOpsConfig != null ? _liveOpsConfig.missionBTarget : 2;
        public int TutorialLevelsCount => _liveOpsConfig != null ? _liveOpsConfig.tutorialLevelsCount : 20;

        public void LoadCurrentLevel()
        {
            _currentLevel = _levelGenerator.Generate(_profile.CurrentLevel);
            LevelLoaded?.Invoke(_currentLevel);
            int stage = ((_currentLevel.LevelNumber - 1) / _liveOpsConfig.levelsPerStage) + 1;
            Debug.Log($"Loaded level {_currentLevel.LevelNumber} (Stage {stage})");
        }

        public SimulationResult SubmitProgram(IReadOnlyList<CodeInstruction> instructions)
        {
            var result = _gameplayController.SubmitRun(_profile, _currentLevel, instructions);
            RunResolved?.Invoke(result);
            ProfileChanged?.Invoke(_profile);

            _saveSystem.Save(_profile);

            if (result.Success)
            {
                LoadCurrentLevel();
                ProfileChanged?.Invoke(_profile);
                _saveSystem.Save(_profile);
            }

            Debug.Log($"Run success={result.Success}, steps={result.StepsUsed}, collected={result.CoinsCollected}, err={result.Error}");
            return result;
        }

        public int ClaimMissionRewards()
        {
            int reward = _dailyMissionSystem.ClaimMissionRewards(_profile);
            if (reward > 0)
            {
                ProfileChanged?.Invoke(_profile);
                _saveSystem.Save(_profile);
            }

            return reward;
        }

        public bool TrySpendHint(out string message)
        {
            bool ok = _rewardSystem.TrySpendHints(_profile);
            message = ok ? "Hint unlocked." : "Need more coins for hint.";
            if (ok)
            {
                ProfileChanged?.Invoke(_profile);
                _saveSystem.Save(_profile);
            }

            return ok;
        }

        public void ClaimRewardedHint()
        {
            _rewardSystem.GrantRewardedHint(_profile);
            ProfileChanged?.Invoke(_profile);
            _saveSystem.Save(_profile);
        }

        public void AdvanceTutorialStep()
        {
            if (_profile.TutorialCompleted)
                return;

            _profile.TutorialStepIndex++;
            ProfileChanged?.Invoke(_profile);
            _saveSystem.Save(_profile);
        }

        public void MarkTutorialCompleted()
        {
            if (_profile.TutorialCompleted)
                return;

            _profile.TutorialCompleted = true;
            _profile.TutorialStepIndex = 999;
            ProfileChanged?.Invoke(_profile);
            _saveSystem.Save(_profile);
        }

        public bool TryBuySkin(string skinId, out string message)
        {
            bool ok = _shopSystem.TryBuySkin(_profile, skinId, out message);
            if (ok)
            {
                ProfileChanged?.Invoke(_profile);
                _saveSystem.Save(_profile);
            }

            return ok;
        }

        public bool TryEquipSkin(string skinId, out string message)
        {
            bool ok = _shopSystem.EquipSkin(_profile, skinId, out message);
            if (ok)
            {
                ProfileChanged?.Invoke(_profile);
                _saveSystem.Save(_profile);
            }

            return ok;
        }

        public void SubmitDemoProgram()
        {
            var demoCode = new List<CodeInstruction>
            {
                new CodeInstruction(OpCode.MoveForward),
                new CodeInstruction(OpCode.MoveForward),
                new CodeInstruction(OpCode.TurnRight),
                new CodeInstruction(OpCode.MoveForward),
                new CodeInstruction(OpCode.AttackAhead),
                new CodeInstruction(OpCode.MoveForward)
            };

            SubmitProgram(demoCode);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                _saveSystem.Save(_profile);
        }

        private void OnApplicationQuit()
        {
            _saveSystem.Save(_profile);
        }
    }
}
