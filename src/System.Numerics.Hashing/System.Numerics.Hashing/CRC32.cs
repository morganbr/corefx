// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace System.Numerics.Hashing
{
    public class CRC32 : HashBase
    {
        // Table of CRCs of all 8-bit messages.
        private static ulong[] s_crc_table = new ulong[256];

        // Flag: has the table been computed? Initially false.
        private static bool s_crc_table_computed = false;

        private ulong _state = 0xffffffffL;
        private bool _stateIsEmpty = true;

        public override int HashSize => 64;

        protected override bool StateIsEmpty => _stateIsEmpty;

        // Make the table for a fast CRC.
        // Derivative work of zlib -- https://github.com/madler/zlib/blob/master/crc32.c (hint: L108)
        private static void make_crc_table()
        {
            ulong c;
            int n, k;

            for (n = 0; n < 256; n++)
            {
                c = (ulong)n;
                for (k = 0; k < 8; k++)
                {
                    if ((c & 1) > 0)
                        c = 0xedb88320L ^ (c >> 1);
                    else
                        c = c >> 1;
                }
                s_crc_table[n] = c;
            }
            s_crc_table_computed = true;
        }

        // Update a running CRC with the bytes buf[0..len-1]--the CRC
        // should be initialized to all 1's, and the transmitted value
        // is the 1's complement of the final running CRC (see the
        // crc() routine below)).
        private void update_crc(byte[] buf, int offset, int count)
        {
            ulong c = _state;
            int n;

            if (!s_crc_table_computed)
                make_crc_table();
            for (n = offset; n < count; n++)
            {
                c = s_crc_table[(c ^ buf[n]) & 0xff] ^ (c >> 8);
            }
            _state = c;
        }

        public override void AddIncrementalData(byte[] data, int offset, int count)
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

            if (_stateIsEmpty)
            {
                _state = 0xffffffffL;
                _stateIsEmpty = false;
            }

            update_crc(data, offset, count);
        }

        public override byte[] GetHashAndReset()
        {
            uint hash = (uint)(_state ^ 0xffffffffL);
            _state = 0xffffffffL;
            _stateIsEmpty = true;
            return BitConverter.GetBytes(hash);
        }
    }
}
