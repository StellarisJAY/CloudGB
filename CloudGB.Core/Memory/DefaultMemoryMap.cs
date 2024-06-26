namespace CloudGB.Core.Memory
{
    public class DefaultMemoryMap: IMemoryMap
    {
        /// <summary>
        ///     4KiB work ram
        /// </summary>
        private byte[] WRAM0;
        /// <summary>
        ///     4KiB work ram, switchable in CGB mode
        /// </summary>
        private byte[] WRAM1;
        /// <summary>
        ///     16KiB fixed ROM
        /// </summary>
        private byte[] ROM0;
        /// <summary>
        ///     16KiB swichable ROM
        /// </summary>
        private byte[] ROM1;

        /// <summary>
        ///     Interrupt Enable Register at 0xFFFF
        /// </summary>
        private byte InterruptEnable;

        /// <summary>
        ///     127B High RAM
        /// </summary>
        private byte[] HighRAM;

        private byte DebugBuffer;

        private IMemoryMap.PPUReader _ppuReader = _ => 0;

        public DefaultMemoryMap(byte[] rom0, byte[] rom1)
        {
            ROM0 = rom0;
            ROM1 = rom1;
            WRAM0 = new byte[4096];
            WRAM1 = new byte[4096];
            HighRAM = new byte[128];
        }

        public DefaultMemoryMap()
        {
            ROM0 = new byte[0x4000];
            ROM1 = new byte[0x4000];
            WRAM0 = new byte[4096];
            WRAM1 = new byte[4096];
            HighRAM = new byte[128];
        }

        public bool Read(ushort address, out byte data)
        {
            bool readable = true;
            data = 0;
            switch (address)
            {
                case <= 0x3FFF:
                    data = ROM0[address];
                    break;
                case <= 0x7FFF:
                    data = ROM1[address - 0x4000];
                    break;
                case <= 0x9FFF:
                    break;
                case <= 0xBFFF:
                    break;
                case <= 0xCFFF:
                    data = WRAM0[address & 0xFFF];
                    break;
                case <= 0xDFFF:
                    data = WRAM1[address & 0xFFF];
                    break;
                case <= 0xFDFF:
                    readable = false;
                    break;
                case <= 0xFE9F:
                    break;
                case <= 0xFEFF:
                    readable = false;
                    break;
                case 0xFF44:
                    data = _ppuReader(address);
                    break;
                case <= 0xFF7F:
                    break;
                case <= 0xFFFE:
                    data = HighRAM[address - 0xFF80];
                    break;
                case 0xFFFF:
                    data = InterruptEnable;
                    break;
            }
            return readable;
        }

        public bool Write(ushort address, byte data)
        {
            bool writable = true;
            switch (address)
            {
                case <= 0x3FFF:
                    ROM1[address] = data;
                    break;
                case <= 0x7FFF:
                    ROM1[address - 0x4000] = data;
                    break;
                case <= 0x9FFF:
                    break;
                case <= 0xBFFF:
                    break;
                case <= 0xCFFF:
                    WRAM0[address & 0xFFF] = data;
                    break;
                case <= 0xDFFF:
                    WRAM1[address & 0xFFF] = data;
                    break;
                case <= 0xFDFF:
                    writable = false;
                    break;
                case <= 0xFE9F:
                    break;
                case <= 0xFEFF:
                    writable = false;
                    break;
                case 0xFF01:
                    DebugBuffer = data;
                    break;
                case 0xFF02:
                    if (data == 0x81)
                        Console.Write((char)DebugBuffer);
                    break;
                case <= 0xFF7F:
                    break;
                case <= 0xFFFE:
                    HighRAM[address - 0xFF80] = data;
                    break;
                case 0xFFFF:
                    InterruptEnable = data;
                break;
            }
            return writable;
        }

        bool IMemoryMap.Read16Bit(ushort address, out ushort data)
        {
            bool lowReadable = Read(address, out byte low);
            bool highReadable = Read((ushort)(address + 1), out byte high);
            data = (ushort)(low&0xFF | (high << 8));
            return lowReadable && highReadable;
        }

        bool IMemoryMap.Write16Bit(ushort address, ushort data)
        {
            byte low = (byte)(data & 0xFF);
            byte high = (byte)(data >> 8);
            return Write(address, low) && Write((ushort)(address + 1), high);
        }

        void IMemoryMap.SetPPUReader(IMemoryMap.PPUReader reader)
        {
            _ppuReader = reader;
        }
    }
}