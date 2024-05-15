namespace CloudGB.Core.Memory
{
    public interface IMemoryMap
    {
        /// <summary>
        ///     Read data from an address.
        /// </summary>
        /// <param name="address">16bits address</param>
        /// <param name="data">output data</param>
        /// <returns>true if address is readable</returns>
        public bool Read(ushort address, out byte data);

        /// <summary>
        ///     Write data to an address.
        /// </summary>
        /// <param name="address">16bits address</param>
        /// <param name="data">data</param>
        /// <returns>true if address is writable</returns>
        public bool Write(ushort address, byte data);

        public bool Read16Bit(ushort address, out ushort data);

        public bool Write16Bit(ushort address, ushort data);
    }
}