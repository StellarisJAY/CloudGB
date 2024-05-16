using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void JPImm16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort addr);
            context.PC = addr;
        }
    }
}
