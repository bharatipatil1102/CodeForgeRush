using CodeForgeRush.Models;

namespace CodeForgeRush.UI
{
    public static class DragContext
    {
        public static PaletteBlockView Current;

        public static CodeInstruction ToInstruction(PaletteBlockView block)
        {
            return new CodeInstruction(block.OpCode, block.Argument);
        }
    }
}
