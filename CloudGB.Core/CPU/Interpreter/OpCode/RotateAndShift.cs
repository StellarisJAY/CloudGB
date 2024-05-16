
using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {

        public static void RotatesAndShiftsCB(CPUContext context, Instruction instruction, IMemoryMap memory)
        {
            memory.Read((ushort)(context.PC + 1), out byte code);
            switch (code)
            {
                case <= 0x07:
                    RLC(context, instruction, memory, code);
                    break;
                case <= 0x0F:
                    break;
                case <= 0x17:
                    break;
                case <= 0x1F:
                    break;
                case <= 0x27:
                    break;
            }
        }

        public static void RLC(CPUContext context, Instruction instruction, IMemoryMap memory, byte code)
        {
            context.SubstractFlag = false;
            context.HalfCarryFlag = false;
            switch(code)
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
            context.PC += 1;
        }
    }
}
