using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void AdcA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand = (byte)(context.CarryFlag ? 1 : 0);
            switch(instruction.Opcode)
            {
                case 0x87:
                    operand += context.A;
                    break;
                case 0x88:
                    operand += context.B;
                    break;
                case 0x89:
                    operand += context.C;
                    break;
                case 0x8A:
                    operand += context.D;
                    break;
                case 0x8B:
                    operand += context.E;
                    break;
                case 0x8C:
                    operand += context.H;
                    break;
                case 0x8D:
                    operand += context.L;
                    break;
                case 0x8E:
                    memory.Read(context.HL, out byte n);
                    operand += n;
                    break;
                case 0xCE:
                    memory.Read((ushort)(context.PC + 1), out n);
                    operand += n;
                    context.PC += 1;
                    break;
            }
            context.PC += 1;
            byte old = context.A;
            context.A += operand;
            if (context.A == 0)
            {
                context.ZeroFlag = true;
            }
            context.SubstractFlag = false;
            context.CarryFlag = IsCarry(old, context.A);
            context.HalfCarryFlag = IsHalfCarry(old, context.A);
        }

        public static void AddA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand = 0;
            switch (instruction.Opcode)
            {
                case 0x8F:
                    operand = context.A;
                    break;
                case 0x80:
                    operand = context.B;
                    break;
                case 0x81:
                    operand = context.C;
                    break;
                case 0x82:
                    operand = context.D;
                    break;
                case 0x83:
                    operand = context.E;
                    break;
                case 0x84:
                    operand = context.H;
                    break;
                case 0x85:
                    operand = context.L;
                    break;
                case 0x86:
                    memory.Read(context.HL, out operand);
                    break;
                case 0xC6:
                    memory.Read((ushort)(context.PC + 1), out operand);
                    context.PC += 1;
                    break;
            }
            context.PC += 1;
            byte old = context.A;
            context.A += operand;
            if (context.A == 0)
            {
                context.ZeroFlag = true;
            }
            context.SubstractFlag = false;
            context.CarryFlag = IsCarry(old, context.A);
            context.HalfCarryFlag = IsHalfCarry(old, context.A);
        }

        public static void Inc(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte res;
            byte old;
            switch (instruction.Opcode)
            {
                case 0x3C:
                    old = context.A;
                    context.A++;
                    res = context.A;
                    break;
                case 0x04:
                    old = context.B;
                    context.B++;
                    res = context.B;
                    break;
                case 0x0C:
                    old = context.C;
                    context.C++;
                    res = context.C;
                    break;
                case 0x14:
                    old = context.D;
                    context.D++;
                    res = context.D;
                    break;
                case 0x1C:
                    old = context.E;
                    context.E++;
                    res = context.E;
                    break;
                case 0x24:
                    old = context.H;
                    context.H++;
                    res = context.H;
                    break;
                case 0x2C:
                    old = context.L;
                    context.L++;
                    res = context.L;
                    break;
                case 0x34:
                    memory.Read(context.HL, out old);
                    res = (byte)(old + 1);
                    memory.Write(context.HL, res);
                    break;
                default:
                    throw new ArgumentException($"unknown opcode:{instruction.Opcode}");
            }
            context.HalfCarryFlag = IsHalfCarry(old, res);
            context.ZeroFlag = res == 0;
            context.PC += 1;
        }



        private static bool IsHalfCarry(byte old, byte res)
        {
            return (old & 0xF) > (res & 0xF);
        }

        private static bool IsCarry(byte old, byte res)
        {
            return old > res;
        }
    }
}
