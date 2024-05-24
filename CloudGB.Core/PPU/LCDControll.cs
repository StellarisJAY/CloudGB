namespace CloudGB.Core.PPU
{
    public class LCDControll: Register
    {
        public bool PPUEnable
        {
            get => GetFlag(7);
            set => SetFlag(7, value);
        }

        public ushort WindowTileMapStart
        {
            get => (ushort)(GetFlag(7) ? 0x9800 : 0x9C00);
        }

        public bool WindowEnable
        {
            get => GetFlag(5);
            set => SetFlag(5, value);
        }

        public ushort BGWindowTileDataStart
        {
            get => (ushort)(GetFlag(4) ? 0x8800 : 0x8000);
        }

        public ushort BGTileMapStart
        {
            get => (ushort)(GetFlag(3) ? 0x9800 : 0x9C00);
        }

        public int ObjSize
        {
            get => GetFlag(2) ? 64 : 128;
        }

        public bool ObjEnable
        {
            get => GetFlag(1);
            set => SetFlag(1, value);
        }

        public bool BGWindowEnablePriority
        {
            get => GetFlag(0);
            set => SetFlag(0, value);
        }
    }
}
