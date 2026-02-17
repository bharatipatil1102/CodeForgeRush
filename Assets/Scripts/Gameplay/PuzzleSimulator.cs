using System;
using System.Collections.Generic;
using CodeForgeRush.Models;

namespace CodeForgeRush.Gameplay
{
    public sealed class SimulationResult
    {
        public bool Success;
        public int StepsUsed;
        public int CoinsCollected;
        public int HealthRemaining;
        public int BossDamageDealt;
        public bool BossDefeated;
        public string Error = string.Empty;
    }

    public sealed class PuzzleSimulator
    {
        private const int DirectionUp = 0;
        private const int DirectionRight = 1;
        private const int DirectionDown = 2;
        private const int DirectionLeft = 3;

        private sealed class EnemyState
        {
            public EnemyDefinition Definition;
            public bool Alive = true;
            public int X;
            public int Y;
        }

        public SimulationResult Run(LevelDefinition level, IReadOnlyList<CodeInstruction> code)
        {
            var result = new SimulationResult { HealthRemaining = 3 };

            if (code.Count > level.MaxInstructions)
            {
                result.Error = "Too many instructions.";
                return result;
            }

            int x = level.StartX;
            int y = level.StartY;
            int dir = level.StartDirection;
            int steps = 0;
            int bossHealth = level.IsBossLevel ? level.BossHealth : 0;

            var coins = new HashSet<int>(level.CoinTiles);
            var funcA = ExtractFunctionA(code);
            var enemies = BuildEnemyStates(level);

            int ip = 0;
            var loopStack = new Stack<(int StartIp, int Remaining)>();

            while (ip < code.Count && steps < 512)
            {
                CodeInstruction ins = code[ip];
                bool actionConsumed = false;

                switch (ins.OpCode)
                {
                    case OpCode.MoveForward:
                        TryMove(level, ref x, ref y, dir);
                        actionConsumed = true;
                        break;

                    case OpCode.TurnLeft:
                        dir = (dir + 3) % 4;
                        actionConsumed = true;
                        break;

                    case OpCode.TurnRight:
                        dir = (dir + 1) % 4;
                        actionConsumed = true;
                        break;

                    case OpCode.AttackAhead:
                        int tx = x;
                        int ty = y;
                        StepInDirection(dir, ref tx, ref ty);

                        if (tx >= 0 && ty >= 0 && tx < level.Width && ty < level.Height)
                        {
                            if (level.IsBossLevel && tx == level.BossX && ty == level.BossY && bossHealth > 0)
                            {
                                bossHealth--;
                                result.BossDamageDealt++;
                                result.BossDefeated = bossHealth <= 0;
                            }

                            for (int i = 0; i < enemies.Count; i++)
                            {
                                if (enemies[i].Alive && enemies[i].X == tx && enemies[i].Y == ty)
                                {
                                    enemies[i].Alive = false;
                                    break;
                                }
                            }
                        }
                        actionConsumed = true;
                        break;

                    case OpCode.Loop:
                        int loopCount = Math.Clamp(ins.Argument <= 0 ? 2 : ins.Argument, 1, 6);
                        loopStack.Push((ip + 1, loopCount));
                        break;

                    case OpCode.EndLoop:
                        if (loopStack.Count == 0)
                        {
                            result.Error = "Loop end without loop start.";
                            return result;
                        }
                        var top = loopStack.Pop();
                        if (top.Remaining > 1)
                        {
                            loopStack.Push((top.StartIp, top.Remaining - 1));
                            ip = top.StartIp;
                            continue;
                        }
                        break;

                    case OpCode.IfCoinAhead:
                        if (!CoinAhead(level, coins, x, y, dir))
                            ip = SkipToMatchingEndIf(code, ip);
                        break;

                    case OpCode.EndIf:
                        break;

                    case OpCode.CallFuncA:
                        if (funcA.Count == 0)
                        {
                            result.Error = "Function A is empty.";
                            return result;
                        }

                        int nested = RunFunction(level, funcA, ref x, ref y, ref dir, coins, enemies, ref bossHealth, ref result);
                        steps += nested;
                        break;

                    case OpCode.FuncAMarker:
                    case OpCode.EndFunc:
                        break;
                }

                if (actionConsumed)
                {
                    steps++;
                    ApplyWorldTick(level, ref x, ref y, steps, coins, enemies, ref bossHealth, ref result);

                    if (result.HealthRemaining <= 0)
                    {
                        result.Success = false;
                        result.StepsUsed = steps;
                        result.Error = "Bot destroyed by hazards/enemies.";
                        return result;
                    }
                }

                CollectCoinAtPosition(level, coins, x, y, ref result);

                bool atGoal = x == level.GoalX && y == level.GoalY;
                if (!level.IsBossLevel && atGoal)
                {
                    result.Success = true;
                    result.StepsUsed = steps;
                    return result;
                }

                if (level.IsBossLevel && atGoal && bossHealth <= 0)
                {
                    result.Success = true;
                    result.BossDefeated = true;
                    result.StepsUsed = steps;
                    return result;
                }

                ip++;
            }

            result.Success = false;
            result.StepsUsed = steps;
            result.BossDefeated = bossHealth <= 0;
            if (string.IsNullOrEmpty(result.Error))
            {
                result.Error = level.IsBossLevel && bossHealth > 0
                    ? "Defeat boss, then reach GOAL."
                    : "Goal not reached.";
            }

            return result;
        }

        private static void ApplyWorldTick(
            LevelDefinition level,
            ref int playerX,
            ref int playerY,
            int steps,
            HashSet<int> coins,
            List<EnemyState> enemies,
            ref int bossHealth,
            ref SimulationResult result)
        {
            int playerIdx = level.ToIndex(playerX, playerY);
            if (level.HazardTiles.Contains(playerIdx))
                result.HealthRemaining--;

            for (int i = 0; i < enemies.Count; i++)
            {
                var enemy = enemies[i];
                if (!enemy.Alive)
                    continue;

                ResolveEnemyPosition(enemy, steps, level, out int ex, out int ey);
                enemy.X = ex;
                enemy.Y = ey;

                if (enemy.X == playerX && enemy.Y == playerY)
                    result.HealthRemaining--;
            }

            if (level.IsBossLevel && bossHealth > 0 && playerX == level.BossX && playerY == level.BossY)
                result.HealthRemaining = 0;

            CollectCoinAtPosition(level, coins, playerX, playerY, ref result);
        }

        private static List<EnemyState> BuildEnemyStates(LevelDefinition level)
        {
            var list = new List<EnemyState>(level.Enemies.Count);
            for (int i = 0; i < level.Enemies.Count; i++)
            {
                list.Add(new EnemyState
                {
                    Definition = level.Enemies[i],
                    X = level.Enemies[i].StartX,
                    Y = level.Enemies[i].StartY
                });
            }

            return list;
        }

        private static void ResolveEnemyPosition(EnemyState enemy, int step, LevelDefinition level, out int x, out int y)
        {
            int range = Math.Max(1, enemy.Definition.Range);
            int period = range * 2;
            int t = period == 0 ? 0 : step % period;
            int offset = t <= range ? t : (period - t);

            x = enemy.Definition.StartX;
            y = enemy.Definition.StartY;

            if (enemy.Definition.Horizontal)
                x = Math.Clamp(enemy.Definition.StartX + offset, 0, level.Width - 1);
            else
                y = Math.Clamp(enemy.Definition.StartY + offset, 0, level.Height - 1);

            int idx = level.ToIndex(x, y);
            if (level.WallTiles.Contains(idx))
            {
                x = enemy.Definition.StartX;
                y = enemy.Definition.StartY;
            }
        }

        private static void CollectCoinAtPosition(LevelDefinition level, HashSet<int> coins, int x, int y, ref SimulationResult result)
        {
            int tile = level.ToIndex(x, y);
            if (coins.Contains(tile))
            {
                coins.Remove(tile);
                result.CoinsCollected++;
            }
        }

        private static void TryMove(LevelDefinition level, ref int x, ref int y, int dir)
        {
            int nx = x;
            int ny = y;
            StepInDirection(dir, ref nx, ref ny);

            if (nx < 0 || ny < 0 || nx >= level.Width || ny >= level.Height)
                return;

            int idx = level.ToIndex(nx, ny);
            if (level.WallTiles.Contains(idx))
                return;

            x = nx;
            y = ny;
        }

        private static void StepInDirection(int dir, ref int x, ref int y)
        {
            switch (dir)
            {
                case DirectionUp: y--; break;
                case DirectionRight: x++; break;
                case DirectionDown: y++; break;
                case DirectionLeft: x--; break;
            }
        }

        private static bool CoinAhead(LevelDefinition level, HashSet<int> coins, int x, int y, int dir)
        {
            int nx = x;
            int ny = y;
            StepInDirection(dir, ref nx, ref ny);

            if (nx < 0 || ny < 0 || nx >= level.Width || ny >= level.Height)
                return false;

            return coins.Contains(level.ToIndex(nx, ny));
        }

        private static int SkipToMatchingEndIf(IReadOnlyList<CodeInstruction> code, int startIp)
        {
            int depth = 0;
            for (int i = startIp + 1; i < code.Count; i++)
            {
                if (code[i].OpCode == OpCode.IfCoinAhead)
                    depth++;

                if (code[i].OpCode == OpCode.EndIf)
                {
                    if (depth == 0)
                        return i;
                    depth--;
                }
            }

            return code.Count - 1;
        }

        private static List<CodeInstruction> ExtractFunctionA(IReadOnlyList<CodeInstruction> code)
        {
            var list = new List<CodeInstruction>();
            bool inside = false;

            for (int i = 0; i < code.Count; i++)
            {
                if (code[i].OpCode == OpCode.FuncAMarker)
                {
                    inside = true;
                    continue;
                }

                if (code[i].OpCode == OpCode.EndFunc)
                    break;

                if (inside)
                    list.Add(code[i]);
            }

            return list;
        }

        private static int RunFunction(
            LevelDefinition level,
            IReadOnlyList<CodeInstruction> func,
            ref int x,
            ref int y,
            ref int dir,
            HashSet<int> coins,
            List<EnemyState> enemies,
            ref int bossHealth,
            ref SimulationResult result)
        {
            int steps = 0;

            for (int i = 0; i < func.Count; i++)
            {
                bool actionConsumed = false;

                switch (func[i].OpCode)
                {
                    case OpCode.MoveForward:
                        TryMove(level, ref x, ref y, dir);
                        actionConsumed = true;
                        break;

                    case OpCode.TurnLeft:
                        dir = (dir + 3) % 4;
                        actionConsumed = true;
                        break;

                    case OpCode.TurnRight:
                        dir = (dir + 1) % 4;
                        actionConsumed = true;
                        break;

                    case OpCode.AttackAhead:
                        int tx = x;
                        int ty = y;
                        StepInDirection(dir, ref tx, ref ty);

                        if (tx >= 0 && ty >= 0 && tx < level.Width && ty < level.Height)
                        {
                            if (level.IsBossLevel && tx == level.BossX && ty == level.BossY && bossHealth > 0)
                            {
                                bossHealth--;
                                result.BossDamageDealt++;
                                result.BossDefeated = bossHealth <= 0;
                            }

                            for (int e = 0; e < enemies.Count; e++)
                            {
                                if (enemies[e].Alive && enemies[e].X == tx && enemies[e].Y == ty)
                                {
                                    enemies[e].Alive = false;
                                    break;
                                }
                            }
                        }

                        actionConsumed = true;
                        break;
                }

                if (actionConsumed)
                {
                    steps++;
                    ApplyWorldTick(level, ref x, ref y, steps, coins, enemies, ref bossHealth, ref result);
                    if (result.HealthRemaining <= 0 || steps > 128)
                        break;
                }
            }

            return steps;
        }
    }
}
