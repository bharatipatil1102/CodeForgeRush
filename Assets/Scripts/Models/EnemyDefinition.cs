using System;

namespace CodeForgeRush.Models
{
    [Serializable]
    public sealed class EnemyDefinition
    {
        public int StartX;
        public int StartY;
        public bool Horizontal;
        public int Range;
    }
}
