namespace CloudGB.Core.PPU
{
    public class PPU
    {
        public static readonly int FrameWidth = 160;
        public static readonly int FrameHeight = 144;

        public delegate void FrameConsumer(byte[] frame, out uint timeUsed);

        public List<FrameConsumer> FrameConsumers { get; private set; }

        private LCDControll _lcdControll;
        private LCDStatus _lcdStatus;



    }
}
