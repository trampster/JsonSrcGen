using System;

namespace JsonSrcGen
{
    public interface IJsonBuilder
    {
        IJsonBuilder Append(string value);
        IJsonBuilder Append(byte value);
        IJsonBuilder Append(short value);
        IJsonBuilder Append(ushort value);
        IJsonBuilder Append(int value);
        IJsonBuilder Append(uint value);
        IJsonBuilder Append(long value);
        IJsonBuilder Append(ulong value);
        IJsonBuilder Append(double value);
        IJsonBuilder Append(decimal value);
        IJsonBuilder Append(Guid value);
        IJsonBuilder AppendDate(DateTime date);
        IJsonBuilder AppendEscaped(char input);
        IJsonBuilder AppendEscaped(string input);
        IJsonBuilder AppendBytes(ReadOnlySpan<byte> input);
        
        void Clear();
        string ToString();
    }
}