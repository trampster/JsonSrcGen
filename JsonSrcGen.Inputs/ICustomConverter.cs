using System;
using System.Text;

namespace JsonSrcGen
{
    internal interface ICustomConverter<T>
    {
        void ToJson(IJsonBuilder builder, T target);

        ReadOnlySpan<char> FromJson(ReadOnlySpan<char> span, T value);
    }
}