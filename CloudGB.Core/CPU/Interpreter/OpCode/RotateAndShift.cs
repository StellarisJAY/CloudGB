
using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {

        private static byte RotateRightThrouhCarry(CPUContext context, byte input)
        {
            //byte old = input;
            byte output = input;
            bool rightMost = (output & 1) == 1;
            output >>= 1;
            output |= (byte)(context.CarryFlag ? (1 << 7) : 0);
            context.CarryFlag = rightMost;
            context.ZeroFlag = output == 0;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            return output;
        }

        private static byte RotateRight(CPUContext context, byte input)
        {
            //byte old = input;
            byte output = input;
            bool rightMost = (output & 1) == 1;
            output >>= 1;
            output |= (byte)(rightMost ? (1<<7):0);
            context.CarryFlag = rightMost;
            context.ZeroFlag = output == 0;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            return output;
        }

        private static byte RotateLeftThroughCarry(CPUContext context, byte input)
        {
            //byte old = input;
            byte output = input;
            bool leftMost = (output & (1 << 7)) != 0;
            output <<= 1;
            output |= (byte)(context.CarryFlag ? 1 : 0);
            context.CarryFlag = leftMost;
            context.ZeroFlag = output == 0;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            return output;
        }
        private static byte RotateLeft(CPUContext context, byte input)
        {
            //byte old = input;
            byte output = input;
            bool leftMost = (output & (1 << 7)) != 0;
            output <<= 1;
            output |= (byte)(leftMost ? 1 : 0);
            context.CarryFlag = leftMost;
            context.ZeroFlag = output == 0;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            return output;
        }

        private static byte ShiftLeft(CPUContext context, byte input, bool lsbZero)
        {
            byte leftMost = (byte)(input & (1<<7));
            byte rightMost = (byte)(input & 1);
            input <<= 1;
            input |= (byte)(lsbZero ? 0 : rightMost);
            context.CarryFlag = leftMost != 0;
            context.ZeroFlag = input == 0;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            return input;
        }

        private static byte ShiftRight(CPUContext context, byte input, bool msbZero)
        {
            byte leftMost = (byte)(input & (1 << 7));
            byte rightMost = (byte)(input & 1);
            input >>= 1;
            input |= (byte)(msbZero ? 0 : leftMost);
            context.CarryFlag = rightMost != 0;
            context.ZeroFlag = input == 0;
            context.HalfCarryFlag = false;
            context.SubstractFlag = false;
            return input;
        }

        public static void RLA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.A = RotateLeftThroughCarry(context, context.A);
            context.PC += 1;
            context.ZeroFlag = false;
        }

        public static void RLCA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.A = RotateLeft(context, context.A);
            context.PC += 1;
            context.ZeroFlag = false;
        }

        public static void RRA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.A = RotateRightThrouhCarry(context, context.A);
            context.PC += 1;
            context.ZeroFlag = false;
        }

        public static void RRCA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.A = RotateRight(context, context.A);
            context.PC += 1;
            context.ZeroFlag = false;
        }

        public static void RotatesAndShiftsCB(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte code);
            var subInstr = instruction.SubInstructions?[code];
            if (subInstr == null)
            { 
                throw new InvalidOperationException($"unsupported sub opcode: {code,0:X2}");
            }
            subInstr.Handle(context, subInstr, memory);
            context.PC += 2;
        }


        public static void RRC(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x0F:
                    context.A = RotateRight(context, context.A);
                    break;
                case 0x08:
                    context.B = RotateRight(context, context.B);
                    break;
                case 0x09:
                    context.C = RotateRight(context, context.C);
                    break;
                case 0x0A:
                    context.D = RotateRight(context, context.D);
                    break;
                case 0x0B:
                    context.E = RotateRight(context, context.E);
                    break;
                case 0x0C:
                    context.H = RotateRight(context, context.H);
                    break;
                case 0x0D:
                    context.L = RotateRight(context, context.L);
                    break;
                case 0x0E:
                    memory.Read(context.HL, out byte data);
                    data = RotateRight(context, data);
                    memory.Write(context.HL, data);
                    break;
            }
        }

        // Rotate Right through Carry
        // shift right, old Carry to left-most bit, old right-most bit to Carry
        public static void RR(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0x1F:
                    context.A = RotateRightThrouhCarry(context, context.A);
                    break;
                case 0x18:
                    context.B = RotateRightThrouhCarry(context, context.B);
                    break;
                case 0x19:
                    context.C = RotateRightThrouhCarry(context, context.C);
                    break;
                case 0x1A:
                    context.D = RotateRightThrouhCarry(context, context.D);
                    break;
                case 0x1B:
                    context.E = RotateRightThrouhCarry(context, context.E);
                    break;
                case 0x1C:
                    context.H = RotateRightThrouhCarry(context, context.H);
                    break;
                case 0x1D:
                    context.L = RotateRightThrouhCarry(context, context.L);
                    break;
                case 0x1E:
                    memory.Read(context.HL, out byte data);
                    data = RotateRightThrouhCarry(context, data);
                    memory.Write(context.HL, data);
                    break;
            }
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
        }

        // SRL n
        // shift right into carry, MSB to zero
        public static void SRL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x3F:
                    context.A = ShiftRight(context, context.A, true);
                    break;
                case 0x38:
                    context.B = ShiftRight(context, context.B, true);
                    break;
                case 0x39:
                    context.C = ShiftRight(context, context.C, true);
                    break;
                case 0x3A:
                    context.D = ShiftRight(context, context.D, true);
                    break;
                case 0x3B:
                    context.E = ShiftRight(context, context.E, true);
                    break;
                case 0x3C:
                    context.H = ShiftRight(context, context.H, true);
                    break;
                case 0x3D:
                    context.L = ShiftRight(context, context.L, true);
                    break;
                case 0x3E:
                    memory.Read(context.HL, out byte data);
                    data = ShiftRight(context, data, true);
                    memory.Write(context.HL, data);
                    break;
            }
        }

        // SRA
        // shift right into carry, MSB doesn't change
        public static void SRA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x2F:
                    context.A = ShiftRight(context, context.A, false);
                    break;
                case 0x28:
                    context.B = ShiftRight(context, context.B, false);
                    break;
                case 0x29:
                    context.C = ShiftRight(context, context.C, false);
                    break;
                case 0x2A:
                    context.D = ShiftRight(context, context.D, false);
                    break;
                case 0x2B:
                    context.E = ShiftRight(context, context.E, false);
                    break;
                case 0x2C:
                    context.H = ShiftRight(context, context.H, false);
                    break;
                case 0x2D:
                    context.L = ShiftRight(context, context.L, false);
                    break;
                case 0x2E:
                    memory.Read(context.HL, out byte data);
                    data = ShiftRight(context, data, false);
                    memory.Write(context.HL, data);
                    break;
            }
        }
        
        // SLA
        // shift left into carry, LSB set to 0
        public static void SLA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x27:
                    context.A = ShiftLeft(context, context.A, true);
                    break;
                case 0x20:
                    context.B = ShiftLeft(context, context.B, true);
                    break;
                case 0x21:
                    context.C = ShiftLeft(context, context.C, true);
                    break;
                case 0x22:
                    context.D = ShiftLeft(context, context.D, true);
                    break;
                case 0x23:
                    context.E = ShiftLeft(context, context.E, true);
                    break;
                case 0x24:
                    context.H = ShiftLeft(context, context.H, true);
                    break;
                case 0x25:
                    context.L = ShiftLeft(context, context.L, true);
                    break;
                case 0x26:
                    memory.Read(context.HL, out byte data);
                    data = ShiftLeft(context, data, true);
                    memory.Write(context.HL, data);
                    break;
            }
        }

        public static void RLC(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            switch(instruction.Opcode)
            {
                case 0x07:
                    context.A = RotateLeft(context, context.A);
                    break;
                case 0x00:
                    context.B = RotateLeft(context, context.B);
                    break;
                case 0x01:
                    context.C = RotateLeft(context, context.C);
                    break;
                case 0x02:
                    context.D = RotateLeft(context, context.D);
                    break;
                case 0x03:
                    context.E = RotateLeft(context, context.E);
                    break;
                case 0x04:
                    context.H = RotateLeft(context, context.H);
                    break;
                case 0x05:
                    context.L = RotateLeft(context, context.L);
                    break;
                case 0x06:
                    memory.Read(context.HL, out byte data);
                    data = RotateLeft(context, data);
                    memory.Write(context.HL, data);
                    break;
            }
        }

        public static void RL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            switch (instruction.Opcode)
            {
                case 0x17:
                    context.A = RotateLeftThroughCarry(context, context.A);
                    break;
                case 0x10:
                    context.B = RotateLeftThroughCarry(context, context.B);
                    break;
                case 0x11:
                    context.C = RotateLeftThroughCarry(context, context.C);
                    break;
                case 0x12:
                    context.D = RotateLeftThroughCarry(context, context.D);
                    break;
                case 0x13:
                    context.E = RotateLeftThroughCarry(context, context.E);
                    break;
                case 0x14:
                    context.H = RotateLeftThroughCarry(context, context.H);
                    break;
                case 0x15:
                    context.L = RotateLeftThroughCarry(context, context.L);
                    break;
                case 0x16:
                    memory.Read(context.HL, out byte data);
                    data = RotateLeftThroughCarry(context, data);
                    memory.Write(context.HL, data);
                    break;
            }
        }
    }
}
