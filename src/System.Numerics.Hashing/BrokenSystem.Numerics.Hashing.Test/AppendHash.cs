using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Numerics.Hashing.Test
{
    class AppendHash : HashBase
    {
        MemoryStream _state = null;

        public override int HashSize => throw new NotImplementedException();

        protected override bool StateIsEmpty => _state == null;

        public override void AddIncrementalData(byte[] data, int offset, int count)
        {
            if(_state == null)
            {
                _state = new MemoryStream();
            }
            _state.Write(data, offset, count);
        }

        public override byte[] GetHashAndReset()
        {
            byte[] hash = new byte[_state.Length];
            _state.Write(hash, 0, (int)_state.Length);
            _state = null;
            return hash;
        }
    }
}
