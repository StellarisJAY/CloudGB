using CloudGB.Core.CPU;
using CloudGB.Core.CPU.Interpreter;
using CloudGB.Core.Memory;

namespace CloudGB.Core
{
    public class InterpreterEmulator: IEmulator
    {
        private readonly IProcessor _cpu;
        private readonly IMemoryMap _memory;
        public InterpreterEmulator()
        {
            _memory = new DefaultMemoryMap();
            _cpu = new InterpreterProcessor(_memory, false);
        }

        void IEmulator.Pause()
        {
            throw new NotImplementedException();
        }

        void IEmulator.Resume()
        {
            throw new NotImplementedException();
        }

        void IEmulator.Start()
        {
            throw new NotImplementedException();
        }

        void IEmulator.Stop()
        {
            throw new NotImplementedException();
        }
    }
}
