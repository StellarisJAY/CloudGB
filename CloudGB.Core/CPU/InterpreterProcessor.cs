using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU
{
    public class InterpreterProcessor: IProcessor
    {
        private CPUContext _context;
        private IMemoryMap _memory;
        
        public InterpreterProcessor(IMemoryMap memory)
        {
            _context = new();
            _memory = memory;
        }

        public bool Step(out int cycles)
        {
            bool hasNext = _memory.Read(_context.PC, out byte opcode);
            cycles = 0;
            if (!hasNext)
            {
                return false;
            }
            switch (opcode)
            {
                case 0x0: //nop
                    cycles = 4;
                    _context.PC += 1;
                    break;
                case 0x18:
                    _memory.Read((ushort)(_context.PC + 1), out byte addr8);
                    int offset = (sbyte)addr8;
                    _context.PC = (ushort)(_context.PC + offset);
                    break;
                case 0xC3:
                    cycles = 12;
                    _memory.Read16Bit((ushort)(_context.PC + 1), out ushort addr);
                    _context.PC = addr;
                    break;
                default:
                    cycles = 0;
                    break;
            }
            return true;
        }
    }
}