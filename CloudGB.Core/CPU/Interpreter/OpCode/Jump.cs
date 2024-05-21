using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void JPImm16(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort addr);
            context.PC = addr;
        }

        public static void JRCondition(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte offset);
            sbyte signedOff = (sbyte)offset;
            switch (instruction.Opcode)
            {
                case 0x20:
                    signedOff = (sbyte)(!context.ZeroFlag ? signedOff : 0);
                    break;
                case 0x28:
                    signedOff = (sbyte)(context.ZeroFlag ? signedOff : 0);
                    break;
                case 0x30:
                    signedOff = (sbyte)(!context.CarryFlag ? signedOff : 0);
                    break;
                case 0x38:
                    signedOff = (sbyte)(context.CarryFlag ? signedOff : 0);
                    break;
            }
            context.PC = (ushort)(context.PC + signedOff + 2);
        }

        public static void JPCondition(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort target);
            
            switch (instruction.Opcode)
            {
                case 0xC2:
                    target = (ushort)(!context.ZeroFlag ? target : context.PC + 3);
                    break;
                case 0xCA:
                    target = (ushort)(context.ZeroFlag ? target : context.PC + 3);
                    break;
                case 0xD2:
                    target = (ushort)(!context.CarryFlag ? target : context.PC + 3);
                    break;
                case 0xDA:
                    target = (ushort)(context.CarryFlag ? target : context.PC + 3);
                    break;
            }
            context.PC = target;
        }

        public static void Call(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort target);
            PushStack16(context, memory, (ushort)(context.PC + 3));
            context.PC = target;
        }

        public static void CallCondition(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read16Bit((ushort)(context.PC + 1), out ushort target);
            bool condition = false;
            switch(instruction.Opcode)
            {
                case 0xC4:
                    condition = !context.ZeroFlag;
                    break;
                case 0xCC:
                    condition = context.ZeroFlag;
                    break;
                case 0xD4:
                    condition = !context.CarryFlag;
                    break;
                case 0xDC:
                    condition = context.CarryFlag;
                    break;
                default:
                    break;
            }
            if (condition)
            {
                PushStack16(context, memory, (ushort)(context.PC + 3));
            }
            context.PC = condition ? target : (ushort)(context.PC + 3);
        }

        public static void Ret(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            ushort retAddr = PopStack16(context, memory);
            context.PC = retAddr;
        }

        public static void RetCondition(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            bool condition = false;
            switch (instruction.Opcode)
            {
                case 0xC0:
                    condition = !context.ZeroFlag;
                    break;
                case 0xC8:
                    condition = context.ZeroFlag;
                    break;
                case 0xD0:
                    condition = !context.CarryFlag;
                    break;
                case 0xD8:
                    condition = context.CarryFlag;
                    break;
            }
            if (condition)
            {
                ushort retAddr = PopStack16(context, memory);
                context.PC = retAddr;
            }else
            {
                context.PC = (ushort)(context.PC + 1);
            }
        }

        public static void Reti(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            ushort retAddr = PopStack16(context, memory);
            context.PC = retAddr;
            // todo enable Interrupt
        }
    }
}
