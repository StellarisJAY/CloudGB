namespace CloudGB.Core.CPU.Interpreter.OpCode
{
    public partial class OpCodes
    {
        public static void RegisterAllOpCodes(Instruction?[] set)
        {
            // NOP
            set[0x0] = new(0x0, "NOP", "NOP", 4, 0, NOP);
            // HALT
            set[0x76] = new(0x76, "HALT", "HALT", 4, 0, HALT);
            // JP nn
            set[0xC3] = new(0xC3, "JP", "JP nn", 12, 2, JPImm16);
            // LD r, n
            set[0x06] = new(0x06, "LD", "LD B,n", 8, 1, LoadImm8);
            set[0x0E] = new(0x0E, "LD", "LD C,n", 8, 1, LoadImm8);
            set[0x16] = new(0x16, "LD", "LD D,n", 8, 1, LoadImm8);
            set[0x1E] = new(0x1E, "LD", "LD E,n", 8, 1, LoadImm8);
            set[0x26] = new(0x26, "LD", "LD H,n", 8, 1, LoadImm8);
            set[0x2E] = new(0x2E, "LD", "LD L,n", 8, 1, LoadImm8);
            // LD A, n
            set[0x7F] = new(0x7F, "LD", "LD A,A", 4, 0, LoadA);
            set[0x78] = new(0x78, "LD", "LD A,B", 4, 0, LoadA);
            set[0x79] = new(0x79, "LD", "LD A,C", 4, 0, LoadA);
            set[0x7A] = new(0x7A, "LD", "LD A,D", 4, 0, LoadA);
            set[0x7B] = new(0x7B, "LD", "LD A,E", 4, 0, LoadA);
            set[0x7C] = new(0x7C, "LD", "LD A,H", 4, 0, LoadA);
            set[0x7D] = new(0x7D, "LD", "LD A,L", 4, 0, LoadA);
            set[0x0A] = new(0x0A, "LD", "LD A,(BC)", 8, 0, LoadA);
            set[0x1A] = new(0x1A, "LD", "LD A,(DE)", 8, 0, LoadA);
            set[0x7E] = new(0x7E, "LD", "LD A,(HL)", 8, 0, LoadA);
            set[0xFA] = new(0xFA, "LD", "LD A,(nn)", 16, 0, LoadA);
        }
    }
}
