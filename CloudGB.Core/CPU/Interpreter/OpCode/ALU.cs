﻿using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void AdcA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand;
            switch(instruction.Opcode)
            {
                case 0x8F:
                    operand = context.A;
                    break;
                case 0x88:
                    operand = context.B;
                    break;
                case 0x89:
                    operand = context.C;
                    break;
                case 0x8A:
                    operand = context.D;
                    break;
                case 0x8B:
                    operand = context.E;
                    break;
                case 0x8C:
                    operand = context.H;
                    break;
                case 0x8D:
                    operand = context.L;
                    break;
                case 0x8E:
                    memory.Read(context.HL, out byte n);
                    operand = n;
                    break;
                case 0xCE:
                    memory.Read((ushort)(context.PC + 1), out n);
                    operand = n;
                    context.PC += 1;
                    break;
                default:
                    throw new InvalidOperationException($"invalid opcode {instruction.Opcode,0:X2}");
            }
            context.PC += 1;
            byte old = context.A;
            context.A += (byte)(operand + (byte)(context.CarryFlag ? 1 : 0));
            context.ZeroFlag = context.A == 0;
            context.HalfCarryFlag = IsHalfCarry3To4(old, operand) || IsHalfCarry3To4((byte)(old+operand), (byte)(context.CarryFlag ? 1 : 0));
            context.SubstractFlag = false;
            context.CarryFlag = IsCarry(old, operand) || IsCarry((byte)(old + operand), (byte)(context.CarryFlag ? 1 : 0));
        }

        public static void AddA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte operand = 0;
            switch (instruction.Opcode)
            {
                case 0x87:
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
            context.ZeroFlag = context.A == 0;
            context.HalfCarryFlag = IsHalfCarry3To4(old, operand);
            context.SubstractFlag = false;
            context.CarryFlag = IsCarry(old, operand);
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
                    throw new NotSupportedException($"unsupported opcode in inc: {instruction.Opcode,0:X2}");
            }
            context.HalfCarryFlag = (old&0xF)==0xF;
            context.ZeroFlag = res == 0;
            context.PC += 1;
            context.SubstractFlag = false;
        }

        public static void Dec(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte old;
            byte res;
            switch (instruction.Opcode)
            {
                case 0x3D:
                    old = context.A;
                    context.A--;
                    res = context.A;
                    break;
                case 0x05:
                    old = context.B;
                    context.B--;
                    res = context.B;
                    break;
                case 0x0D:
                    old = context.C;
                    context.C--;
                    res = context.C;
                    break;
                case 0x15:
                    old = context.D;
                    context.D--;
                    res = context.D;
                    break;
                case 0x1D:
                    old = context.E;
                    context.E--;
                    res = context.E;
                    break;
                case 0x25:
                    old = context.H;
                    context.H--;
                    res = context.H;
                    break;
                case 0x2D:
                    old = context.L;
                    context.L--;
                    res = context.L;
                    break;
                case 0x35:
                    memory.Read(context.HL, out old);
                    res = (byte)(old - 1);
                    memory.Write(context.HL, res);
                    break;
                default:
                    throw new NotSupportedException($"unsupported opcode in dec: {instruction.Opcode,0:X2}");
            }
            context.ZeroFlag = res == 0;
            context.SubstractFlag = true;
            context.PC += 1;
            context.HalfCarryFlag = (old & 0xF) == 0;
        }

        public static void AddHL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.SubstractFlag = false;
            ushort old = context.HL;
            switch(instruction.Opcode)
            {
                case 0x09:
                    context.HL += context.BC;
                    context.HalfCarryFlag = IsHalfCarry11To12(old, context.BC);
                    break;
                case 0x19:
                    context.HL += context.DE;
                    context.HalfCarryFlag = IsHalfCarry11To12(old, context.DE);
                    break;
                case 0x29:
                    context.HL += context.HL;
                    context.HalfCarryFlag = IsHalfCarry11To12(old, old);
                    break;
                case 0x39:
                    context.HL += context.SP;
                    context.HalfCarryFlag = IsHalfCarry11To12(old, context.SP);
                    break;
            }
            context.CarryFlag = old > context.HL;
            context.PC += 1;
        }

        public static void Inc16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0x03:
                    context.BC += 1;
                    break;
                case 0x13:
                    context.DE += 1;
                    break;
                case 0x23:
                    context.HL += 1;
                    break;
                case 0x33:
                    context.SP += 1;
                    break;
            }
            context.PC += 1;
        }

        public static void Dec16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x0B:
                    context.BC -= 1;
                    break;
                case 0x1B:
                    context.DE -= 1;
                    break;
                case 0x2B:
                    context.HL -= 1;
                    break;
                case 0x3B:
                    context.SP -= 1;
                    break;
            }
            context.PC += 1;
        }

        public static void SubA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte old = context.A;
            byte op;
            context.SubstractFlag = true;
            switch(instruction.Opcode)
            {
                case 0x97:
                    op = context.A;
                    break;
                case 0x90:
                    op = context.B;
                    break;
                case 0x91:
                    op = context.C;
                    break;
                case 0x92:
                    op = context.D;
                    break;
                case 0x93:
                    op = context.E;
                    break;
                case 0x94:
                    op = context.H;
                    break;
                case 0x95:
                    op = context.L;
                    break;
                case 0x96:
                    memory.Read(context.HL, out op);
                    break;
                case 0xD6:
                    memory.Read((ushort)(context.PC + 1), out op);
                    context.PC += 1;
                    break;
                default:
                    throw new InvalidOperationException($"unsupported opcode {instruction.Opcode,0:X2}");
            }
            context.HalfCarryFlag = IsHalfBorrow4To3(context.A, op);
            context.CarryFlag = IsBorrow(context.A, op);
            context.A -= op;
            context.ZeroFlag = context.A == 0;
            context.PC += 1;
        }

        public static void SBCA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte old = context.A;
            byte op;
            context.SubstractFlag = true;
            switch (instruction.Opcode)
            {
                case 0x9F:
                    op = context.A;
                    break;
                case 0x98:
                    op = context.B;
                    break;
                case 0x99:
                    op = context.C;
                    break;
                case 0x9A:
                    op = context.D;
                    break;
                case 0x9B:
                    op = context.E;
                    break;
                case 0x9C:
                    op = context.H;
                    break;
                case 0x9D:
                    op = context.L;
                    break;
                case 0x9E:
                    memory.Read(context.HL, out op);
                    break;
                case 0xDE:
                    memory.Read((ushort)(context.PC + 1), out op);
                    context.PC += 1;
                    break;
                default:
                    throw new InvalidOperationException($"unsupported opcode {instruction.Opcode,0:X2}");
            }
            byte carry = (byte)(context.CarryFlag ? 1 : 0);
            context.A -= op;
            context.A -= carry;
            context.ZeroFlag = context.A == 0;
            context.CarryFlag = op > old || carry > (old - op);
            context.HalfCarryFlag = (op & 0xF) > (old & 0xF) || (carry & 0xF) > ((old-op) & 0xF);
            context.PC += 1;
        }

        public static void AndA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0xA7:
                    context.A &= context.A;
                    break;
                case 0xA0:
                    context.A &= context.B;
                    break;
                case 0xA1:
                    context.A &= context.C;
                    break;
                case 0xA2:
                    context.A &= context.D;
                    break;
                case 0xA3:
                    context.A &= context.E;
                    break;
                case 0xA4:
                    context.A &= context.H;
                    break;
                case 0xA5:
                    context.A &= context.L;
                    break;
                case 0xA6:
                    memory.Read(context.HL, out byte op);
                    context.A &= op;
                    break;
                case 0xE6:
                    memory.Read((ushort)(context.PC + 1), out op);
                    context.A &= op;
                    context.PC += 1;
                    break;
            }
            context.PC += 1;
            context.ZeroFlag = context.A == 0;
            context.SubstractFlag = false;
            context.HalfCarryFlag = true;
            context.CarryFlag = false;
        }

        public static void OrA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0xB7:
                    context.A |= context.A;
                    break;
                case 0xB0:
                    context.A |= context.B;
                    break;
                case 0xB1:
                    context.A |= context.C;
                    break;
                case 0xB2:
                    context.A |= context.D;
                    break;
                case 0xB3:
                    context.A |= context.E;
                    break;
                case 0xB4:
                    context.A |= context.H;
                    break;
                case 0xB5:
                    context.A |= context.L;
                    break;
                case 0xB6:
                    memory.Read(context.HL, out byte op);
                    context.A |= op;
                    break;
                case 0xF6:
                    memory.Read((ushort)(context.PC + 1), out op);
                    context.A |= op;
                    context.PC += 1;
                    break;
            }
            context.PC += 1;
            context.ZeroFlag = context.A == 0;
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            context.CarryFlag = false;
        }

        public static void XorA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0xAF:
                    context.A ^= context.A;
                    break;
                case 0xA8:
                    context.A ^= context.B;
                    break;
                case 0xA9:
                    context.A ^= context.C;
                    break;
                case 0xAA:
                    context.A ^= context.D;
                    break;
                case 0xAB:
                    context.A ^= context.E;
                    break;
                case 0xAC:
                    context.A ^= context.H;
                    break;
                case 0xAD:
                    context.A ^= context.L;
                    break;
                case 0xAE:
                    memory.Read(context.HL, out byte op);
                    context.A ^= op;
                    break;
                case 0xEE:
                    memory.Read((ushort)(context.PC + 1), out op);
                    context.A ^= op;
                    context.PC += 1;
                    break;
            }
            context.PC += 1;
            context.ZeroFlag = context.A == 0;
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            context.CarryFlag = false;
        }

        public static void CP(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            byte other;
            switch (instruction.Opcode)
            {
                case 0xBF:
                    other = context.A;
                    break;
                case 0xB8:
                    other = context.B;
                    break;
                case 0xB9:
                    other = context.C;
                    break;
                case 0xBA:
                    other = context.D;
                    break;
                case 0xBB:
                    other = context.E;
                    break;
                case 0xBC:
                    other = context.H;
                    break;
                case 0xBD:
                    other = context.L;
                    break;
                case 0xBE:
                    memory.Read(context.HL, out other);
                    break;
                case 0xFE:
                    memory.Read((ushort)(context.PC + 1), out other);
                    context.PC += 1;
                    break;
                default:
                    throw new InvalidOperationException($"unsupported opcode {instruction.Opcode}");
            }
            context.ZeroFlag = context.A == other;
            context.SubstractFlag = true;
            context.CarryFlag = context.A < other;
            context.HalfCarryFlag = (other & 0xF) > (context.A & 0xF);
            context.PC += 1;
        }

        private static bool IsHalfCarry(byte old, byte res)
        {
            return (old & 0xF) > (res & 0xF);
        }

        private static bool IsCarry(byte op1, byte op2)
        {
            return (((op1 & 0xFF) + (op2 & 0xFF)) & 0x100) != 0;
        }

        private static bool IsHalfCarry3To4(byte op1, byte op2)
        {
            return (((op1 & 0xF) + (op2 & 0xF)) & 0x10) != 0;
        }
        
        private static bool IsHalfCarry11To12(ushort op1, ushort op2)
        {
            return (((op1 & 0xFFF) + (op2 & 0xFFF)) & 0x1000) != 0;
        }

        private static bool IsBorrow(byte op1, byte op2)
        {
            return op1 < op2;
        }

        private static bool IsHalfBorrow4To3(byte op1, byte op2)
        {
            return (op1 & 0xF) < (op2 & 0xF);
        }
    }
}
