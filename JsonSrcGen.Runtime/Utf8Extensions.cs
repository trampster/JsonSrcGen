using System;
using System.Runtime.CompilerServices;

namespace JsonSrcGen.Runtime
{
    public static class Utf8Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteAscii(this byte[] utf8, int start, string value)
        {
            unsafe
            {
                int length = value.Length*2;
                fixed (void* voidPtr = value)
                fixed (byte* utf8Ptr = utf8)
                {
                    byte* spanPtr = (byte*)voidPtr;
                    int utf8Index = start;
                    for(int index = 0; index < length; index+=2, utf8Index++)
                    {
                        utf8Ptr[utf8Index] = spanPtr[index];
                    }
                    return start+value.Length;
                }
            }
        }
    }
}
