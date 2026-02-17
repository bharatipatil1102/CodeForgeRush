namespace CodeForgeRush.Models
{
    public enum OpCode
    {
        MoveForward,
        TurnLeft,
        TurnRight,
        AttackAhead,
        Loop,
        EndLoop,
        IfCoinAhead,
        EndIf,
        CallFuncA,
        FuncAMarker,
        EndFunc
    }

    public readonly struct CodeInstruction
    {
        public OpCode OpCode { get; }
        public int Argument { get; }

        public CodeInstruction(OpCode opCode, int argument = 0)
        {
            OpCode = opCode;
            Argument = argument;
        }
    }
}
