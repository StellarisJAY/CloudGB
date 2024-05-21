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
                    memory.Read((ushort)(context.PC + 1), out data);
                    context.A = data;
                    context.PC += 2;
                    break;
            }
        }

        public static void StoreA(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0x7F:
                    context.PC += 1;
                    break;
                case 0x47:
                    context.B = context.A;
                    context.PC += 1;
                    break;
                case 0x4F:
                    context.C = context.A;
                    context.PC += 1;
                    break;
                case 0x57:
                    context.D = context.A;
                    context.PC += 1;
                    break;
                case 0x5F:
                    context.E = context.A;
                    context.PC += 1;
                    break;
                case 0x67:
                    context.H = context.A;
                    context.PC += 1;
                    break;
                case 0x6F:
                    context.L = context.A;
                    context.PC += 1;
                    break;
                case 0x02:
                    memory.Write(context.BC, context.A);
                    context.PC += 1;
                    break;
                case 0x12:
                    memory.Write(context.DE, context.A);
                    context.PC += 1;
                    break;
                case 0x77:
                    memory.Write(context.HL, context.A);
                    context.PC += 1;
                    break;
                case 0xEA:
                    memory.Read16Bit((ushort)(context.PC + 1), out ushort addr);
                    memory.Write(addr, context.A);
                    context.PC += 3;
                    break;
            }
        }

        public static void LoadAC(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0xF2:
                    ushort addr = (ushort)(context.C + 0xFF00);
                    memory.Read(addr, out byte data);
                    context.A = data;
                    break;
                case 0xE2:
                    addr = (ushort)(context.C + 0xFF00);
                    memory.Write(addr, context.A);
                    break;
            }
        }

        public static void LoadB(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x40:
                    break;
                case 0x41:
                    context.B = context.C;
                    break;
                case 0x42:
                    context.B = context.D;
                    break;
                case 0x43:
                    context.B = context.E;
                    break;
                case 0x44:
                    context.B = context.H;
                    break;
                case 0x45:
                    context.B = context.L;
                    break;
                case 0x46:
                    memory.Read(context.HL, out byte data);
                    context.B = data;
                    break;
            }
            context.PC += 1;
        }

        public static void LoadC(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x48:
                    context.C = context.B;
                    break;
                case 0x49:
                    break;
                case 0x4A:
                    context.C = context.D;
                    break;
                case 0x4B:
                    context.C = context.E;
                    break;
                case 0x4C:
                    context.C = context.H;
                    break;
                case 0x4D:
                    context.C = context.L;
                    break;
                case 0x4E:
                    memory.Read(context.HL, out byte data);
                    context.C = data;
                    break;
            }
            context.PC += 1;
        }

        public static void LoadD(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x50:
                    context.D = context.B;
                    break;
                case 0x51:
                    context.D = context.C;
                    break;
                case 0x52:
                    break;
                case 0x53:
                    context.D = context.E;
                    break;
                case 0x54:
                    context.D = context.H;
                    break;
                case 0x55:
                    context.D = context.L;
                    break;
                case 0x56:
                    memory.Read(context.HL, out byte data);
                    context.D = data;
                    break;
            }
            context.PC += 1;
        }

        public static void LoadE(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x58:
                    context.E = context.B;
                    break;
                case 0x59:
                    context.E = context.C;
                    break;
                case 0x5A:
                    context.E = context.D;
                    break;
                case 0x5B:
                    break;
                case 0x5C:
                    context.E = context.H;
                    break;
                case 0x5D:
                    context.E = context.L;
                    break;
                case 0x5E:
                    memory.Read(context.HL, out byte data);
                    context.E = data;
                    break;
            }
            context.PC += 1;
        }

        public static void LoadH(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x60:
                    context.H = context.B;
                    break;
                case 0x61:
                    context.H = context.C;
                    break;
                case 0x62:
                    context.H = context.D;
                    break;
                case 0x63:
                    context.H = context.E;
                    break;
                case 0x64:
                    break;
                case 0x65:
                    context.H = context.L;
                    break;
                case 0x66:
                    memory.Read(context.HL, out byte data);
                    context.H = data;
                    break;
            }
            context.PC += 1;
        }

        public static void LoadL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch (instruction.Opcode)
            {
                case 0x68:
                    context.L = context.B;
                    break;
                case 0x69:
                    context.L = context.C;
                    break;
                case 0x6A:
                    context.L = context.D;
                    break;
                case 0x6B:
                    context.L = context.E;
                    break;
                case 0x6C:
                    context.L = context.H;
                    break;
                case 0x6D:
                    break;
                case 0x6E:
                    memory.Read(context.HL, out byte data);
                    context.L = data;
                    break;
            }
            context.PC += 1;
        }

        // LD (HL), r
        public static void StoreAtHL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            switch(instruction.Opcode)
            {
                case 0x70:
                    memory.Write(context.HL, context.B);
                    break;
                case 0x71:
                    memory.Write(context.HL, context.C);
                    break;
                case 0x72:
                    memory.Write(context.HL, context.D);
                    break;
                case 0x73:
                    memory.Write(context.HL, context.E);
                    break;
                case 0x74:
                    memory.Write(context.HL, context.H);
                    break;
                case 0x75:
                    memory.Write(context.HL, context.L);
                    break;
            }
            context.PC += 1;
        }

        // LD (HL),n
        public static void StoreImmAtHL(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte data);
            memory.Write(context.HL, data);
            context.PC += 2;
        }

        // LD r16,nn
        public static void Load16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort data);
            switch(instruction.Opcode)
            {
                case 0x01:
                    context.BC = data;
                    break;
                case 0x11:
                    context.DE = data;
                    break;
                case 0x21:
                    context.HL = data;
                    break;
                case 0x31:
                    context.SP = data;
                    break;
            }
            context.PC += 3;
        }

        // LDH n,A
        public static void StoreAN(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte offset);
            ushort addr = (ushort)(0xFF00 + offset);
            memory.Write(addr, context.A);
            context.PC += 2;
        }

        // LDH A,n
        public static void LoadAN(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte offset);
            ushort addr = (ushort)(0xFF00 + offset);
            memory.Read(addr, out byte data);
            context.A = data;
            context.PC += 2;
        }
    }
}
