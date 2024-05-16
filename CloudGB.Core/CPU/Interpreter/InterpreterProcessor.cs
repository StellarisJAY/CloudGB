using CloudGB.Core.Memory;
using CloudGB.Core.CPU.Interpreter.OpCode;

namespace CloudGB.Core.CPU.Interpreter
{
    public class InterpreterProcessor : IProcessor
    {
        private CPUContext _context;
        private IMemoryMap _memory;
        private Instruction?[] _instructionSet = new Instruction?[256];
        private ulong _cycles = 0;
        private bool _debugMode;

        public delegate void InstructionCallback(Instruction instruction, CPUContext context, IMemoryMap memory);
        private InstructionCallback _instructionCallback;

        public InterpreterProcessor(IMemoryMap memory, bool debugMode)
        {
            _context = new();
            _memory = memory;
            OpCodes.RegisterAllOpCodes(_instructionSet);
            _debugMode = debugMode;
            if (_debugMode )
            {
                _instructionCallback = TraceExecution;
            }else
            {
                _instructionCallback = (ins, ctx, mem) => { };
            }
        }

        public bool Step(out int cycles)
        {
            cycles = 0;
            if (_context.Halt)
                return false;
            bool hasNext = _memory.Read(_context.PC, out byte opcode);
            if (!hasNext)
            {
                return false;
            }
            Instruction? instruction = _instructionSet[opcode];
            if (instruction == null)
            {
                return false;
            }
            _instructionCallback(instruction, _context, _memory);
            instruction.Handle(_context, instruction, _memory);
            cycles = instruction.Cycles;
            _cycles += (ulong)(cycles);
            return true;
        }

        public void Reset()
        {
            _context.PC = 0;
            _cycles = 0;
        }

        private void TraceExecution(Instruction instruction, CPUContext context, IMemoryMap memory)
        {
            string args = "     ";
            if (instruction.Argc == 1)
            {
                memory.Read((ushort)(context.PC + 1), out byte arg);
                args = $"{arg,0:X2}   ";
            }
            else if (instruction.Argc == 2)
            {
                memory.Read((ushort)(context.PC + 1), out byte low);
                memory.Read((ushort)(context.PC + 2), out byte high);
                args = $"{low,0:X2} {high,0:X2}";
            }
            Console.WriteLine($"{context.PC,0:X4}\t{instruction.Opcode,0:X2}  {instruction.FullDescription,-15} {args}\t{DumpContext()}");
        }

        private string DumpContext()
        {
            return $"A:{_context.A,0:X2},B:{_context.B,0:X2},C:{_context.C,0:X2},D:{_context.D,0:X2},E:{_context.E,0:X2},F:{_context.F,0:X2},H:{_context.H,0:X2},L:{_context.L,0:X2}  Cycles:{_cycles}";
        }
    }
}