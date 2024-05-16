using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
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
                    memory.Write16Bit((ushort)(context.SP - 1), context.AF);
                    break;
                case 0xC5:
                    memory.Write16Bit((ushort)(context.SP - 1), context.BC);
                    break;
                case 0xD5:
                    memory.Write16Bit((ushort)(context.SP - 1), context.DE);
                    break;
                case 0xE5:
                    memory.Write16Bit((ushort)(context.SP - 1), context.HL);
                    break;
            }
            context.PC += 1;
            context.SP -= 2;
        }

        public static void Pop16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.SP + 1), out ushort data);
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
            context.SP += 2;
        }
    }
}
