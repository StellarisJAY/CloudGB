namespace CloudGB.Core.PPU
{
    public class Register
    {
        private byte _data = 0;

        public bool GetFlag(byte offset)
        {
            return (_data & (1 << offset)) != 0;
        }

        public void SetFlag(byte offset, bool value)
        {
            _data |= (byte)(value ? (1<<offset) : 0);
        }
    }
}
