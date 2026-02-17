using System;
using CodeForgeRush.Config;
using CodeForgeRush.Data;
using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public sealed class LevelGenerator
    {
        private readonly LiveOpsConfig _config;

        public LevelGenerator(LiveOpsConfig config)
        {
            _config = config;
        }

        public LevelDefinition Generate(int levelNumber)
        {
            levelNumber = Math.Clamp(levelNumber, 1, GameBalance.MaxLevel);
            if (levelNumber <= _config.tutorialLevelsCount && TutorialLevelFactory.TryCreate(levelNumber, out var tutorialLevel))
                return tutorialLevel;

            int stage = StageFromLevel(levelNumber);
            bool isBoss = levelNumber % _config.bossEveryLevels == 0;

            var rng = new Random(levelNumber * 7919);
            int size = isBoss
                ? Math.Clamp(_config.bossGridBase + (stage / Math.Max(1, _config.bossGridStageDivisor)), _config.normalGridBase, _config.maxGridSize)
                : Math.Clamp(_config.normalGridBase + (stage / Math.Max(1, _config.normalGridStageDivisor)), _config.normalGridBase, _config.maxGridSize);

            var level = new LevelDefinition
            {
                LevelNumber = levelNumber,
                Width = size,
                Height = size,
                StartX = 0,
                StartY = 0,
                StartDirection = 1,
                GoalX = size - 1,
                GoalY = size - 1,
                HasLoops = stage >= 3,
                HasConditions = stage >= 7,
                HasFunction = stage >= 12,
                MaxInstructions = Math.Clamp(6 + stage + (isBoss ? 4 : 0), 8, _config.maxInstructionsCap),
                ParMoves = Math.Clamp((size * 2) + (stage / 2) + (isBoss ? 4 : 0), 8, _config.parMovesCap),
                IsBossLevel = isBoss,
                BossX = size / 2,
                BossY = size / 2,
                BossHealth = isBoss ? Math.Clamp(3 + (stage / 5), 3, 12) : 0
            };

            AddWalls(level, rng, stage, isBoss);
            AddCoins(level, rng, stage, isBoss);
            AddHazards(level, rng, stage, isBoss);
            AddEnemies(level, rng, stage, isBoss);
            EnsureCriticalTilesAreClear(level);

            return level;
        }

        private void AddWalls(LevelDefinition level, Random rng, int stage, bool isBoss)
        {
            int maxWalls = (level.Width * level.Height) / (isBoss ? 4 : 3);
            int budget = isBoss
                ? Math.Clamp((stage / 2) + 2, 2, maxWalls)
                : Math.Clamp(stage + 2, 2, maxWalls);

            for (int i = 0; i < budget; i++)
            {
                int x = rng.Next(0, level.Width);
                int y = rng.Next(0, level.Height);
                level.WallTiles.Add(level.ToIndex(x, y));
            }
        }

        private void AddCoins(LevelDefinition level, Random rng, int stage, bool isBoss)
        {
            int coinCount = isBoss
                ? Math.Clamp(5 + (stage / 4), 5, 14)
                : Math.Clamp(2 + (stage / 3), 2, 10);

            int safety = 0;
            while (level.CoinTiles.Count < coinCount && safety < 500)
            {
                int x = rng.Next(0, level.Width);
                int y = rng.Next(0, level.Height);
                int idx = level.ToIndex(x, y);
                if (!level.WallTiles.Contains(idx))
                    level.CoinTiles.Add(idx);
                safety++;
            }
        }

        private void AddHazards(LevelDefinition level, Random rng, int stage, bool isBoss)
        {
            int hazardCount = isBoss
                ? Math.Clamp(3 + (stage / 4), 3, _config.hazardCap)
                : Math.Clamp((stage / 3), 0, _config.hazardCap);

            int safety = 0;
            while (level.HazardTiles.Count < hazardCount && safety < 500)
            {
                int x = rng.Next(0, level.Width);
                int y = rng.Next(0, level.Height);
                int idx = level.ToIndex(x, y);
                if (!level.WallTiles.Contains(idx))
                    level.HazardTiles.Add(idx);
                safety++;
            }
        }

        private void AddEnemies(LevelDefinition level, Random rng, int stage, bool isBoss)
        {
            int enemyCount = isBoss
                ? Math.Clamp(2 + (stage / 8), 2, _config.enemyCap)
                : Math.Clamp(stage / 6, 0, _config.enemyCap);

            for (int i = 0; i < enemyCount; i++)
            {
                int x = rng.Next(0, level.Width);
                int y = rng.Next(0, level.Height);
                int idx = level.ToIndex(x, y);
                if (level.WallTiles.Contains(idx))
                    continue;

                level.Enemies.Add(new EnemyDefinition
                {
                    StartX = x,
                    StartY = y,
                    Horizontal = rng.NextDouble() > 0.5,
                    Range = rng.Next(1, Math.Min(4, level.Width - 1))
                });
            }
        }

        private static void EnsureCriticalTilesAreClear(LevelDefinition level)
        {
            int startIdx = level.ToIndex(level.StartX, level.StartY);
            int goalIdx = level.ToIndex(level.GoalX, level.GoalY);
            int bossIdx = level.ToIndex(level.BossX, level.BossY);

            level.WallTiles.Remove(startIdx);
            level.WallTiles.Remove(goalIdx);
            level.CoinTiles.Remove(startIdx);
            level.HazardTiles.Remove(startIdx);
            level.HazardTiles.Remove(goalIdx);

            if (level.IsBossLevel)
            {
                level.WallTiles.Remove(bossIdx);
                level.HazardTiles.Remove(bossIdx);
            }

            level.Enemies.RemoveAll(e =>
                (e.StartX == level.StartX && e.StartY == level.StartY) ||
                (e.StartX == level.GoalX && e.StartY == level.GoalY) ||
                (level.IsBossLevel && e.StartX == level.BossX && e.StartY == level.BossY));
        }

        private int StageFromLevel(int level)
        {
            if (level < 1)
                return 1;

            return ((level - 1) / _config.levelsPerStage) + 1;
        }
    }
}
