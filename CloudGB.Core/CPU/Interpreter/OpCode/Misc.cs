using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        
        public static void Swap(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte res, old;
            switch(instruction.Opcode)
            {
                case 0x37:
                    old = context.A;
                    context.A = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.A;
                    break;
                case 0x30:
                    old = context.B;
                    context.B = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.B;
                    break;
                case 0x31:
                    old = context.C;
                    context.C = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.C;
                    break;
                case 0x32:
                    old = context.D;
                    context.D = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.D;
                    break;
                case 0x33:
                    old = context.E;
                    context.E = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.E;
                    break;
                case 0x34:
                    old = context.H;
                    context.H = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.H;
                    break;
                case 0x35:
                    old = context.L;
                    context.L = (byte)((old >> 4) | ((old & 0xF) << 4));
                    res = context.L;
                    break;
                case 0x36:
                    memory.Read(context.HL, out old);
                    res = (byte)((old >> 4) | ((old & 0xF) << 4));
                    memory.Write(context.HL, res);
                    break;
                default:
                    throw new InvalidOperationException($"unsupported code:{instruction.Opcode,0:X2}");
            }
            context.ZeroFlag = res == 0;
            context.HalfCarryFlag = false;
            context.CarryFlag = false;
            context.SubstractFlag = false;
            context.LastCycles = instruction.Cycles;
            context.TotalCycles += context.LastCycles;
        }

        public static void CPL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.A = (byte)~context.A;
            context.SubstractFlag = true;
            context.HalfCarryFlag = true;
            context.PC += 1;
        }

        public static void CCF(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.CarryFlag = !context.CarryFlag;
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
        }
    }
}
