
using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {

        public static void RRA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            bool rightMost = (context.A & 1) == 1;
            context.A >>= 1;
            context.A |= (byte)(context.CarryFlag ? (1 << 7) : 0);
            context.CarryFlag = rightMost;
            context.ZeroFlag = context.A == 0;
            context.PC += 1;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
        }

        public static void RRCA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.CarryFlag = (context.A & 1) == 1;
            context.A >>= 1;
            context.A |= (byte)(context.CarryFlag ? 1 << 7 : 0);
            context.ZeroFlag = context.A == 0;
            context.PC += 1;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
        }

        public static void RotatesAndShiftsCB(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte code);
            var subInstr = instruction.SubInstructions?[code];
            if (subInstr == null)
            {
                throw new InvalidOperationException($"unsupported sub opcode: {code}");
            }
            subInstr.Handle(context, subInstr, memory);
            context.PC += 2;
        }


        public static void RRC(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x0F:
                    context.CarryFlag = (context.A & 1) == 1;
                    context.A >>= 1;
                    context.A |= (byte)(context.CarryFlag ? 1<<7 : 0);
                    context.ZeroFlag = context.A == 0;
                    break;
                case 0x08:
                    context.CarryFlag = (context.B & 1) == 1;
                    context.B >>= 1;
                    context.B |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = context.B == 0;
                    break;
                case 0x09:
                    context.CarryFlag = (context.C & 1) == 1;
                    context.C >>= 1;
                    context.C |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = context.C == 0;
                    break;
                case 0x0A:
                    context.CarryFlag = (context.D & 1) == 1;
                    context.D >>= 1;
                    context.D |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = context.D == 0;
                    break;
                case 0x0B:
                    context.CarryFlag = (context.E & 1) == 1;
                    context.E >>= 1;
                    context.E |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = context.E == 0;
                    break;
                case 0x0C:
                    context.CarryFlag = (context.H & 1) == 1;
                    context.H >>= 1;
                    context.H |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = context.H == 0;
                    break;
                case 0x0D:
                    context.CarryFlag = (context.L & 1) == 1;
                    context.L >>= 1;
                    context.L |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = context.L == 0;
                    break;
                case 0x0E:
                    memory.Read(context.HL, out byte data);
                    context.CarryFlag = (data & 1) == 1;
                    data >>= 1;
                    data |= (byte)(context.CarryFlag ? 1 << 7 : 0);
                    context.ZeroFlag = data == 0;
                    memory.Write(context.HL, data);
                    break;
            }
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            context.PC += 1;
        }

        // Rotate Right through Carry
        // shift right, old Carry to left-most bit, old right-most bit to Carry
        public static void RR(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0x1F:
                    bool rightMost = (context.A & 1) == 1;
                    context.A >>= 1;
                    context.A |= (byte)(context.CarryFlag ?  (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.A == 0;
                    break;
                case 0x18:
                    rightMost = (context.B & 1) == 1;
                    context.B >>= 1;
                    context.B |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.B == 0;
                    break;
                case 0x19:
                    rightMost = (context.C & 1) == 1;
                    context.C >>= 1;
                    context.C |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.C == 0;
                    break;
                case 0x1A:
                    rightMost = (context.D & 1) == 1;
                    context.D >>= 1;
                    context.D |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.D == 0;
                    break;
                case 0x1B:
                    rightMost = (context.E & 1) == 1;
                    context.E >>= 1;
                    context.E |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.E == 0;
                    break;
                case 0x1C:
                    rightMost = (context.H & 1) == 1;
                    context.H >>= 1;
                    context.H |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.H == 0;
                    break;
                case 0x1D:
                    rightMost = (context.L & 1) == 1;
                    context.L >>= 1;
                    context.L |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = context.L == 0;
                    break;
                case 0x1E:
                    memory.Read(context.HL, out byte data);
                    rightMost = (data & 1) == 1;
                    data >>= 1;
                    data |= (byte)(context.CarryFlag ? (1 << 7) : 0);
                    context.CarryFlag = rightMost;
                    context.ZeroFlag = data == 0;
                    memory.Write(context.HL, data);
                    break;
            }
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            context.PC += 1;
        }

        // SRL n
        // shift right into carry, MSB to zero
        public static void SRL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x3F:
                    context.CarryFlag = (context.A & 1) == 1;
                    context.A >>= 1;
                    context.ZeroFlag = context.A == 0;
                    break;
                case 0x38:
                    context.CarryFlag = (context.B & 1) == 1;
                    context.B >>= 1;
                    context.ZeroFlag = context.B == 0;
                    break;
                case 0x39:
                    context.CarryFlag = (context.C & 1) == 1;
                    context.C >>= 1;
                    context.ZeroFlag = context.C== 0;
                    break;
                case 0x3A:
                    context.CarryFlag = (context.D & 1) == 1;
                    context.D >>= 1;
                    context.ZeroFlag = context.D == 0;
                    break;
                case 0x3B:
                    context.CarryFlag = (context.E & 1) == 1;
                    context.E >>= 1;
                    context.ZeroFlag = context.E == 0;
                    break;
                case 0x3C:
                    context.CarryFlag = (context.H & 1) == 1;
                    context.H >>= 1;
                    context.ZeroFlag = context.H == 0;
                    break;
                case 0x3D:
                    context.CarryFlag = (context.L & 1) == 1;
                    context.L >>= 1;
                    context.ZeroFlag = context.L == 0;
                    break;
                case 0x3E:
                    memory.Read(context.HL, out byte data);
                    context.CarryFlag = (data & 1) == 1;
                    data >>= 1;
                    context.ZeroFlag = data == 0;
                    memory.Write(context.HL, data);
                    break;
            }
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            context.PC += 1;
        }

        public static void RLC(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            switch(instruction.Opcode)
            {
                case 0x07:
                    context.CarryFlag = (context.A & (1 << 7)) != 0;
                    context.A <<= 1;
                    context.A |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x00:
                    context.CarryFlag = (context.B & (1 << 7)) != 0;
                    context.B <<= 1;
                    context.B |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x01:
                    context.CarryFlag = (context.C & (1 << 7)) != 0;
                    context.C <<= 1;
                    context.C |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x02:
                    context.CarryFlag = (context.D & (1 << 7)) != 0;
                    context.D <<= 1;
                    context.D |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x03:
                    context.CarryFlag = (context.E & (1 << 7)) != 0;
                    context.E <<= 1;
                    context.E |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x04:
                    context.CarryFlag = (context.H & (1 << 7)) != 0;
                    context.H <<= 1;
                    context.H |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x05:
                    context.CarryFlag = (context.L & (1 << 7)) != 0;
                    context.L <<= 1;
                    context.L |= (byte)(context.CarryFlag ? 1 : 0);
                    break;
                case 0x06:
                    memory.Read(context.HL, out byte data);
                    context.CarryFlag = (data & (1 << 7)) != 0;
                    data <<= 1;
                    data |= (byte)(context.CarryFlag ? 1 : 0);
                    memory.Write(context.HL, data);
                    break;
            }
        }
    }
}
