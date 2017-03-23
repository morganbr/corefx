using System;
using System.IO;
using Xunit;

namespace System.Numerics.Hashing.Test
{
    public class HashBaseTests
    {
        [Fact]
        public void TestNullBytes()
        {
            AppendHash hash = new AppendHash();
            Assert.Throws<ArgumentNullException>(() => hash.ComputeHash((byte[])null));
        }

        [Fact]
        public void Test1Byte()
        {
            AppendHash hash = new AppendHash();
            byte[] hashBytes = hash.ComputeHash(new byte[1] { 1 });
            Assert.Equal(1, hashBytes.Length);
            Assert.Equal(1, hashBytes[0]);
        }

        [Fact]
        public void TestStream()
        {
            AppendHash hash = new AppendHash();
            MemoryStream inputStream = new MemoryStream();
            inputStream.WriteByte(3);
            inputStream.Position = 0;
            byte[] hashBytes = hash.ComputeHash(inputStream);
            Assert.Equal(1, hashBytes.Length);
            Assert.Equal(3, hashBytes[0]);
        }
        
        [Fact]
        public void TestLongStream()
        {
            AppendHash hash = new AppendHash();
            MemoryStream inputStream = new MemoryStream();
            const int streamLen = 129;
            for(int i = 0; i < streamLen; i++)
            {
                inputStream.WriteByte((byte)i);
            }

            inputStream.Position = 0;
            byte[] hashBytes = hash.ComputeHash(inputStream);

            Assert.Equal(streamLen, hashBytes.Length);

            for (int i = 0; i < streamLen; i++)
            {
                Assert.Equal(i, hashBytes[i]);
            }
        }
    }
}
