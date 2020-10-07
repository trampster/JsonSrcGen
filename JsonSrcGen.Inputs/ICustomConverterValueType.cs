using System;
using System.Text;

namespace JsonSrcGen
{
    internal interface ICustomConverterValueType<T> where T : struct
    {
        void ToJson(IJsonBuilder builder, T target);

        ReadOnlySpan<char> FromJson(ReadOnlySpan<char> span, out T value);
    }
}