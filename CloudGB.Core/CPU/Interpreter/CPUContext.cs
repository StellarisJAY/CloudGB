namespace CloudGB.Core.CPU.Interpreter
{
    public class CPUContext
    {
        public ushort AF { get; set; }
        public ushort BC { get; set; }
        public ushort DE { get; set; }
        public ushort SP { get; set; }
        public ushort PC { get; set; }
        public ushort HL { get; set; }

        public bool Halt { get; set; }

        public long TotalCycles { get; set; }
        public int LastCycles { get; set; }

        public bool InterruptEnable { get; set; }

        public byte A
        {
            get => (byte)(AF >> 8);
            set => AF = (ushort)((AF & 0xFF) | (value << 8));
        }

        public byte F
        {
            get => (byte)(AF & 0xFF);
            set => AF = (ushort)((AF & 0xFF00) | (value & 0xFF));
        }

        public byte B
        {
            get => (byte)(BC >> 8);
            set => BC = (ushort)((BC & 0xFF) | (value << 8));
        }

        public byte C
        {
            get => (byte)(BC & 0xFF);
            set => BC = (ushort)((BC & 0xFF00) | (value & 0xFF));
        }

        public byte D
        {
            get => (byte)(DE >> 8);
            set => DE = (ushort)((DE & 0xFF) | (value << 8));
        }

        public byte E
        {
            get => (byte)(DE & 0xFF);
            set => DE = (ushort)((DE & 0xFF00) | (value & 0xFF));
        }

        public byte H
        {
            get => (byte)(HL >> 8);
            set => HL = (ushort)((HL & 0xFF) | (value << 8));
        }

        public byte L
        {
            get => (byte)(HL & 0xFF);
            set => HL = (ushort)((HL & 0xFF00) | (value & 0xFF));
        }

        public bool ZeroFlag
        {
            get => GetFlag(7);
            set => SetFlag(7, value);
        }

        public bool SubstractFlag
        {
            get => GetFlag(6);
            set => SetFlag(6, value);
        }

        public bool HalfCarryFlag
        {
            get => GetFlag(5);
            set => SetFlag(5, value);
        }

        public bool CarryFlag
        {
            get => GetFlag(4);
            set => SetFlag(4, value);
        }

        private bool GetFlag(int offset)
        {
            return (F & (1 << offset)) != 0;
        }

        private void SetFlag(int offset, bool value)
        { 
            if (value) F |= (byte)(1 << offset);
            else F &= (byte)(~(1 << offset));
        }
    }
}