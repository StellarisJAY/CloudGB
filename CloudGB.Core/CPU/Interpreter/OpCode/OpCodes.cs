using CloudGB.Core.Memory;

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
            set[0xFA] = new(0xFA, "LD", "LD A,(nn)", 16, 2, LoadA);

            // LD n, A
            set[0x47] = new(0x47, "LD", "LD B,A", 4, 0, StoreA);
            set[0x4F] = new(0x4F, "LD", "LD C,A", 4, 0, StoreA);
            set[0x57] = new(0x57, "LD", "LD D,A", 4, 0, StoreA);
            set[0x5F] = new(0x5F, "LD", "LD E,A", 4, 0, StoreA);
            set[0x67] = new(0x67, "LD", "LD H,A", 4, 0, StoreA);
            set[0x6F] = new(0x6F, "LD", "LD L,A", 4, 0, StoreA);
            set[0x02] = new(0x02, "LD", "LD (BC),A", 8, 0, StoreA);
            set[0x12] = new(0x12, "LD", "LD (DE),A", 8, 0, StoreA);
            set[0x77] = new(0x77, "LD", "LD (HL),A", 8, 0, StoreA);
            set[0xEA] = new(0xEA, "LD", "LD (nn),A", 16, 2, StoreA);

            // LD A,(C); LD (C),A
            set[0xF2] = new(0xF2, "LD", "LD A,(C)", 8, 0, LoadAC);
            set[0xE2] = new(0xE2, "LD", "LD (C), A", 8, 0, LoadAC);

            // LD B, n
            set[0x40] = new(0x40, "LD", "LD B,B", 4, 0, LoadB);
            set[0x41] = new(0x41, "LD", "LD B,C", 4, 0, LoadB);
            set[0x42] = new(0x42, "LD", "LD B,D", 4, 0, LoadB);
            set[0x43] = new(0x43, "LD", "LD B,E", 4, 0, LoadB);
            set[0x44] = new(0x44, "LD", "LD B,H", 4, 0, LoadB);
            set[0x45] = new(0x45, "LD", "LD B,L", 4, 0, LoadB);
            set[0x46] = new(0x46, "LD", "LD B,(HL)", 8, 0, LoadB);

            // LD C,n
            set[0x48] = new(0x48, "LD", "LD C,B", 4, 0, LoadC);
            set[0x49] = new(0x49, "LD", "LD C,C", 4, 0, LoadC);
            set[0x4A] = new(0x4A, "LD", "LD C,D", 4, 0, LoadC);
            set[0x4B] = new(0x4B, "LD", "LD C,E", 4, 0, LoadC);
            set[0x4C] = new(0x4C, "LD", "LD C,H", 4, 0, LoadC);
            set[0x4D] = new(0x4D, "LD", "LD C,L", 4, 0, LoadC);
            set[0x4E] = new(0x4E, "LD", "LD C,(HL)", 8, 0, LoadC);

            // LD D,n
            set[0x50] = new(0x50, "LD", "LD D,B", 4, 0, LoadD);
            set[0x51] = new(0x51, "LD", "LD D,C", 4, 0, LoadD);
            set[0x52] = new(0x52, "LD", "LD D,D", 4, 0, LoadD);
            set[0x53] = new(0x53, "LD", "LD D,E", 4, 0, LoadD);
            set[0x54] = new(0x54, "LD", "LD D,H", 4, 0, LoadD);
            set[0x55] = new(0x55, "LD", "LD D,L", 4, 0, LoadD);
            set[0x56] = new(0x56, "LD", "LD D,(HL)", 8, 0, LoadD);

            // LD E,n
            set[0x58] = new(0x58, "LD", "LD E,B", 4, 0, LoadE);
            set[0x59] = new(0x59, "LD", "LD E,C", 4, 0, LoadE);
            set[0x5A] = new(0x5A, "LD", "LD E,D", 4, 0, LoadE);
            set[0x5B] = new(0x5B, "LD", "LD E,E", 4, 0, LoadE);
            set[0x5C] = new(0x5C, "LD", "LD E,H", 4, 0, LoadE);
            set[0x5D] = new(0x5D, "LD", "LD E,L", 4, 0, LoadE);
            set[0x5E] = new(0x5E, "LD", "LD E,(HL)", 8, 0, LoadE);

            // LD H,n
            set[0x60] = new(0x60, "LD", "LD H,B", 4, 0, LoadH);
            set[0x61] = new(0x61, "LD", "LD H,C", 4, 0, LoadH);
            set[0x62] = new(0x62, "LD", "LD H,D", 4, 0, LoadH);
            set[0x63] = new(0x63, "LD", "LD H,E", 4, 0, LoadH);
            set[0x64] = new(0x64, "LD", "LD H,H", 4, 0, LoadH);
            set[0x65] = new(0x65, "LD", "LD H,L", 4, 0, LoadH);
            set[0x66] = new(0x66, "LD", "LD H,(HL)", 8, 0, LoadH);

            // LD L,n
            set[0x68] = new(0x68, "LD", "LD L,B", 4, 0, LoadL);
            set[0x69] = new(0x69, "LD", "LD L,C", 4, 0, LoadL);
            set[0x6A] = new(0x6A, "LD", "LD L,D", 4, 0, LoadL);
            set[0x6B] = new(0x6B, "LD", "LD L,E", 4, 0, LoadL);
            set[0x6C] = new(0x6C, "LD", "LD L,H", 4, 0, LoadL);
            set[0x6D] = new(0x6D, "LD", "LD L,L", 4, 0, LoadL);
            set[0x6E] = new(0x6E, "LD", "LD L,(HL)", 8, 0, LoadL);

            // LD (HL),r
            set[0x70] = new(0x70, "LD", "LD (HL),B", 8, 0, StoreAtHL);
            set[0x71] = new(0x71, "LD", "LD (HL),C", 8, 0, StoreAtHL);
            set[0x72] = new(0x72, "LD", "LD (HL),D", 8, 0, StoreAtHL);
            set[0x73] = new(0x73, "LD", "LD (HL),E", 8, 0, StoreAtHL);
            set[0x74] = new(0x74, "LD", "LD (HL),H", 8, 0, StoreAtHL);
            set[0x75] = new(0x75, "LD", "LD (HL),L", 8, 0, StoreAtHL);

            // LD (HL),n
            set[0x36] = new(0x36, "LD", "LD (HL),n", 12, 1, StoreImmAtHL);

            // LD r16,nn
            set[0x01] = new(0x01, "LD", "LD BC,nn", 12, 2, Load16);
            set[0x11] = new(0x11, "LD", "LD DE,nn", 12, 2, Load16);
            set[0x21] = new(0x21, "LD", "LD HL,nn", 12, 2, Load16);
            set[0x31] = new(0x31, "LD", "LD SP,nn", 12, 2, Load16);


            // LDI A,(HL+)
            set[0x2A] = new(0x2A, "LDI", "LDI A,(HL+)", 8, 0, (ctx, instr, mem) =>
            {
                mem.Read(ctx.HL, out byte data);
                ctx.A = data;
                ctx.HL += 1;
                ctx.PC += 1;
            });

            // Stack
            // LD SP,HL
            set[0xF9] = new(0xF9, "LD", "LD SP,HL", 8, 0, (ctx, ins, mem) => { ctx.SP = ctx.HL; ctx.PC += 1; });

            // LD (nn),SP
            set[0x08] = new(0x08, "LD", "LD (nn),SP", 20, 0, StoreSP);

            // PUSH r16
            set[0xF5] = new(0xF5, "PUSH", "PUSH AF", 16, 0, Push16);
            set[0xC5] = new(0xC5, "PUSH", "PUSH BC", 16, 0, Push16);
            set[0xD5] = new(0xD5, "PUSH", "PUSH DE", 16, 0, Push16);
            set[0xE5] = new(0xE5, "PUSH", "PUSH HL", 16, 0, Push16);

            // POP r16
            set[0xF1] = new(0xF1, "POP", "POP AF", 12, 0, Pop16);
            set[0xC1] = new(0xC1, "POP", "POP BC", 12, 0, Pop16);
            set[0xD1] = new(0xD1, "POP", "POP DE", 12, 0, Pop16);
            set[0xE1] = new(0xE1, "POP", "POP HL", 12, 0, Pop16);

            // ALU
            // ADD A,n
            set[0x80] = new(0x80, "ADD", "ADD A,B", 4, 0, AddA);
            set[0x81] = new(0x81, "ADD", "ADD A,C", 4, 0, AddA);
            set[0x82] = new(0x82, "ADD", "ADD A,D", 4, 0, AddA);
            set[0x83] = new(0x83, "ADD", "ADD A,E", 4, 0, AddA);
            set[0x84] = new(0x84, "ADD", "ADD A,H", 4, 0, AddA);
            set[0x85] = new(0x85, "ADD", "ADD A,L", 4, 0, AddA);
            set[0x86] = new(0x86, "ADD", "ADD A,(HL)", 8, 0, AddA);
            set[0x87] = new(0x87, "ADD", "ADD A,A", 4, 0, AddA);
            set[0xC6] = new(0xC6, "ADD", "ADD A,n", 8, 0, AddA);

            // ADC A,n
            set[0x88] = new(0x88, "ADC", "ADC A,B", 4, 0, AdcA);
            set[0x89] = new(0x89, "ADC", "ADC A,C", 4, 0, AdcA);
            set[0x8A] = new(0x8A, "ADC", "ADC A,D", 4, 0, AdcA);
            set[0x8B] = new(0x8B, "ADC", "ADC A,E", 4, 0, AdcA);
            set[0x8C] = new(0x8C, "ADC", "ADC A,H", 4, 0, AdcA);
            set[0x8D] = new(0x8D, "ADC", "ADC A,L", 4, 0, AdcA);
            set[0x8E] = new(0x8E, "ADC", "ADC A,(HL)", 8, 0, AdcA);
            set[0x8F] = new(0x8F, "ADC", "ADC A,A", 4, 0, AdcA);
            set[0xCE] = new(0xCE, "ADC", "ADC A,n", 8, 0, AdcA);

            // INC
            set[0x04] = new(0x04, "INC", "INC B", 4, 0, Inc);
            set[0x0C] = new(0x0C, "INC", "INC C", 4, 0, Inc);
            set[0x14] = new(0x14, "INC", "INC D", 4, 0, Inc);
            set[0x1C] = new(0x1C, "INC", "INC E", 4, 0, Inc);
            set[0x24] = new(0x24, "INC", "INC H", 4, 0, Inc);
            set[0x2C] = new(0x2C, "INC", "INC L", 4, 0, Inc);
            set[0x34] = new(0x34, "INC", "INC (HL)", 12, 0, Inc);
            set[0x3C] = new(0x3C, "INC", "INC A", 4, 0, Inc);
        }

        public static void BenchmarkOpCodes(long N)
        {
            byte[] load = [0xFA, 0x0, 0x0, 0xC3, 0x6, 0x0, 0xFA, 0x0, 0x0, 0x76];
            IMemoryMap memory = new DefaultMemoryMap(load, load);
            IProcessor cpu = new InterpreterProcessor(memory, false);
            long totalCycles = 0;
            var start = DateTime.Now;
            for (var i = 0; i < N; i++)
            {
                cpu.Step(out int cycles);
                cpu.Reset();
                totalCycles += cycles;
            }
            var end = DateTime.Now;
            long time = end.Subtract(start).Milliseconds;
            Console.WriteLine($"N={N}, time used={time}ms, speed={(totalCycles /1000) / time}MHz");
        }
    }
}
