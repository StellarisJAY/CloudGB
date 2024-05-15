using CloudGB.Core.CPU;
using CloudGB.Core.Memory;
using System.Text;
using CloudGB.Core.Cartridge;
try
{
    using StreamReader reader = new(@"D:\code\other\game-boy-test-roms\blargg\cpu_instrs\individual\06-ld r,r.gb");
    string s = reader.ReadToEnd();
    byte[] data = Encoding.ASCII.GetBytes(s);

    var header = CartridgeHeader.Parse(data);
    Console.WriteLine(header.ROMSize >> 20);

    byte[] rom1 = [0x0, 0x18, 0b11111111];
    IMemoryMap memory = new DefaultMemoryMap(rom1, rom1);
    IProcessor cpu = new InterpreterProcessor(memory);
    while(cpu.Step(out int cycles))
    {
    }
}catch(Exception e)
{
    Console.WriteLine(e.Message);
}

