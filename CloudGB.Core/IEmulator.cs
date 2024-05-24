namespace CloudGB.Core
{
    public interface IEmulator
    {
        void Start();
        void Pause();
        void Resume();
        void Stop();
    }
}