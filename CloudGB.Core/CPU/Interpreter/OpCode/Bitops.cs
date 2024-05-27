using CloudGB.Core.Memory;
using System;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void BIT(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand = (byte)(instruction.Opcode & 0b111);
            byte n = (byte)((instruction.Opcode >> 3) & 0b111);
            bool res;
            switch (operand)
            {
                case 7:
                    res = ((context.A >> n) & 1) == 1;
                    break;
                case 0:
                    res = ((context.B >> n) & 1) == 1;
                    break;
                case 1:
                    res = ((context.C >> n) & 1) == 1;
                    break;
                case 2:
                    res = ((context.D >> n) & 1) == 1;
                    break;
                case 3:
                    res = ((context.E >> n) & 1) == 1;
                    break;
                case 4:
                    res = ((context.H >> n) & 1) == 1;
                    break;
                case 5:
                    res = ((context.L >> n) & 1) == 1;
                    break;
                case 6:
                    memory.Read(context.HL, out byte data);
                    res = ((data >> n) & 1) == 1;
                    break;
                default:
                    throw new IndexOutOfRangeException("unreachable index");
            }
            context.ZeroFlag = !res;
            context.HalfCarryFlag = true;
            context.SubstractFlag = false;
            context.PC += 1;
        }


        private static void SetBit(CPUContext context, IMemoryMap memory, byte operand, byte n, bool state)
        {
            byte reset = (byte)~(1 << n);
            byte set = (byte)(1 << n);
            switch (operand)
            {
                case 7:
                    context.A = (byte)(state ? context.A | set : context.A & reset);
                    break;
                case 0:
                    context.B = (byte)(state ? context.B | set : context.B & reset);
                    break;
                case 1:
                    context.C = (byte)(state ? context.C | set : context.C & reset);
                    break;
                case 2:
                    context.D = (byte)(state ? context.D | set : context.D & reset);
                    break;
                case 3:
                    context.E = (byte)(state ? context.E | set : context.E & reset);
                    break;
                case 4:
                    context.H = (byte)(state ? context.H | set : context.H & reset);
                    break;
                case 5:
                    context.L = (byte)(state ? context.L | set : context.L & reset);
                    break;
                case 6:
                    memory.Read(context.HL, out byte data);
                    data = (byte)(state ? data | set : data & reset);
                    memory.Write(context.HL, data);
                    break;
                default:
                    throw new IndexOutOfRangeException("unreachable index");
            }
        }

        public static void BitopSet(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand = (byte)(instruction.Opcode & 0b111);
            byte n = (byte)((instruction.Opcode >> 3) & 0b111);
            SetBit(context, memory, operand, n, true);
            context.PC += 1;
        }

        public static void BitopReset(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand = (byte)(instruction.Opcode & 0b111);
            byte n = (byte)((instruction.Opcode >> 3) & 0b111);
            SetBit(context, memory, operand, n, false);
            context.PC += 1;
        }
    }
}
