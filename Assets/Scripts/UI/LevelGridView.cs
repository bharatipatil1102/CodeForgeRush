using CodeForgeRush.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class LevelGridView : MonoBehaviour
    {
        [SerializeField] private RectTransform gridRoot;
        [SerializeField] private Image cellPrefab;
        [SerializeField] private GridLayoutGroup gridLayout;

        [Header("Colors")]
        [SerializeField] private Color emptyColor = new Color(0.12f, 0.14f, 0.18f);
        [SerializeField] private Color wallColor = new Color(0.32f, 0.18f, 0.18f);
        [SerializeField] private Color coinColor = new Color(0.95f, 0.82f, 0.25f);
        [SerializeField] private Color hazardColor = new Color(0.88f, 0.35f, 0.1f);
        [SerializeField] private Color enemyColor = new Color(0.9f, 0.15f, 0.22f);
        [SerializeField] private Color bossColor = new Color(0.58f, 0.18f, 0.78f);
        [SerializeField] private Color startColor = new Color(0.22f, 0.78f, 0.32f);
        [SerializeField] private Color goalColor = new Color(0.2f, 0.6f, 0.95f);

        public void Render(LevelDefinition level)
        {
            if (gridRoot == null || cellPrefab == null || gridLayout == null)
                return;

            for (int i = gridRoot.childCount - 1; i >= 0; i--)
                Destroy(gridRoot.GetChild(i).gameObject);

            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = level.Width;

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    int idx = level.ToIndex(x, y);
                    Image cell = Instantiate(cellPrefab, gridRoot);
                    cell.color = ResolveCellColor(level, x, y, idx);
                }
            }
        }

        private Color ResolveCellColor(LevelDefinition level, int x, int y, int idx)
        {
            if (x == level.StartX && y == level.StartY)
                return startColor;

            if (x == level.GoalX && y == level.GoalY)
                return goalColor;

            if (level.WallTiles.Contains(idx))
                return wallColor;

            if (level.IsBossLevel && x == level.BossX && y == level.BossY)
                return bossColor;

            if (level.HazardTiles.Contains(idx))
                return hazardColor;

            for (int i = 0; i < level.Enemies.Count; i++)
            {
                if (level.Enemies[i].StartX == x && level.Enemies[i].StartY == y)
                    return enemyColor;
            }

            if (level.CoinTiles.Contains(idx))
                return coinColor;

            return emptyColor;
        }
    }
}
