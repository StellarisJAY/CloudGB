using CloudGB.Core.Memory;
using CloudGB.Core.CPU.Interpreter.OpCode;

namespace CloudGB.Core.CPU.Interpreter
{
    public class InterpreterProcessor : IProcessor
    {
        private CPUContext _context;
        private IMemoryMap _memory;
        private Instruction?[] _instructionSet = new Instruction?[256];
        private bool _debugMode;

        public delegate void InstructionCallback(Instruction instruction, CPUContext context, IMemoryMap memory);
        private InstructionCallback _instructionCallback;

        private List<ushort> _breakpoints = [];

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

        public bool Step(out int cycles, out int breakpoint)
        {
            cycles = 0;
            breakpoint = -1;
            if (_context.Halt)
                return false;
            bool hasNext = _memory.Read(_context.PC, out byte opcode);
            breakpoint = _breakpoints.IndexOf(_context.PC);
            if (!hasNext)
            {
                return false;
            }
            Instruction? instruction = _instructionSet[opcode];
            if (instruction == null)
            {
                Console.WriteLine($"unknown instr {opcode,0:X2}");
                Console.WriteLine($"cpu dump:\nPC={_context.PC,0:X4}  {DumpContext()}");
                return false;
            }
            _instructionCallback(instruction, _context, _memory);
            instruction.Handle(_context, instruction, _memory);
            // Normal commands add cycles here, special commands like 0xCB add cycles in execution function
            _context.LastCycles = instruction.Cycles;
            _context.TotalCycles += instruction.Cycles;
            cycles = _context.LastCycles;
            return true;
        }

        public void Reset()
        {
            // A: 01,B: 00,C: 13,D: 00,E: D8,F: B0,H: 01,L: 4D,SP: FFFE
            _context.PC = 0x100;
            _context.LastCycles = 0;
            _context.TotalCycles = 0;
            _context.SP = 0xFFFE;
            _context.A = 0x01;
            _context.B = 0x00;
            _context.C = 0x13;
            _context.D = 0x00;
            _context.E = 0xD8;
            _context.F = 0xB0;
            _context.H = 0x01;
            _context.L = 0x4D;
            _context.Halt = false;
        }

        public void Reset(CPUContext context)
        {
            _context = context;
        }

        public int SetBreakpoint(ushort address)
        {
            int idx = _breakpoints.FindIndex(addr => addr == address);
            if (idx != -1) return idx;
            _breakpoints.Add(address);
            return _breakpoints.Count - 1;
        }

        public void RemoveBreakpoint(int idx)
        {
            _breakpoints.RemoveAt(idx);
        }

        private void TraceExecution(Instruction instruction, CPUContext context, IMemoryMap memory)
        {
            Console.WriteLine(FormatInstruction(instruction, _context
                .PC));
        }

        private string DumpContext()
        {
            return $"A:{_context.A,0:X2},B:{_context.B,0:X2},C:{_context.C,0:X2},D:{_context.D,0:X2},E:{_context.E,0:X2},F:{_context.F,0:X2},H:{_context.H,0:X2},L:{_context.L,0:X2}  Cycles:{_context.TotalCycles}";
        }

        string IProcessor.Disassemble(ushort address)
        {
            _memory.Read(address, out byte opcode); 
            Instruction? instruction = _instructionSet[opcode];
            if (instruction == null)
            {
                return $"Unknown opcode {opcode}";
            }
            return FormatInstruction(instruction, address);
        }

        private string FormatInstruction(Instruction instruction, ushort pc)
        {
            _memory.Read((ushort)(pc + 0), out byte m1);
            _memory.Read((ushort)(pc + 1), out byte m2);
            _memory.Read((ushort)(pc + 2), out byte m3);
            _memory.Read((ushort)(pc + 3), out byte m4);
            return $"A:{_context.A,0:X2} F:{_context.F,0:X2} B:{_context.B,0:X2} C:{_context.C,0:X2} D:{_context.D,0:X2} E:{_context.E,0:X2} H:{_context.H,0:X2} L:{_context.L,0:X2} SP:{_context.SP,0:X4} PC:{_context.PC,0:X4} PCMEM:{m1,0:X2},{m2,0:X2},{m3,0:X2},{m4,0:X2}";
        }

        string IProcessor.DumpRegisters()
        {
            return DumpContext();
        }

        ushort IProcessor.PC()
        {
            return _context.PC;
        }
    }
}