using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        private static void PushStack16(CPUContext context, IMemoryMap memory, ushort data)
        {
            context.SP -= 2;
            memory.Write16Bit(context.SP, data);
        }

        private static ushort PopStack16(CPUContext context, IMemoryMap memory)
        {
            memory.Read16Bit(context.SP, out ushort data);
            context.SP += 2;
            return data;
        }

        // LD (nn),SP
        public static void StoreSP(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort addr);
            memory.Write16Bit(addr, context.SP);
            context.PC += 3;
        }

        // LD (r16),SP
        public static void Push16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode) 
            {
                case 0xF5:
                    PushStack16(context, memory, context.AF);
                    break;
                case 0xC5:
                    PushStack16(context, memory, context.BC);
                    break;
                case 0xD5:
                    PushStack16(context, memory, context.DE);
                    break;
                case 0xE5:
                    PushStack16(context, memory, context.HL);
                    break;
            }
            context.PC += 1;
        }

        public static void Pop16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            ushort data = PopStack16(context, memory);
            switch(instruction.Opcode)
            {
                case 0xF1:
                    context.AF = data;
                    break;
                case 0xC1:
                    context.BC = data;
                    break;
                case 0xD1:
                    context.DE = data;
                    break;
                case 0xE1:
                    context.HL = data;
                    break;
            }
            context.PC += 1;
        }

        public static void LoadHLSp(CPUContext context, Instruction instr, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte data);
            sbyte signedOffset = (sbyte)data;
            context.ZeroFlag = false;
            context.SubstractFlag = false;
            if (signedOffset >= 0)
            {
                context.HalfCarryFlag = IsHalfCarry3To4((byte)(context.SP & 0xFF), data);
                context.CarryFlag = IsCarry((byte)(context.SP & 0xFF), data);
                context.HL = (ushort)(context.SP + data);
                context.PC += 2;
            }else
            {
                context.HalfCarryFlag = !IsHalfBorrow4To3((byte)(context.SP & 0xFF), (byte)(-signedOffset));
                context.CarryFlag = !IsBorrow((byte)(context.SP & 0xFF), (byte)(-signedOffset));
                context.HL = (ushort)(context.SP + signedOffset);
                context.PC += 2;
            }
        }

        public static void AddSP(CPUContext context, Instruction instr, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte data);
            context.ZeroFlag = false;
            context.SubstractFlag = false;
            sbyte signedOffset = (sbyte)data;
            if (signedOffset >= 0)
            {
                context.HalfCarryFlag = IsHalfCarry3To4((byte)(context.SP & 0xFF), data);
                context.CarryFlag = IsCarry((byte)(context.SP & 0xFF), data);
                context.SP += data;
                context.PC += 2;
            }else
            {
                context.HalfCarryFlag = !IsHalfBorrow4To3((byte)(context.SP & 0xFF), (byte)(-signedOffset));
                context.CarryFlag = !IsBorrow((byte)(context.SP & 0xFF), (byte)(-signedOffset));
                context.SP = (ushort)(context.SP + signedOffset);
                context.PC += 2;
            }
        }
    }
}
