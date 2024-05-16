namespace CloudGB.Core.CPU
{
    public interface IProcessor
    {
        /// <summary>
        ///     step the cpu, execute a single command
        /// </summary>
        /// <param name="cycles">output the cycles of last executed command</param>
        /// <returns>true if has next command, false to stop emulation loop</returns>
        public bool Step(out int cycles);

        public void Reset();
    }
}