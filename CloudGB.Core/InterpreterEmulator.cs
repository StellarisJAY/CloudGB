using CloudGB.Core.CPU;
using CloudGB.Core.CPU.Interpreter;
using CloudGB.Core.Memory;
using CloudGB.Core.PPU;

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

        public InterpreterEmulator()
        {
            _memory = new DefaultMemoryMap();
            _cpu = new InterpreterProcessor(_memory, false);
            _ppu = new GraphicProcessor(_memory);
            _bgWorker = new Thread(_ =>
            {
                EmulatorLoop();
            });
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
    }
}
