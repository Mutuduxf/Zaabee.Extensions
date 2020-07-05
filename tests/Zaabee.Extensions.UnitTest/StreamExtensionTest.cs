using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Zaabee.Extensions.UnitTest.Commons;

namespace Zaabee.Extensions.UnitTest
{
    public class StreamExtensionTest
    {
        [Fact]
        public void TrySeekTest()
        {
            var ms = new MemoryStream();
            var bytes = new byte[1024];
            for (var i = 0; i < bytes.Length; i++) bytes[i] = (byte) (i % (byte.MaxValue + 1));
            Assert.True(ms.TryWrite(bytes, 0, 1024));
            Assert.Equal(1024, ms.Position);
            Assert.Equal(0, ms.TrySeek(0, SeekOrigin.Begin));
            Assert.Equal(0, ms.Position);
        }

        [Fact]
        public void TryReadWriteTest()
        {
            var ms = new MemoryStream();
            var bytes = new byte[1024];
            var result = new byte[1024];
            for (var i = 0; i < bytes.Length; i++) bytes[i] = (byte) (i % (byte.MaxValue + 1));
            Assert.True(ms.TryWrite(bytes, 0, 1024));
            Assert.Equal(0, ms.TrySeek(0, SeekOrigin.Begin));
            Assert.Equal(1024, ms.TryRead(result, 0, 1024));
            Assert.True(BytesEqual(bytes, result));
        }

        [Fact]
        public async Task TryReadWriteAsyncTest()
        {
            var ms = new MemoryStream();
            var bytes = new byte[1024];
            var result = new byte[1024];
            for (var i = 0; i < bytes.Length; i++) bytes[i] = (byte) (i % (byte.MaxValue + 1));
            Assert.True(await ms.TryWriteAsync(bytes, 0, 1024));
            Assert.Equal(0, ms.TrySeek(0, SeekOrigin.Begin));
            Assert.Equal(1024, await ms.TryReadAsync(result, 0, 1024));
            Assert.True(BytesEqual(bytes, result));
        }

        [Fact]
        public void TryWriteReadByteTest()
        {
            var ms = new MemoryStream();
            var bytes = new byte[1024];
            var result = new byte[1024];
            for (var i = 0; i < bytes.Length; i++) bytes[i] = (byte) (i % (byte.MaxValue + 1));
            for (var i = 0; i < bytes.Length; i++) ms.TryWriteByte(bytes[i]);
            Assert.Equal(0, ms.TrySeek(0, SeekOrigin.Begin));
            for (var i = 0; i < result.Length; i++) result[i] = (byte) ms.TryReadByte();
            Assert.True(BytesEqual(bytes, result));
        }

        [Fact]
        public void TrySetReadTimeoutTest()
        {
            var stream = new FakeNetworkStream(new MemoryStream());
            Assert.True(stream.TrySetReadTimeout(1000));
            Assert.Equal(1000, stream.ReadTimeout);
            var timeSpan = TimeSpan.FromMinutes(1);
            Assert.True(stream.TrySetReadTimeout(timeSpan));
            Assert.Equal(timeSpan.Milliseconds, stream.ReadTimeout);
        }

        [Fact]
        public void TrySetWriteTimeoutTest()
        {
            var stream = new FakeNetworkStream(new MemoryStream());
            Assert.True(stream.TrySetWriteTimeout(1000));
            Assert.Equal(1000, stream.WriteTimeout);
            var timeSpan = TimeSpan.FromMinutes(1);
            Assert.True(stream.TrySetWriteTimeout(timeSpan));
            Assert.Equal(timeSpan.Milliseconds, stream.WriteTimeout);
        }

        [Fact]
        public void ReadToEndTest()
        {
            MemoryStream ms = null;
            Assert.Empty(ms.ReadToEnd());
            ms = new MemoryStream();
            Assert.Empty(ms.ReadToEnd());
            
            var msBytes = new byte[1024];
            for (var i = 0; i < msBytes.Length; i++) msBytes[i] = (byte) (i % (byte.MaxValue + 1));
            for (var i = 0; i < msBytes.Length; i++) ms.TryWriteByte(msBytes[i]);
            Assert.Equal(0, ms.TrySeek(0, SeekOrigin.Begin));
            var msResult = ms.ReadToEnd();
            Assert.True(BytesEqual(msBytes, msResult));

            var ns = new FakeNetworkStream(new MemoryStream());
            var nsBytes = new byte[1024];
            for (var i = 0; i < nsBytes.Length; i++) nsBytes[i] = (byte) (i % (byte.MaxValue + 1));
            for (var i = 0; i < nsBytes.Length; i++) ns.TryWriteByte(nsBytes[i]);
            Assert.Equal(0, ns.TrySeek(0, SeekOrigin.Begin));
            var nsResult = ns.ReadToEnd();
            Assert.True(BytesEqual(nsBytes, nsResult));
        }

        private static bool BytesEqual(byte[] first, byte[] second)
        {
            if (first.IsNullOrEmpty() || second.IsNullOrEmpty()) return false;
            return !first.Where((t, i) => t != second[i]).Any();
        }
    }
}