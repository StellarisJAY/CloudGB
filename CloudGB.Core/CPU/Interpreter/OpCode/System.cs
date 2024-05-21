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

        public static void RST(CPUContext context, Instruction ins, IMemoryMap memory)
        {
            PushStack16(context, memory, (ushort)(context.PC + 1));
            ushort target = 0x0000;
            switch(ins.Opcode)
            {
                case 0xC7:
                    target += 0x00;
                    break;
                case 0xCF:
                    target += 0x08;
                    break;
                case 0xD7:
                    target += 0x10;
                    break;
                case 0xDF:
                    target += 0x18;
                    break;
                case 0xE7:
                    target += 0x20;
                    break;
                case 0xEF:
                    target += 0x28;
                    break;
                case 0xF7:
                    target += 0x30;
                    break;
                case 0xFF:
                    target += 0x38;
                    break;
            }
            context.PC = target;
        }
    }
}
