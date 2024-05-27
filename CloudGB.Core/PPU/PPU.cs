using CloudGB.Core.Memory;

namespace CloudGB.Core.PPU
{
    public class GraphicProcessor
    {
        public static readonly int FrameWidth = 160;
        public static readonly int FrameHeight = 144;

        public delegate void FrameConsumer(byte[] frame, out uint timeUsed);

        public List<FrameConsumer> FrameConsumers = [];

        private LCDControll _lcdControll;
        private LCDStatus _lcdStatus;

        private byte[] _frame;

        private IMemoryMap _memory;

        private int _dots = 0;
        private int _scanline = 0;

        public int LY
        {
            get => _scanline;
        }
        
        public GraphicProcessor(IMemoryMap memory)
        {
            _lcdControll = new LCDControll();
            _lcdStatus = new LCDStatus();
            _frame = new byte[FrameWidth * FrameHeight * 3];
            _memory = memory;
        }

        public byte Read(ushort address)
        {
            return address switch
            {
                0xFF44 => 0x90,
                _ => 0,
            };
        }

        public void Step(int cpuCycles)
        {
            if (_scanline < 144)
            {
                _dots += cpuCycles;
                if (_dots <= 80)
                {
                    _lcdStatus.Mode = LCDStatus.PPUMode.OAMScan;
                }
                else if (_dots <= 80 + 172)
                {
                    _lcdStatus.Mode = LCDStatus.PPUMode.Drawing;
                }
                else if (_dots < 456)
                {
                    _lcdStatus.Mode = LCDStatus.PPUMode.HBlank;
                }
                else
                {
                    _scanline++;
                    _dots -= 456;
                }
            } 
            else if (_scanline < 153)
            {
                _lcdStatus.Mode = LCDStatus.PPUMode.VBlank;
                _dots += cpuCycles;
                if (_dots >= 456)
                {
                    _scanline++;
                    _dots -= 456;
                }
            }else
            {
                _scanline = 0;
                _dots = 0;
            }
        }

        private void RenderBackground()
        {

        }

        private void RenderWindow()
        {

        }

        private void RenderObj()
        {

        }
    }
}
