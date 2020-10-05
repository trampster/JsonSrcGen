using System;
using System.Text;

namespace JsonSrcGen
{
    internal interface ICustomConverterValueType<T>
    {
        void ToJson(IJsonBuilder builder, T target);

        ReadOnlySpan<char> FromJson(ReadOnlySpan<char> span, out T value);
    }
}