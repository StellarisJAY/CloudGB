namespace CloudGB.Core.CPU
{
    public class CPUContext
    {
        public ushort AF {get; set;}
        public ushort BC {get; set;}
        public ushort DE {get; set;}
        public ushort SP {get; set;}
        public ushort PC {get; set;}
        public ushort HL {get; set;}

        public byte A 
        {
            get => (byte)(AF >> 8);
            set => AF |= (ushort)(value << 8);
        }

        public byte F
        {
            get => (byte)(AF & 0xFF);
            set => AF |= (ushort)(value & 0xFF);
        }

        public byte B 
        {
            get => (byte)(BC >> 8);
            set => BC |= (ushort)(value << 8);
        }

        public byte C
        {
            get => (byte)(BC & 0xFF);
            set => BC |= (ushort)(value & 0xFF);
        }

        public byte D 
        {
            get => (byte)(DE >> 8);
            set => DE |= (ushort)(value << 8);
        }

        public byte E
        {
            get => (byte)(DE & 0xFF);
            set => DE |= (ushort)(value & 0xFF);
        }

        public byte H 
        {
            get => (byte)(HL >> 8);
            set => HL |= (ushort)(value << 8);
        }

        public byte L
        {
            get => (byte)(HL & 0xFF);
            set => HL |= (ushort)(value & 0xFF);
        }
    }
}