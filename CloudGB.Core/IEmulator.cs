using CloudGB.Core.CPU;
using CloudGB.Core.PPU;
namespace CloudGB.Core
{
    public interface IEmulator
    {
        void Start();
        void Pause();
        void Resume();
        void Stop();

        void AddFrameConsumer(GraphicProcessor.FrameConsumer consumer);

        bool DebugStep(out int breakpoint);

        IProcessor CPU();

        void Reset();
    }
}