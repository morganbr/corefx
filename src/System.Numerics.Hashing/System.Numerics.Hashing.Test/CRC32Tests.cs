using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.Numerics.Hashing.Test
{
    public class CRC32Tests
    {
        [Fact]
        public void TestNoData()
        {
            CRC32 crc = new CRC32();
            byte[] result = crc.GetHashAndReset();
            Assert.Equal(4, result.Length);
            uint crcInt = BitConverter.ToUInt32(result, 0);
            Assert.Equal<uint>(0, crcInt);
        }

        // Test vectors from http://www.wilbaden.com/neil_bawd/crc32.txt
        [Fact]
        public void TestAnAbitraryString()
        {
            CRC32 crc = new CRC32();
            byte[] result = crc.ComputeHash(Encoding.ASCII.GetBytes("An Arbitrary String"));
            Assert.Equal((uint)0x6FBEAAE7, BitConverter.ToUInt32(result, 0));
        }

        [Fact]
        public void TestReverseAlphabet()
        {
            CRC32 crc = new CRC32();
            byte[] result = crc.ComputeHash(Encoding.ASCII.GetBytes("ZYXWVUTSRQPONMLKJIHGFEDBCA"));
            Assert.Equal((uint)0x99CDFDB2, BitConverter.ToUInt32(result, 0));
        }

        [Fact]
        public void TestNumberString()
        {
            CRC32 crc = new CRC32();
            byte[] result = crc.ComputeHash(Encoding.ASCII.GetBytes("123456789"));
            Assert.Equal((uint)0xCBF43926, BitConverter.ToUInt32(result, 0));
        }
    }
}
