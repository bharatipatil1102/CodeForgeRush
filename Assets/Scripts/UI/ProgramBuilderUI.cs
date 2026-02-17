using System.Collections.Generic;
using CodeForgeRush.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CodeForgeRush.UI
{
    public sealed class ProgramBuilderUI : MonoBehaviour
    {
        [SerializeField] private Transform tokensRoot;
        [SerializeField] private ProgramTokenView tokenPrefab;
        [SerializeField] private Text instructionCounter;

        private readonly List<CodeInstruction> _instructions = new List<CodeInstruction>();
        private int _maxInstructions = 12;

        public IReadOnlyList<CodeInstruction> Instructions => _instructions;

        public void SetMaxInstructions(int maxInstructions)
        {
            _maxInstructions = Mathf.Max(1, maxInstructions);
            RefreshCounter();
        }

        public bool TryAddInstruction(CodeInstruction instruction)
        {
            if (_instructions.Count >= _maxInstructions)
                return false;

            _instructions.Add(instruction);
            RebuildTokens();
            return true;
        }

        public void RemoveInstructionAt(int index)
        {
            if (index < 0 || index >= _instructions.Count)
                return;

            _instructions.RemoveAt(index);
            RebuildTokens();
        }

        public void ClearProgram()
        {
            _instructions.Clear();
            RebuildTokens();
        }

        private void RebuildTokens()
        {
            for (int i = tokensRoot.childCount - 1; i >= 0; i--)
                Destroy(tokensRoot.GetChild(i).gameObject);

            for (int i = 0; i < _instructions.Count; i++)
            {
                var token = Instantiate(tokenPrefab, tokensRoot);
                token.Bind(ToLabel(_instructions[i]), i, RemoveInstructionAt);
            }

            RefreshCounter();
        }

        private void RefreshCounter()
        {
            if (instructionCounter != null)
                instructionCounter.text = $"Instructions: {_instructions.Count}/{_maxInstructions}";
        }

        private static string ToLabel(CodeInstruction ins)
        {
            if (ins.OpCode == OpCode.Loop)
                return $"LOOP x{Mathf.Max(1, ins.Argument)}";
            return ins.OpCode.ToString().ToUpperInvariant();
        }
    }
}
