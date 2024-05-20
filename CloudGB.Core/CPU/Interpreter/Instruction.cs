using CloudGB.Core.Memory;

namespace CloudGB.Core.CPU.Interpreter
{
    public class Instruction
    {
        public byte Opcode { get; set; }
        public string? Name { get; set; }
        public string? FullDescription { get; set; }
        public int Cycles { get; set; }

        public int Argc { get; set; }

        public Instruction?[]? SubInstructions { get; set; } = null;

        public delegate void Handler(CPUContext context, Instruction instruction, IMemoryMap memory);
        
        public Handler Handle { get; set; }
        public Instruction(byte opcode, string? name, string? fullDescription, int cycles, int argc, Handler handle)
        {
            Opcode = opcode;
            Name = name;
            FullDescription = fullDescription;
            Cycles = cycles;
            Argc = argc;
            Handle = handle;
        }
    }
}
