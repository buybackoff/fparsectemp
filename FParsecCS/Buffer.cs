// Copyright (c) Stephan Tolksdorf 2007-2010
// License: Simplified BSD License. See accompanying documentation.

using System;
using System.Diagnostics;

namespace Spreads.Slang.FParsec
{
    public static class Buffer
    {
        internal static uint SwapByteOrder(uint value)
        {
            return (((value << 24) | (value >> 8)) & 0xff00ff00U)
                    | (((value << 8) | (value >> 24)) & 0x00ff00ffU);
        }

        internal static ulong SwapByteOrder(ulong value)
        {
            return (((value << 56) | (value >> 8)) & 0xff000000ff000000UL)
                   | (((value << 8) | (value >> 56)) & 0x000000ff000000ffUL)
                   | (((value << 40) | (value >> 24)) & 0x00ff000000ff0000UL)
                   | (((value << 24) | (value >> 40)) & 0x0000ff000000ff00UL);
        }

        internal static void SwapByteOrder(uint[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                var v = array[i];
                array[i] = (((v << 24) | (v >> 8)) & 0xff00ff00U)
                           | (((v << 8) | (v >> 24)) & 0x00ff00ffU);
            }
        }

#if LOW_TRUST

        internal static byte[] CopySubarray(byte[] array, int index, int length)
        {
            var subArray = new byte[length];
            System.Buffer.BlockCopy(array, index, subArray, 0, length);
            return subArray;
        }

        internal static uint[] CopyUIntsStoredInLittleEndianByteArray(byte[] src, int srcIndex, int srcLength)
        {
            Debug.Assert(srcLength % sizeof(uint) == 0);
            var subArray = new uint[srcLength / sizeof(uint)];
            System.Buffer.BlockCopy(src, srcIndex, subArray, 0, srcLength);
            if (!BitConverter.IsLittleEndian) SwapByteOrder(subArray);
            return subArray;
        }
#endif
    }
}