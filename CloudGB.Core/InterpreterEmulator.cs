using CloudGB.Core.CPU;
using CloudGB.Core.CPU.Interpreter;
using CloudGB.Core.Memory;
using CloudGB.Core.PPU;
using System.Diagnostics;

namespace CloudGB.Core
{
    public class InterpreterEmulator: IEmulator
    {
        private readonly IProcessor _cpu;
        private readonly IMemoryMap _memory;
        private readonly GraphicProcessor _ppu;
        private Thread _bgWorker;
        private object _mutex = new();
        private long _status = 0;

        private long _cyclesCounter = 0;

        static readonly long StatusInit = 0;
        static readonly long StatusRunning = 1;
        static readonly long StatusPaused = 2;
        static readonly long StatusStopped = 3;
        static readonly long StatusDebug = 4;


        public InterpreterEmulator(byte[] rom, bool debug)
        {
            _memory = new DefaultMemoryMap(rom[0..0x4000], rom[0x4000..0x8000]);
            _cpu = new InterpreterProcessor(_memory, debug);
            _ppu = new GraphicProcessor(_memory);
            _memory.SetPPUReader(_ppu.Read);
            _bgWorker = new Thread(_ =>
            {
                EmulatorLoop();
            });
            if(debug)
            {
                _status = StatusDebug;
            }
        }

        private void EmulatorLoop()
        {
            while(true)
            {
                var status = Interlocked.Read(ref _status);
                if (status == StatusStopped)
                {
                    break;
                }
                if (status == StatusPaused)
                {
                    // wait for signal to wake up
                    InternalPauseBgWorker();
                }
                bool halt = _cpu.Step(out int cycles, out int breakpoint);
                if (!halt)
                {
                    _ppu.Step(cycles);   
                }else
                {
                    break;
                }
            }
        }

        void IEmulator.AddFrameConsumer(GraphicProcessor.FrameConsumer consumer)
        {
            _ppu.FrameConsumers.Add(consumer);
        }

        void IEmulator.Pause()
        {
            if (Interlocked.CompareExchange(ref _status, StatusPaused, StatusRunning) != StatusRunning)
            {
                throw new InvalidOperationException("unable to change emulator status to Paused, emulator currently is not Running");
            }
        }

        void IEmulator.Resume()
        {
            if (Interlocked.CompareExchange(ref _status, StatusRunning, StatusPaused) == StatusPaused)
            {
                // wake up _bgWorker that is waiting
                InternalResumeBgWorker();
            }else
            {
                throw new InvalidOperationException("unable to change emulator status to Running, emulator currently is not Paused");
            }
        }

        void IEmulator.Start()
        {
            if (Interlocked.CompareExchange(ref _status, StatusRunning, StatusInit) == StatusInit)
            {
                _bgWorker.Start();
            }else
            {
                throw new InvalidOperationException("unable to start emulator, emulator currently is not in INIT State");
            }
        }

        void IEmulator.Stop()
        {
            if (Interlocked.CompareExchange(ref _status, StatusStopped, StatusRunning) == StatusRunning)
            {
                
            }
            else if (Interlocked.CompareExchange(ref _status, StatusStopped, StatusPaused) == StatusPaused)
            {

            }else
            {
                throw new InvalidOperationException("unable to stop emulator");
            }
        }

        private void InternalPauseBgWorker()
        {
            lock(_mutex)
            {
                Monitor.Wait(_mutex);
            }
        }

        private void InternalResumeBgWorker()
        {
            lock(_mutex)
            {
                Monitor.Pulse(_mutex);
            }
        }

        bool IEmulator.DebugStep(out int breakpoint)
        {
            bool hasNext = _cpu.Step(out int cycles, out breakpoint);
            if (hasNext)
            {
                _ppu.Step(cycles * 4);
            }
            return hasNext;
        }

        IProcessor IEmulator.CPU()
        {
            return _cpu;
        }

        public void Reset()
        {
            _cpu.Reset();
        }
    }
}
