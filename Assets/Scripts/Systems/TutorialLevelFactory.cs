using CodeForgeRush.Models;

namespace CodeForgeRush.Systems
{
    public static class TutorialLevelFactory
    {
        public static bool TryCreate(int levelNumber, out LevelDefinition level)
        {
            level = null;
            if (levelNumber < 1 || levelNumber > 20)
                return false;

            level = Base(levelNumber);
            Build(levelNumber, level);
            return true;
        }

        private static LevelDefinition Base(int levelNumber)
        {
            return new LevelDefinition
            {
                LevelNumber = levelNumber,
                Width = 5,
                Height = 5,
                StartX = 0,
                StartY = 0,
                StartDirection = 1,
                GoalX = 4,
                GoalY = 4,
                MaxInstructions = 8,
                ParMoves = 8,
                HasLoops = levelNumber >= 7,
                HasConditions = levelNumber >= 13,
                HasFunction = levelNumber >= 18,
                IsBossLevel = false
            };
        }

        private static void Build(int n, LevelDefinition l)
        {
            switch (n)
            {
                case 1: PathCoins(l, "RRRRDDDD"); break;
                case 2: Walls(l, (2,0), (2,1)); PathCoins(l, "RRDDRRDD"); break;
                case 3: Walls(l, (1,1), (1,2), (3,3)); PathCoins(l, "RDRDRDDD"); break;
                case 4: Hazards(l, (2,2)); PathCoins(l, "RRDDRRDD"); break;
                case 5: Enemies(l, (2,1,true,2)); PathCoins(l, "RRRDRDDD"); break;
                case 6: Hazards(l, (3,1), (3,2)); Walls(l, (1,3)); PathCoins(l, "RRDRRDDD"); break;
                case 7: l.MaxInstructions = 7; l.ParMoves = 6; PathCoins(l, "RRRRDD"); break;
                case 8: l.MaxInstructions = 7; Walls(l, (2,2), (2,3)); PathCoins(l, "RRDDRR"); break;
                case 9: l.MaxInstructions = 7; Hazards(l, (1,2), (2,2)); PathCoins(l, "RRRDDD"); break;
                case 10: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 9; l.ParMoves = 8; Walls(l, (2,2), (3,2)); PathCoins(l, "RRRDRRDDDD"); break;
                case 11: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 10; Hazards(l, (2,1), (2,2), (2,3)); PathCoins(l, "RRRRDDDDD"); break;
                case 12: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 10; Enemies(l, (3,1,false,3)); PathCoins(l, "RRDRRDDDD"); break;
                case 13: l.MaxInstructions = 11; l.ParMoves = 9; Hazards(l, (1,1), (2,1)); PathCoins(l, "RRRDRDDD"); break;
                case 14: l.MaxInstructions = 11; Walls(l, (2,0), (2,1), (2,2)); PathCoins(l, "RDDRRRDD"); break;
                case 15: l.MaxInstructions = 12; Hazards(l, (3,3), (2,3)); Enemies(l, (1,2,true,2)); PathCoins(l, "RRRRDDDD"); break;
                case 16: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 12; Walls(l, (1,2), (2,2), (3,2)); Hazards(l, (4,1), (4,2)); PathCoins(l, "RRRDRRDDD"); break;
                case 17: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 13; Enemies(l, (2,1,true,3), (4,3,false,2)); PathCoins(l, "RRDDRRDDD"); break;
                case 18: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 14; l.HasFunction = true; Walls(l, (2,4), (3,4)); PathCoins(l, "RRRDDRDDD"); break;
                case 19: l.Width = 6; l.Height = 6; l.GoalX = 5; l.GoalY = 5; l.MaxInstructions = 14; l.HasFunction = true; Hazards(l, (1,4), (2,4), (3,4)); PathCoins(l, "RRRRDDDDD"); break;
                case 20: l.Width = 7; l.Height = 7; l.GoalX = 6; l.GoalY = 6; l.MaxInstructions = 16; l.ParMoves = 13; l.HasFunction = true; Hazards(l, (2,2), (3,2), (4,2)); Walls(l, (1,4), (2,4), (3,4)); Enemies(l, (4,1,false,3)); PathCoins(l, "RRRRRDDDDDD"); break;
            }

            CleanupCritical(l);
        }

        private static void PathCoins(LevelDefinition l, string path)
        {
            int x = l.StartX;
            int y = l.StartY;
            for (int i = 0; i < path.Length; i++)
            {
                switch (path[i])
                {
                    case 'R': x++; break;
                    case 'L': x--; break;
                    case 'D': y++; break;
                    case 'U': y--; break;
                }

                if (x >= 0 && y >= 0 && x < l.Width && y < l.Height)
                    l.CoinTiles.Add(l.ToIndex(x, y));
            }
        }

        private static void Walls(LevelDefinition l, params (int x, int y)[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
                l.WallTiles.Add(l.ToIndex(cells[i].x, cells[i].y));
        }

        private static void Hazards(LevelDefinition l, params (int x, int y)[] cells)
        {
            for (int i = 0; i < cells.Length; i++)
                l.HazardTiles.Add(l.ToIndex(cells[i].x, cells[i].y));
        }

        private static void Enemies(LevelDefinition l, params (int x, int y, bool horizontal, int range)[] defs)
        {
            for (int i = 0; i < defs.Length; i++)
            {
                l.Enemies.Add(new EnemyDefinition
                {
                    StartX = defs[i].x,
                    StartY = defs[i].y,
                    Horizontal = defs[i].horizontal,
                    Range = defs[i].range
                });
            }
        }

        private static void CleanupCritical(LevelDefinition l)
        {
            int start = l.ToIndex(l.StartX, l.StartY);
            int goal = l.ToIndex(l.GoalX, l.GoalY);
            l.WallTiles.Remove(start);
            l.WallTiles.Remove(goal);
            l.HazardTiles.Remove(start);
            l.HazardTiles.Remove(goal);
            l.CoinTiles.Remove(start);
            l.Enemies.RemoveAll(e =>
                (e.StartX == l.StartX && e.StartY == l.StartY) ||
                (e.StartX == l.GoalX && e.StartY == l.GoalY));
        }
    }
}
