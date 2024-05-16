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

        public static void JRCondition(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte offset);
            sbyte signedOff = (sbyte)offset;
            switch (instruction.Opcode)
            {
                case 0x20:
                    signedOff = (sbyte)(!context.ZeroFlag ? signedOff : 2);
                    break;
                case 0x28:
                    signedOff = (sbyte)(context.ZeroFlag ? signedOff : 2);
                    break;
                case 0x30:
                    signedOff = (sbyte)(!context.CarryFlag ? signedOff : 2);
                    break;
                case 0x38:
                    signedOff = (sbyte)(context.CarryFlag ? signedOff : 2);
                    break;
            }
            context.PC = (ushort)(context.PC + signedOff);
        }
    }
}
