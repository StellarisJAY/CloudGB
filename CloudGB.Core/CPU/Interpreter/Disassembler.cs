using CloudGB.Core.Memory;
using CloudGB.Core.CPU.Interpreter.OpCode;
using System;

namespace CloudGB.Core.CPU.Interpreter
{
    public class Disassembler : IProcessor
    {
        private ushort _pc = 0;
        private IMemoryMap _memory;
        private Instruction[] _instructionSet = new Instruction[256];

        public Disassembler(IMemoryMap memory) { 
            _memory = memory;
            OpCodes.RegisterAllOpCodes(_instructionSet);
        }
        public void Reset()
        {
            _pc = 0x0;
        }

        public bool Step(out int cycles)
        {
            cycles = 0;
            if (_pc == 0xFFFF) return false;
            _memory.Read(_pc, out byte opcode);
            var instr = _instructionSet[opcode];
            if (instr != null && opcode != 0)
            {
                string args = "     ";
                if (instr.Argc == 1)
                {
                    _memory.Read((ushort)(_pc + 1), out byte arg);
                    args = $"{arg,0:X2}   ";
                }
                else if (instr.Argc == 2)
                {
                    _memory.Read((ushort)(_pc + 1), out byte low);
                    _memory.Read((ushort)(_pc + 2), out byte high);
                    args = $"{low,0:X2} {high,0:X2}";
                }
                Console.WriteLine($"{_pc,0:X4}\t{instr.Opcode,0:X2}  {instr.FullDescription,-15}  {args}");
                _pc += (ushort)(instr.Argc + 1);
                return true;
            }else if(opcode != 0)
            {
                _pc += 1;
                return true;
            }else
            {
                _pc += 1;
                return true;
            }
        }
    }
}
