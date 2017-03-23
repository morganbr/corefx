using System;
using System.IO;
using System.Threading.Tasks;

namespace System.Numerics.Hashing
{
    public abstract class HashBase
    {
        /// <summary>
        /// Number of bits produced by the hash
        /// </summary>
        public abstract int HashSize { get; }

        /// <summary>
        /// Core hashing function filled in by implementors. Adds data to the internal
        /// state of the hash.
        /// </summary>
        public abstract void AddIncrementalData(byte[] data, int offset, int count);

        /// <summary>
        /// Both returns the result of the hash of incrementally added data and resets
        /// the internal state to be ready to hash again.
        /// </summary>
        public abstract byte[] GetHashAndReset();

        /// <summary>
        /// Used to indicate that the hash state is currently empty and it can start a new hash.
        /// </summary>
        protected abstract bool StateIsEmpty { get; }

        // Lots of overloads for hashing a single chunk of data and returning the result. 
        // These are intended to cover most usages
        public byte[] ComputeHash(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return ComputeHash(data, 0, data.Length);
        }

        public byte[] ComputeHash(byte[] data, int offset, int count)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (offset < 0 || offset >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0 || count > data.Length - offset)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (!StateIsEmpty)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_NeedToReset);
            }

            AddIncrementalData(data, offset, count);
            return GetHashAndReset();
        }

        public byte[] ComputeHash(Stream data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!StateIsEmpty)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_NeedToReset);
            }

            byte[] streamBuffer = new byte[128];
            int bytesRead = 0;
            do
            {
                bytesRead = data.Read(streamBuffer, 0, streamBuffer.Length);
                AddIncrementalData(streamBuffer, 0, bytesRead);
            } while (bytesRead != 0);

            return GetHashAndReset();
        }

        public async Task<byte[]> ComputeHashAsync(byte[] data)
        {
            return await Task.Run(() => ComputeHash(data));
        }

        public async Task<byte[]> ComputeHashAsync(byte[] data, int offset, int count)
        {
            return await Task.Run(() => ComputeHash(data, offset, count));
        }

        public async Task<byte[]> ComputeHashAsync(Stream data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!StateIsEmpty)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_NeedToReset);
            }

            byte[] streamBuffer = new byte[128];
            int bytesRead = 0;
            do
            {
                bytesRead = await data.ReadAsync(streamBuffer, 0, streamBuffer.Length);
                AddIncrementalData(streamBuffer, 0, bytesRead);
            } while (bytesRead != 0);

            return GetHashAndReset();
        }

    }
}
