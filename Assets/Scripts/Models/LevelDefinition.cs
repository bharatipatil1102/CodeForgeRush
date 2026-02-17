using System.Collections.Generic;

namespace CodeForgeRush.Models
{
    public sealed class LevelDefinition
    {
        public int LevelNumber;
        public int Width;
        public int Height;
        public int StartX;
        public int StartY;
        public int StartDirection;
        public int GoalX;
        public int GoalY;
        public int MaxInstructions;
        public int ParMoves;
        public bool HasLoops;
        public bool HasConditions;
        public bool HasFunction;

        public bool IsBossLevel;
        public int BossX;
        public int BossY;
        public int BossHealth;

        public HashSet<int> WallTiles = new HashSet<int>();
        public HashSet<int> CoinTiles = new HashSet<int>();
        public HashSet<int> HazardTiles = new HashSet<int>();
        public List<EnemyDefinition> Enemies = new List<EnemyDefinition>();

        public int ToIndex(int x, int y) => y * Width + x;
    }
}
