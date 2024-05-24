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
        
        public GraphicProcessor(IMemoryMap memory)
        {
            _lcdControll = new LCDControll();
            _lcdStatus = new LCDStatus();
            _frame = new byte[FrameWidth * FrameHeight * 3];
            _memory = memory;
        }

        public void Step(int cpuCycles)
        {
            if (_scanline >= 144)
            {
                _lcdStatus.Mode = LCDStatus.PPUMode.VBlank;
            }else
            {
                _dots += cpuCycles;
                if (_dots <= 80)
                {
                    _lcdStatus.Mode = LCDStatus.PPUMode.OAMScan;
                }else if (_dots <= 80 + 172)
                {
                    _lcdStatus.Mode = LCDStatus.PPUMode.Drawing;
                }else if (_dots < 456)
                {
                    _lcdStatus.Mode = LCDStatus.PPUMode.HBlank;
                }else
                {
                    _scanline++;
                    _dots -= 456;
                }
            }
            foreach(var consumer in FrameConsumers)
            {
                consumer(_frame, out uint timeUsed);
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
