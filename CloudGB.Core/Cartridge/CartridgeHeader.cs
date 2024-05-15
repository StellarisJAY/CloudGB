using System.Text;
using System.Text.Unicode;

namespace CloudGB.Core.Cartridge
{
    public class CartridgeHeader
    {
        public byte[] EntryPoint { get; } = new byte[4];
        public string? Title { get; private set; }
        public string? Manufacturer { get; private set; }
        public byte CGBFlag { get; private set; }
        public byte Type { get; private set; }

        private byte ROMSize0;
        public int ROMSize {
            get {
                return ROMSize0 switch
                {
                    <=8 => (ROMSize0 + 1) * (32 << 20),
                    _ => 0,
                };
            }
        }

        public static CartridgeHeader Parse(byte[] cartridgeData)
        {
            if (cartridgeData.Length <= 0x014F)
                throw new IndexOutOfRangeException("invalid cartridge data length");
            CartridgeHeader header = new();
            // 0x100~0x103 entry point instructions
            header.EntryPoint[0] = cartridgeData[0x100];
            header.EntryPoint[1] = cartridgeData[0x101];
            header.EntryPoint[2] = cartridgeData[0x102];
            header.EntryPoint[3] = cartridgeData[0x103];
            // 0x134~0x144 game title and manufacturer
            header.Title = Encoding.ASCII.GetString(cartridgeData[0x134..0x13F]);
            Console.WriteLine(cartridgeData[0x134..0x13F][0]);
            header.Manufacturer = Encoding.ASCII.GetString(cartridgeData[0x13F..0x143]);
            // 0x143 CGBFlag
            header.CGBFlag = cartridgeData[0x143];
            // 0x147 mapper type
            header.Type = cartridgeData[0x147];
            header.ROMSize0 = cartridgeData[0x148];
            return header;
        }
    }
}
