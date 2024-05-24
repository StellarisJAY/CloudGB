namespace CloudGB.Core.PPU
{
    public class LCDStatus: Register
    {
        public enum PPUMode
        {
            HBlank = 0,
            VBlank = 1,
            OAMScan = 2,
            Drawing = 3,
        }

        public PPUMode Mode
        {
            get
            {
                int flag = (GetFlag(0) ? 1 : 0) | (GetFlag(1) ? 2 : 0);
                return flag switch
                {
                    0 => PPUMode.HBlank,
                    1 => PPUMode.VBlank,
                    2 => PPUMode.OAMScan,
                    3 => PPUMode.Drawing,
                    _ => throw new NotImplementedException("mode value not possible"),
                };
            }
            set
            {
                int flag = value switch
                {
                    PPUMode.HBlank => 0,
                    PPUMode.VBlank => 1,
                    PPUMode.OAMScan => 2,
                    PPUMode.Drawing => 3,
                    _ => throw new NotImplementedException("mode value not possible")
                };
                bool low = (flag & 1) == 1;
                bool high = (flag & 2) != 0;
                SetFlag(0, low);
                SetFlag(1, high);
            }
        }

        // todo implement LY == LYC
        public bool LYEqualsLYC
        {
            get => false;
        }

        public bool Mode0
        {
            get => GetFlag(3);
            set => SetFlag(3, value);
        }
        public bool Mode1
        {
            get => GetFlag(4);
            set => SetFlag(4, value);
        }
        public bool Mode2
        {
            get => GetFlag(5);
            set => SetFlag(5, value);
        }

        public bool LYCSelect
        {
            get => GetFlag(6);
            set => SetFlag(6, value);
        }
    }
}
