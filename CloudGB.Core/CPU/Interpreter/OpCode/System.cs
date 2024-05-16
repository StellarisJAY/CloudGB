using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void NOP(CPUContext ctx, Instruction ins, IMemoryMap memory)
        {
            // nop
            ctx.PC += 1;
        }

        public static void HALT(CPUContext ctx, Instruction ins, IMemoryMap memory)
        {
            ctx.PC += 1;
            ctx.Halt = true;
        }
    }
}
