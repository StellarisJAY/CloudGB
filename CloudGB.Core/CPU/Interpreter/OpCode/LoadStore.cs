using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void LoadImm8(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte data);
            switch (instruction.Opcode)
            {
                case 0x06:
                    context.B = data;
                    break;
                case 0x0E:
                    context.C = data;
                    break;
                case 0x16:
                    context.D = data;
                    break;
                case 0x1E:
                    context.E = data;
                    break;
                case 0x26:
                    context.H = data;
                    break;
                case 0x2E:
                    context.L = data;
                    break;
            }
            context.PC += 2;
        }

        public static void LoadA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0x7F:
                    context.PC += 1;
                    break;
                case 0x78:
                    context.A = context.B;
                    context.PC += 1;
                    break;
                case 0x79:
                    context.A = context.C;
                    context.PC += 1;
                    break;
                case 0x7A:
                    context.A = context.D;
                    context.PC += 1;
                    break;
                case 0x7B:
                    context.A = context.E;
                    context.PC += 1;
                    break;
                case 0x7C:
                    context.A = context.H;
                    context.PC += 1;
                    break;
                case 0x7D:
                    context.A = context.L;
                    context.PC += 1;
                    break;
                case 0x0A:
                    memory.Read(context.BC, out byte data);
                    context.A = data;
                    context.PC += 1;
                    break;
                case 0x1A:
                    memory.Read(context.DE, out data);
                    context.A = data;
                    context.PC += 1;
                    break;
                case 0x7E:
                    memory.Read(context.HL, out data);
                    context.A = data;
                    context.PC += 1;
                    break;
                case 0xFA:
                    memory.Read16Bit((ushort)(context.PC + 1), out ushort addr);
                    memory.Read(addr, out data);
                    context.A = data;
                    context.PC += 3;
                    break;
                case 0x3E:
                    break;
            }
        }
    }
}
