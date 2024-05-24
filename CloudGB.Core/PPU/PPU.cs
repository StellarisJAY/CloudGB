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

        public GraphicProcessor()
        {
            _lcdControll = new LCDControll();
            _lcdStatus = new LCDStatus();
        }
    }
}
